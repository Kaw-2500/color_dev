using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Camerapos : MonoBehaviour
{
    public Transform playerTransform;
    [SerializeField] float camerahight = 5f;
    [SerializeField] float cameradistance = 5f;
    [SerializeField] float cameraSmoothTime = 0.1f;
    [SerializeField] float parryDurationSeconds = 0.6f;
    [SerializeField] float zoomHeight = 1.5f;        // ズーム時の高さ補正
    [SerializeField] CameraPositionManager cpm;//略してる

    public Transform Cameratransform;

    private bool isParryZooming = false;
    bool Finisheddeadaction;

    private float smoothY;
    private float velocityY;

    Vector3 parryHopeposition;
    Vector3 deadHopeposition;
    Vector3 followPlayerHopePosition;

    [SerializeField] private PlayerStateManager playerStateManager;
    public Camera mainCamera;

    [SerializeField] private float parryZoomSize = 3f;
    [SerializeField] private float parryZoomSpeed = 0.2f;

    private Coroutine parryZoomCoroutine;
    private Vector3 shakeOffset = Vector3.zero; // シェイク用オフセット
    private Tween shakeTween; // DOTweenのシェイク管理用

    void Start()
    {
        if (playerTransform == null)
        {
            Debug.LogError("PlayerTransform is not assigned.");
            return;
        }
        if (playerStateManager == null)
        {
            Debug.LogError("PlayerStateManager is not assigned.");
            return;
        }
        if (mainCamera == null)
            mainCamera = Camera.main;

        smoothY = transform.position.y;
    }

    private void Update()
    {
        if (!Finisheddeadaction) return;
        if (isParryZooming) return;
        ApplyCameraPosition(new Vector3(
            playerTransform.position.x,
            playerTransform.position.y,
            Cameratransform.position.z
        ));
    }

    void LateUpdate()
    {
        if (playerStateManager.IsDead || isParryZooming)
        {
            return;
        }

        FollowPlayerYOnly();
    }

    void FollowPlayerYOnly()
    {
        float targetGroundY = playerStateManager.GroundYpos;
        if (float.IsNaN(targetGroundY))
        {
            targetGroundY = playerTransform.position.y;
        }

        smoothY = Mathf.SmoothDamp(smoothY, targetGroundY + camerahight, ref velocityY, cameraSmoothTime);

        Vector3 newPos = new Vector3(
            playerTransform.position.x + cameradistance,
            smoothY,
            Cameratransform.position.z
        );
        ApplyCameraPosition(newPos);
    }

    public IEnumerator DeadCameraEffect(float targetOrthoSize, float duration)
    {
        if (parryZoomCoroutine != null)
        {
            StopCoroutine(parryZoomCoroutine);
            parryZoomCoroutine = null;
        }

        Vector3 startPos = Cameratransform.position;
        float startOrtho = mainCamera.orthographicSize;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(elapsed / duration);
            t = 1f - Mathf.Pow(1f - t, 2f); // イーズアウト

            Vector3 targetPos = new Vector3(
                playerTransform.position.x,
                playerTransform.position.y,
                Cameratransform.position.z
            );

            ApplyCameraPosition(Vector3.Lerp(startPos, targetPos, t));
            mainCamera.orthographicSize = Mathf.Lerp(startOrtho, targetOrthoSize, t);

            yield return null;
        }

        Vector3 finalPos = new Vector3(
            playerTransform.position.x,
            playerTransform.position.y,
            Cameratransform.position.z
        );
        ApplyCameraPosition(finalPos);
        mainCamera.orthographicSize = targetOrthoSize;
        Finisheddeadaction = true;
    }

    public void StartParryZoom(float parryTime)
    {
        if (parryZoomCoroutine != null)
        {
            StopCoroutine(parryZoomCoroutine);
            parryZoomCoroutine = null;
        }
        isParryZooming = true;
        parryZoomCoroutine = StartCoroutine(ParryZoomCoroutine(parryTime));
    }

private IEnumerator ParryZoomCoroutine(float parryTime)
{
    Vector3 originalPos = Cameratransform.position;
    float originalOrtho = mainCamera.orthographicSize;

    // --- ズームイン ---
    float zoomInDuration = parryTime * 0.2f;
    float elapsed = 0f;
    while (elapsed < zoomInDuration)
    {
        elapsed += Time.unscaledDeltaTime;
        float t = Mathf.Clamp01(elapsed / zoomInDuration);
        t = 1f - Mathf.Pow(1f - t, 2f); // イーズアウト

        Vector3 targetPos = new Vector3(
            playerTransform.position.x,
            playerTransform.position.y + zoomHeight,
            originalPos.z
        );

        ApplyCameraPosition(Vector3.Lerp(originalPos, targetPos, t));
        mainCamera.orthographicSize = Mathf.Lerp(originalOrtho, parryZoomSize, t);
        yield return null;
    }

    // ズームイン完了状態を明示的に設定
    Vector3 zoomedPos = new Vector3(
        playerTransform.position.x,
        playerTransform.position.y + zoomHeight,
        originalPos.z
    );
    ApplyCameraPosition(zoomedPos);
    mainCamera.orthographicSize = parryZoomSize;

    // --- 演出維持時間 ---
    float holdDuration = parryTime * 0.6f;
    elapsed = 0f;
    while (elapsed < holdDuration)
    {
        elapsed += Time.unscaledDeltaTime;
        zoomedPos = new Vector3(
            playerTransform.position.x,
            playerTransform.position.y + zoomHeight,
            originalPos.z
        );
        ApplyCameraPosition(zoomedPos);
        yield return null;
    }

    // --- ズームアウト ---
    float zoomOutDuration = parryTime * 0.2f;
    Vector3 zoomOutStartPos = Cameratransform.position;
    float zoomOutStartOrtho = mainCamera.orthographicSize;

    elapsed = 0f;
    while (elapsed < zoomOutDuration)
    {
        elapsed += Time.unscaledDeltaTime;
        float t = Mathf.Clamp01(elapsed / zoomOutDuration);
        t = 1f - Mathf.Pow(1f - t, 2f); // イーズアウト

        ApplyCameraPosition(Vector3.Lerp(zoomOutStartPos, originalPos, t));
        mainCamera.orthographicSize = Mathf.Lerp(zoomOutStartOrtho, originalOrtho, t);
        yield return null;
    }

    // ズームアウト完了状態を明示的に設定
    ApplyCameraPosition(originalPos);
    mainCamera.orthographicSize = originalOrtho;

    isParryZooming = false;
    parryZoomCoroutine = null;
}

    private Vector3 GetBaseCameraPosition(Vector3 basePos)
    {
        return basePos;
    }

    private void ApplyCameraPosition(Vector3 basePos)
    {
        Cameratransform.position = GetBaseCameraPosition(basePos) + shakeOffset;
    }

    public void ShakeCamera(float duration = 0.3f, float strength = 0.5f, int vibrato = 20, float randomness = 90)
    {
        if (shakeTween != null && shakeTween.IsActive())
        {
            shakeTween.Kill();
        }
        Vector3 basePos = Cameratransform.position - shakeOffset;
        shakeTween = DOTween.To(
            () => shakeOffset,
            x => shakeOffset = x,
            Vector3.zero,
            duration
        ).SetId("CameraShake");

        Cameratransform.DOShakePosition(duration, strength, vibrato, randomness)
            .SetId("CameraShake")
            .OnUpdate(() => {
                shakeOffset = Cameratransform.position - basePos;
            })
            .OnComplete(() => {
                shakeOffset = Vector3.zero;
            });
    }
}
