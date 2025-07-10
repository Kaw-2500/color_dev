using UnityEngine;
using UnityEngine.UI;

public class Hpbar : MonoBehaviour
{
    [SerializeField] private Playeroperate playerOperate;  // Playeroperate�R���|�[�l���g�̎Q��
    [SerializeField] private Slider hpSlider;              // HP�o�[��Slider

    private float maxHp;

    void Start()
    {
        if (playerOperate == null)
        {
            Debug.LogError("Hpbar: playerOperate ���A�T�C������Ă��܂���B");
            enabled = false;
            return;
        }

        if (hpSlider == null)
        {
            Debug.LogError("Hpbar: hpSlider ���A�T�C������Ă��܂���B");
            enabled = false;
            return;
        }

        // �����ő�HP���擾�i����Playeroperate��maxHp���������100f�ȂǌŒ�l�ł��j
        maxHp = playerOperate.PlayerHp;
        hpSlider.maxValue = maxHp;
        hpSlider.value = maxHp;
    }

    void Update()
    {
        // ���t���[��PlayerHp��Slider�ɔ��f
        if (playerOperate.PlayerHp < 0) playerOperate.PlayerHp = 0; // ����HP�h�~

        hpSlider.value = playerOperate.PlayerHp;
    }
}
