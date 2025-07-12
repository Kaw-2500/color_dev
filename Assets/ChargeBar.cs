using UnityEngine;
using UnityEngine.UI;

public class ChargeBar : MonoBehaviour

//ColorManagaerから貰ったdataを元に、色とクールタイムを設定するUIです。
//Sliderの値で、クールタイムが終わったかを示すIscolorCgangeCoolのみをColorManagerに渡します。
//呼び出しは必ずPlayerColorManagerから行われます。
{
    [SerializeField] private Slider chargeSlider;
    //[SerializeField] private PlayerBase playerBase;
    [SerializeField] private Image backgroundImage;
    [SerializeField]PlayerColorManager  playerColorManager;

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
            playerColorManager.ResetColorChangeCool();
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
