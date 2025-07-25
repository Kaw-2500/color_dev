using UnityEngine;
using System.Collections;

public class Camerapos : MonoBehaviour
{
    public Transform playerTransform;
    [SerializeField] float camerahight = 5f;
    [SerializeField] float cameradistance = 5f;
    [SerializeField] float cameraSmoothTime = 0.1f;
    [SerializeField] float parryDurationSeconds = 0.6f;
    [SerializeField] float zoomHeight = 1.5f;        // ズーム時の高さ補正

    public Transform Cameratransform;

    private bool isParryZooming = false;
    bool Finisheddeadaction;

    private float smoothY;
    private float velocityY;

    [SerializeField] private PlayerStateManager playerStateManager;
    public Camera mainCamera;

    [SerializeField] private float parryZoomSize = 3f;
    [SerializeField] private float parryZoomSpeed = 0.2f;

    private Coroutine parryZoomCoroutine;

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
        Cameratransform.position = new Vector3(
            playerTransform.position.x,
            playerTransform.position.y,
            Cameratransform.position.z
        );
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

        Cameratransform.position = newPos;
    }

    public IEnumerator PanAndZoomCoroutine(float targetOrthoSize, float duration)
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

            Cameratransform.position = Vector3.Lerp(startPos, targetPos, t);
            mainCamera.orthographicSize = Mathf.Lerp(startOrtho, targetOrthoSize, t);

            yield return null;
        }

        Vector3 finalPos = new Vector3(
            playerTransform.position.x,
            playerTransform.position.y,
            Cameratransform.position.z
        );
        Cameratransform.position = finalPos;
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
        // コルーチン開始時のカメラの初期状態を保存
        Vector3 originalPos = Cameratransform.position;
        float originalOrtho = mainCamera.orthographicSize;

        float elapsed = 0f;

        // ズームイン処理
        while (elapsed < parryTime * 0.2)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / parryZoomSpeed );
            t = 1f - Mathf.Pow(1f - t, 2f); // イーズアウト

            // カメラが到達すべき目標位置
            Vector3 targetCameraCenterPos = new Vector3(
                playerTransform.position.x,
                playerTransform.position.y + zoomHeight,
                originalPos.z
            );

            //Debug.Log(targetCameraCenterPos);
            //Debug.Log(playerTransform.position);

            // カメラの位置をoriginalPosからtargetCameraCenterPosへ補間
            Cameratransform.position = Vector3.Lerp(originalPos, targetCameraCenterPos, t);

            // オーソグラフィックサイズをoriginalOrthoからparryZoomSizeへ補間
            mainCamera.orthographicSize = Mathf.Lerp(originalOrtho, parryZoomSize, t);

            yield return null;
        }

        // ズームインが完全に完了した時点でのカメラの状態を確定させる
        Cameratransform.position = new Vector3(
            playerTransform.position.x,
            playerTransform.position.y + zoomHeight,
            originalPos.z
        );
        mainCamera.orthographicSize = parryZoomSize;


        // パリィ時間が、ズームアウトの時間で埋まるまで待機
        float parryElapsed = 0f;
        while (parryElapsed < parryTime * 0.6)
        {
            parryElapsed += Time.unscaledDeltaTime;

            // ズームインされた状態でプレイヤーに追従
            Vector3 followPos = new Vector3(
                playerTransform.position.x,
                playerTransform.position.y + zoomHeight,
                originalPos.z
            );
            Cameratransform.position = followPos;
            yield return null;
        }

        // ズームアウト処理
        elapsed = 0f;
        // ズームアウト開始時のカメラの現在の位置とオーソグラフィックサイズを保存
        Vector3 currentPosAtZoomOutStart = Cameratransform.position;
        float currentOrthoAtZoomOutStart = mainCamera.orthographicSize;

        while (elapsed < parryZoomSpeed * 0.2)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / parryZoomSpeed);
            t = 1f - Mathf.Pow(1f - t, 2f); // イーズアウト

            // currentPosAtZoomOutStartからoriginalPosへ補間
            Cameratransform.position = Vector3.Lerp(currentPosAtZoomOutStart, originalPos, t);
            // currentOrthoAtZoomOutStartからoriginalOrthoへ補間
            mainCamera.orthographicSize = Mathf.Lerp(currentOrthoAtZoomOutStart, originalOrtho, t);

            yield return null;
        }

        // 最終的に元の位置とサイズに確実に設定し直す
        Cameratransform.position = originalPos;
        mainCamera.orthographicSize = originalOrtho;

        // パリィズームが終了
        isParryZooming = false;
        parryZoomCoroutine = null;
    }
}
