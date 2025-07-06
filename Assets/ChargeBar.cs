using UnityEngine;
using UnityEngine.UI;

public class ChargeBar : MonoBehaviour
{
    [SerializeField] private Slider chargeSlider;
    [SerializeField] private Player player;
    [SerializeField] private Image backgroundImage;

    [SerializeField] private float RedCooltime = 5f;
    [SerializeField] private float BlueCooltime = 3f;
    [SerializeField] private float GreenCooltime = 2f;

    private float chargeCoolTime = 0f;
    private float timer = 0f;
    private bool isCooling = false;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Player")?.GetComponent<Player>();
        }

        if (chargeSlider != null)
        {
            chargeSlider.value = 0f; // �ŏ��̓X���C�_�[��
        }
    }

    void Update()
    {
        if (!isCooling) return;

        if (chargeCoolTime <= 0f)
        {
            Debug.LogWarning("�N�[���^�C����0");
            return;
        }

        timer += Time.deltaTime;
        float value = Mathf.Clamp(1f - (timer / chargeCoolTime), 0f, 1f);//��Βl��p����0�ȉ��ɂȂ邱�Ƃ�h��
        if (chargeSlider != null)
        {
            chargeSlider.value = value;
        }

        if (value <= 0f)
        {
            player.IsColorChangeCoolTime = false; // �N�[���^�C���I�����Ƀv���C���[�̃t���O���X�V
            isCooling = false;
            if (player != null)
            {
                player.IsFinishColorChangeCoolTime = true;
            }
        }
    }

    public void ChangeCoolTime(string changeColorName)
    {
        switch (changeColorName)
        {
            case "Green":
                SetColorAndCooltime(new Color(0.5f, 1f, 0.5f), GreenCooltime);
                break;
            case "Blue":
                SetColorAndCooltime(new Color(0.5f, 0.5f, 1f), BlueCooltime);
                break;
            case "Red":
                SetColorAndCooltime(new Color(1f, 0.5f, 0.5f), RedCooltime);
                break;
            default:
                Debug.LogWarning("�O���F�ȊO�̐F���o�[cs�ɓ����Ă�: " + changeColorName);
                break;
        }
    }

    private void SetColorAndCooltime(Color color, float cooltime)
    {
        if (backgroundImage != null)
        {
            backgroundImage.color = color;
        }
        chargeCoolTime = cooltime;
        timer = 0f;
        isCooling = true;
        if (chargeSlider != null)
        {
            chargeSlider.value = 1f;
        }
        if (player != null)
        {
            player.IsFinishColorChangeCoolTime = false; // �N�[���^�C���J�n����false�ɖ߂�
        }
    }
}