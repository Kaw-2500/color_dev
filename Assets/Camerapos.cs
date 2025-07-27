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

        float elapsed = 0f;

        // ズームイン処理
        while (elapsed < parryTime * 0.2f)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / parryZoomSpeed );
            t = 1f - Mathf.Pow(1f - t, 2f); // イーズアウト

            Vector3 targetCameraCenterPos = new Vector3(
                playerTransform.position.x,
                playerTransform.position.y + zoomHeight,
                originalPos.z
            );

            ApplyCameraPosition(Vector3.Lerp(originalPos, targetCameraCenterPos, t));
            mainCamera.orthographicSize = Mathf.Lerp(originalOrtho, parryZoomSize, t);

            yield return null;
        }

        ApplyCameraPosition(new Vector3(
            playerTransform.position.x,
            playerTransform.position.y + zoomHeight,
            originalPos.z
        ));
        mainCamera.orthographicSize = parryZoomSize;

        float parryElapsed = 0f;
        while (parryElapsed < parryTime * 0.6f)
        {
            parryElapsed += Time.unscaledDeltaTime;
            Vector3 followPos = new Vector3(
                playerTransform.position.x,
                playerTransform.position.y + zoomHeight,
                originalPos.z
            );
            ApplyCameraPosition(followPos);
            yield return null;
        }

        elapsed = 0f;
        Vector3 currentPosAtZoomOutStart = Cameratransform.position;
        float currentOrthoAtZoomOutStart = mainCamera.orthographicSize;

        while (elapsed < parryZoomSpeed * 0.2f)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / parryZoomSpeed);
            t = 1f - Mathf.Pow(1f - t, 2f); // イーズアウト

            ApplyCameraPosition(Vector3.Lerp(currentPosAtZoomOutStart, originalPos, t));
            mainCamera.orthographicSize = Mathf.Lerp(currentOrthoAtZoomOutStart, originalOrtho, t);

            yield return null;
        }

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
