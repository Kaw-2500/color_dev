using UnityEngine;

public class Camerapos : MonoBehaviour
{
    public Transform playerTransform;
    [SerializeField] float camerahight = 5f;     // Y軸の高さオフセット
    [SerializeField] float cameradistance = 5f;  // プレイヤーのX座標からの横距離オフセット
    [SerializeField] float cameraSmoothTime = 0.1f; // Y追従のスムーズ時間

    private float smoothY;
    private float velocityY;

    [SerializeField] private PlayerBase playerBase;

    void Start()
    {
        if (playerTransform == null)
        {
            Debug.LogError("PlayerTransform is not assigned.");
            return;
        }
        if (playerBase == null)
        {
            Debug.LogError("PlayerBase component is not assigned.");
            return;
        }

        // 初期化
        smoothY = transform.position.y;
    }

    void LateUpdate()
    {
        FollowPlayerYOnly();
    }

    void FollowPlayerYOnly()
    {
        float targetGroundY = playerBase.GroundYpos;
        if (float.IsNaN(targetGroundY))
        {
            targetGroundY = playerTransform.position.y; // フォールバック
        }

        // Y座標のみスムーズに追従
        smoothY = Mathf.SmoothDamp(smoothY, targetGroundY + camerahight, ref velocityY, cameraSmoothTime);

        Vector3 newPos = new Vector3(
            playerTransform.position.x + cameradistance, // X座標はプレイヤー位置＋距離
            smoothY,
            transform.position.z // Z座標はカメラのまま
        );

        transform.position = newPos;
    }
}
