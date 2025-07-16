using UnityEngine;
using System.Collections;
public class Camerapos : MonoBehaviour
//Statemanager��������GroundYpos�ƁA�V���A���C�Y����Player�̍��W�ƁA�J������transform���g���āA
//Y���W�ƁAx���W�����ꂼ��v�Z�Ō��߂āA�J�����̈ʒu�����肷��X�N���v�g�ł��B
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
        Vector3 startPos = transform.position;// ���݂̃J�����ʒu
        float startOrtho = mainCamera.orthographicSize;// ���݂̃J�����̃Y�[���̋����i�f���Ă�͈́j

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(elapsed / duration);
            t = 1f - Mathf.Pow(1f - t, 2f); //�C�[�Y�A�E�g�ōŏ�����

            Vector3 targetPos = new Vector3(//�v���C���[�����S��Ɉړ�����ꍇ�ɔ����āA��ɒǏ]
                playerTransform.position.x,
                playerTransform.position.y,
                transform.position.z
            );

            transform.position = Vector3.Lerp(startPos, targetPos, t);
            mainCamera.orthographicSize = Mathf.Lerp(startOrtho, targetOrthoSize, t);

            yield return null;
        }

        // �ŏI�I�ɃJ�����̓v���C���[�̌��݈ʒu�ɍ��킹��
        Vector3 finalPos = new Vector3(
            playerTransform.position.x,
            playerTransform.position.y,
            transform.position.z
        );
        transform.position = finalPos;
        mainCamera.orthographicSize = targetOrthoSize;
        dead = true; // ���S���̃J�����Y�[���������������Ƃ������t���O��ݒ�
    }
}