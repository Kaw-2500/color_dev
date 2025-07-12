using UnityEngine;

public class Camerapos : MonoBehaviour
//Statemanager��������GroundYpos�ƁA�V���A���C�Y����Player�̍��W�ƁA�J������transform���g���āA
//Y���W�ƁAx���W�����ꂼ��v�Z�Ō��߂āA�J�����̈ʒu�����肷��X�N���v�g�ł��B
{
    public Transform playerTransform;
    [SerializeField] float camerahight = 5f;
    [SerializeField] float cameradistance = 5f;
    [SerializeField] float cameraSmoothTime = 0.1f;

    private float smoothY;
    private float velocityY;

    [SerializeField] private PlayerStateManager playerStateManager;  

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

        smoothY = transform.position.y;
    }

    void LateUpdate()
    {
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
}