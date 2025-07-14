using UnityEngine;
using UnityEngine.UI;

public class Hpbar : MonoBehaviour
{
    [SerializeField] private PlayerStateManager playerStateManager;  // Playeroperate�R���|�[�l���g�̎Q��
    [SerializeField] private Slider hpSlider;              // HP�o�[��Slider

    private float maxHp;

    void Start()
    {
        if (playerStateManager == null)
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
        maxHp = playerStateManager.playerHp;
        hpSlider.maxValue = maxHp;
        hpSlider.value = maxHp;
    }

    void Update()
    {
        // ���t���[��PlayerHp��Slider�ɔ��f
        if (playerStateManager.playerHp < 0) return;// ����HP�h�~
        
         hpSlider.value = playerStateManager.playerHp; //HP��Slider�ɔ��f   
    }
}
