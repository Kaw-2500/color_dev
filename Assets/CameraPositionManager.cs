using UnityEngine;

public class CameraPositionManager : MonoBehaviour
{
    public Vector3 FirstPosition;
    public Vector3 SecondPosition;
    public Vector3 ThirdPosition;

    private Transform mc_transform;

    [SerializeField] private Camera _camera;
    [SerializeField] private float SecondWeight = 0.5f;
    [SerializeField] private float ThirdWeight = 0.25f;

    private bool needsUpdate = false;

    void Start()
    {
        if (_camera == null)
        {
            Debug.LogWarning("�J�����R���|�[�l���g���A�^�b�`����Ă��܂���");
            return;
        }

        mc_transform = _camera.transform;
        if (mc_transform == null)
        {
            Debug.LogError("Camera��Transform��������܂���");
        }
    }

    void Update()
    {
        if (needsUpdate)
        {
            MergePosition();
            needsUpdate = false;
        }
    }

    void MergePosition()
    {
        float weight1 = 1.0f;
        float weight2 = (SecondPosition != Vector3.zero) ? SecondWeight : 0f;
        float weight3 = (ThirdPosition != Vector3.zero) ? ThirdWeight : 0f;

        float totalWeight = weight1 + weight2 + weight3;
        if (totalWeight <= 0f) return;

        Vector3 merged = (
            FirstPosition * weight1 +
            SecondPosition * weight2 +
            ThirdPosition * weight3
        ) / totalWeight;

        merged.z = mc_transform.position.z;
        mc_transform.position = merged;
    }

    public void MixSelectPositionNumber(Vector3 hopePosition, int number)
    {
        switch (number)
        {
            case 1:
                if (FirstPosition == Vector3.zero) SetPosition(1, hopePosition);
                else if (SecondPosition == Vector3.zero) SetPosition(2, hopePosition);
                else if (ThirdPosition == Vector3.zero) SetPosition(3, hopePosition);
                else Debug.Log("���ʂ܂ŒT���܂������A�}�[�W�����N1��cameraposition�͎g���܂���");
                break;

            case 2:
                if (SecondPosition == Vector3.zero) SetPosition(2, hopePosition);
                else if (ThirdPosition == Vector3.zero) SetPosition(3, hopePosition);
                else Debug.Log("���ʂ܂ŒT���܂������A�}�[�W�����N2��cameraposition�͎g���܂���");
                break;

            case 3:
                if (ThirdPosition == Vector3.zero) SetPosition(3, hopePosition);
                else Debug.Log("���ʂ܂ŒT���܂������A�}�[�W�����N3��cameraposition�͎g���܂���");
                break;

            case 100:
                Debug.Log("�S�Ă̗D�揇�ʂ𖳎����āA������̍��W�Ɉړ����܂�");
                DirectConnectCameraPos(hopePosition);
                return;
        }

        needsUpdate = true;
    }

    void SetPosition(int setNumber, Vector3 hopePosition)
    {
        switch (setNumber)
        {
            case 1: FirstPosition = hopePosition; break;
            case 2: SecondPosition = hopePosition; break;
            case 3: ThirdPosition = hopePosition; break;
        }
    }

    void DirectConnectCameraPos(Vector3 decidePosition)
    {
        mc_transform.position = decidePosition;
    }
}
