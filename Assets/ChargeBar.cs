using UnityEngine;
using UnityEngine.UI;

public class ChargeBar : MonoBehaviour
{
    [SerializeField] private Slider chargeSlider;
    //[SerializeField] private PlayerBase playerBase;
    [SerializeField] private Image backgroundImage;

    private float chargeCoolTime = 0f;
    private float timer = 0f;
    private bool isCooling = false;

    void Start()
    {
        //if (playerBase == null)
        //{
        //    //playerBase = GameObject.Find("Player")?.GetComponent<PlayerBase>();
        //    if (playerBase == null) Debug.LogError("ChargeBar: PlayerBaseが見つかりません。");
        //}

        if (chargeSlider != null)
        {
            chargeSlider.value = 0f;
        }
    }

    void Update()
    {
        if (!isCooling) return;
        if (chargeCoolTime <= 0f)
        {
            Debug.LogWarning("ChargeBar: クールタイムが0以下です。");
            return;
        }

        timer += Time.deltaTime;
        float value = Mathf.Clamp01(1f - (timer / chargeCoolTime));

        if (chargeSlider != null)
            chargeSlider.value = value;

        if (value <= 0f)
        {
            isCooling = false;
            //if (playerBase != null)
            //{
            //    playerBase.IsColorChangeCool = false;
            //}
        }
    }

    public void ChangeCoolTime(PlayerColorDataExtended data)
    {
        SetColorAndCooltime(data.displayColor,data.chargeCoolTime);
    }

    private void SetColorAndCooltime(Color color, float cooltime)
    {
        if (backgroundImage != null)
            backgroundImage.color = color;

        chargeCoolTime = cooltime;
        timer = 0f;
        isCooling = true;

        if (chargeSlider != null)
            chargeSlider.value = 1f;

        //if (playerBase != null)
        //    playerBase.IsColorChangeCool = true;
    }
}
