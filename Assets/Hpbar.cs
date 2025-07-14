using UnityEngine;
using UnityEngine.UI;

public class Hpbar : MonoBehaviour
{
    [SerializeField] private PlayerStateManager playerStateManager;  // Playeroperateコンポーネントの参照
    [SerializeField] private Slider hpSlider;              // HPバーのSlider

    private float maxHp;

    void Start()
    {
        if (playerStateManager == null)
        {
            Debug.LogError("Hpbar: playerOperate がアサインされていません。");
            enabled = false;
            return;
        }

        if (hpSlider == null)
        {
            Debug.LogError("Hpbar: hpSlider がアサインされていません。");
            enabled = false;
            return;
        }

        // 初期最大HPを取得（もしPlayeroperateにmaxHpが無ければ100fなど固定値でも可）
        maxHp = playerStateManager.playerHp;
        hpSlider.maxValue = maxHp;
        hpSlider.value = maxHp;
    }

    void Update()
    {
        // 毎フレームPlayerHpをSliderに反映
        if (playerStateManager.playerHp < 0) return;// 負のHP防止
        
         hpSlider.value = playerStateManager.playerHp; //HPをSliderに反映   
    }
}
