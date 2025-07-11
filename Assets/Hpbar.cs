using UnityEngine;
using UnityEngine.UI;

public class Hpbar : MonoBehaviour
{
    [SerializeField] private Playeroperate playerOperate;  // Playeroperateコンポーネントの参照
    [SerializeField] private Slider hpSlider;              // HPバーのSlider

    private float maxHp;

    void Start()
    {
        if (playerOperate == null)
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
        maxHp = playerOperate.PlayerHp;
        hpSlider.maxValue = maxHp;
        hpSlider.value = maxHp;
    }

    void Update()
    {
        // 毎フレームPlayerHpをSliderに反映
        if (playerOperate.PlayerHp < 0) playerOperate.PlayerHp = 0; // 負のHP防止

        hpSlider.value = playerOperate.PlayerHp;
    }
}
