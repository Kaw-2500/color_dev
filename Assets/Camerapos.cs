using UnityEngine;
using System.Collections;
public class Camerapos : MonoBehaviour
//Statemanagerから貰ったGroundYposと、シリアライズしたPlayerの座標と、カメラのtransformを使って、
//Y座標と、x座標をそれぞれ計算で決めて、カメラの位置を決定するスクリプトです。
{
    public Transform playerTransform;
    [SerializeField] float camerahight = 5f;
    [SerializeField] float cameradistance = 5f;
    [SerializeField] float cameraSmoothTime = 0.1f;

    bool dead;

    private float smoothY;
    private float velocityY;

    [SerializeField] private PlayerStateManager playerStateManager;
    public Camera mainCamera;


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
        if (!dead) return;

        transform.position = new Vector3(
            playerTransform.position.x,
            playerTransform.position.y,
            transform.position.z
        );
    }

    void LateUpdate()
    {
        if (playerStateManager.IsDead)
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
            transform.position.z
        );

        transform.position = newPos;
    }

    public IEnumerator PanAndZoomCoroutine(float targetOrthoSize, float duration)
    {
        Vector3 startPos = transform.position;// 現在のカメラ位置
        float startOrtho = mainCamera.orthographicSize;// 現在のカメラのズームの強さ（映してる範囲）

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(elapsed / duration);
            t = 1f - Mathf.Pow(1f - t, 2f); //イーズアウトで最初速く

            Vector3 targetPos = new Vector3(//プレイヤーが死亡後に移動する場合に備えて、常に追従
                playerTransform.position.x,
                playerTransform.position.y,
                transform.position.z
            );

            transform.position = Vector3.Lerp(startPos, targetPos, t);
            mainCamera.orthographicSize = Mathf.Lerp(startOrtho, targetOrthoSize, t);

            yield return null;
        }

        // 最終的にカメラはプレイヤーの現在位置に合わせる
        Vector3 finalPos = new Vector3(
            playerTransform.position.x,
            playerTransform.position.y,
            transform.position.z
        );
        transform.position = finalPos;
        mainCamera.orthographicSize = targetOrthoSize;
        dead = true; // 死亡時のカメラズームが完了したことを示すフラグを設定
    }
}