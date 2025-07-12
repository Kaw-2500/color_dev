using UnityEngine;

public class Camerapos : MonoBehaviour
{
    public Transform playerTransform;
    [SerializeField] float camerahight = 5f;     // Y���̍����I�t�Z�b�g
    [SerializeField] float cameradistance = 5f;  // �v���C���[��X���W����̉������I�t�Z�b�g
    [SerializeField] float cameraSmoothTime = 0.1f; // Y�Ǐ]�̃X���[�Y����

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

        // ������
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
            targetGroundY = playerTransform.position.y; // �t�H�[���o�b�N
        }

        // Y���W�̂݃X���[�Y�ɒǏ]
        smoothY = Mathf.SmoothDamp(smoothY, targetGroundY + camerahight, ref velocityY, cameraSmoothTime);

        Vector3 newPos = new Vector3(
            playerTransform.position.x + cameradistance, // X���W�̓v���C���[�ʒu�{����
            smoothY,
            transform.position.z // Z���W�̓J�����̂܂�
        );

        transform.position = newPos;
    }
}
