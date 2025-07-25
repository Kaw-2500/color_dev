using UnityEngine;
using System.Collections;

public class Camerapos : MonoBehaviour
{
    public Transform playerTransform;
    [SerializeField] float camerahight = 5f;
    [SerializeField] float cameradistance = 5f;
    [SerializeField] float cameraSmoothTime = 0.1f;
    [SerializeField] float parryDurationSeconds = 0.6f;
    [SerializeField] float zoomHeight = 1.5f;        // �Y�[�����̍����␳

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
            t = 1f - Mathf.Pow(1f - t, 2f); // �C�[�Y�A�E�g

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
        // �R���[�`���J�n���̃J�����̏�����Ԃ�ۑ�
        Vector3 originalPos = Cameratransform.position;
        float originalOrtho = mainCamera.orthographicSize;

        float elapsed = 0f;

        // �Y�[���C������
        while (elapsed < parryTime * 0.2)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / parryZoomSpeed );
            t = 1f - Mathf.Pow(1f - t, 2f); // �C�[�Y�A�E�g

            // �J���������B���ׂ��ڕW�ʒu
            Vector3 targetCameraCenterPos = new Vector3(
                playerTransform.position.x,
                playerTransform.position.y + zoomHeight,
                originalPos.z
            );

            //Debug.Log(targetCameraCenterPos);
            //Debug.Log(playerTransform.position);

            // �J�����̈ʒu��originalPos����targetCameraCenterPos�֕��
            Cameratransform.position = Vector3.Lerp(originalPos, targetCameraCenterPos, t);

            // �I�[�\�O���t�B�b�N�T�C�Y��originalOrtho����parryZoomSize�֕��
            mainCamera.orthographicSize = Mathf.Lerp(originalOrtho, parryZoomSize, t);

            yield return null;
        }

        // �Y�[���C�������S�Ɋ����������_�ł̃J�����̏�Ԃ��m�肳����
        Cameratransform.position = new Vector3(
            playerTransform.position.x,
            playerTransform.position.y + zoomHeight,
            originalPos.z
        );
        mainCamera.orthographicSize = parryZoomSize;


        // �p���B���Ԃ��A�Y�[���A�E�g�̎��ԂŖ��܂�܂őҋ@
        float parryElapsed = 0f;
        while (parryElapsed < parryTime * 0.6)
        {
            parryElapsed += Time.unscaledDeltaTime;

            // �Y�[���C�����ꂽ��ԂŃv���C���[�ɒǏ]
            Vector3 followPos = new Vector3(
                playerTransform.position.x,
                playerTransform.position.y + zoomHeight,
                originalPos.z
            );
            Cameratransform.position = followPos;
            yield return null;
        }

        // �Y�[���A�E�g����
        elapsed = 0f;
        // �Y�[���A�E�g�J�n���̃J�����̌��݂̈ʒu�ƃI�[�\�O���t�B�b�N�T�C�Y��ۑ�
        Vector3 currentPosAtZoomOutStart = Cameratransform.position;
        float currentOrthoAtZoomOutStart = mainCamera.orthographicSize;

        while (elapsed < parryZoomSpeed * 0.2)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / parryZoomSpeed);
            t = 1f - Mathf.Pow(1f - t, 2f); // �C�[�Y�A�E�g

            // currentPosAtZoomOutStart����originalPos�֕��
            Cameratransform.position = Vector3.Lerp(currentPosAtZoomOutStart, originalPos, t);
            // currentOrthoAtZoomOutStart����originalOrtho�֕��
            mainCamera.orthographicSize = Mathf.Lerp(currentOrthoAtZoomOutStart, originalOrtho, t);

            yield return null;
        }

        // �ŏI�I�Ɍ��̈ʒu�ƃT�C�Y�Ɋm���ɐݒ肵����
        Cameratransform.position = originalPos;
        mainCamera.orthographicSize = originalOrtho;

        // �p���B�Y�[�����I��
        isParryZooming = false;
        parryZoomCoroutine = null;
    }
}
