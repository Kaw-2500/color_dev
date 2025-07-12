using UnityEngine;
using UnityEngine.UI;

public class ChargeBar : MonoBehaviour
{
    [SerializeField] private Slider chargeSlider;

    [SerializeField] private Playeroperate playerOperate; // Playeroperate参照（リファクタ前の保険）
    [SerializeField] private PlayerBase playerBase;       // PlayerBase参照（リファクタ後対応）

    [SerializeField] private Image backgroundImage;


    private float chargeCoolTime = 0f;
    private float timer = 0f;
    private bool isCooling = false;

    void Start()
    {
        //if (playerOperate == null)
        //{
        //    playerOperate = GameObject.Find("Player")?.GetComponent<Playeroperate>();
        //}

        if (playerBase == null)
        {
            playerBase = GameObject.Find("Player")?.GetComponent<PlayerBase>();
        }

        if (chargeSlider != null)
        {
            chargeSlider.value = 0f; // 最初は空
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
        {
            chargeSlider.value = value;
        }

        if (value <= 0f)
        {
            isCooling = false;

            //if (playerOperate != null)
            //{
            //    playerOperate.IsColorChangeCoolTime = false;
            //    playerOperate.IsFinishColorChangeCoolTime = true;
            //}

            if (playerBase != null)
            {
                playerBase.IsColorChangeCool = false;
             
            }
        }
    }

    public void ChangeCoolTime(PlayerColorManager.PlayerColorState color,float chargetime)
    {
        switch (color)
        {
            case PlayerColorManager.PlayerColorState.Red:
                SetColorAndCooltime(new Color(1f, 0.5f, 0.5f), chargetime);
                break;
            case PlayerColorManager.PlayerColorState.Blue:
                SetColorAndCooltime(new Color(0.5f, 0.5f, 1f), chargetime);
                break;
            case PlayerColorManager.PlayerColorState.Green:
                SetColorAndCooltime(new Color(0.5f, 1f, 0.5f), chargetime);
                break;
            default:
                Debug.LogWarning("ChargeBar: 不正な色が指定されました: " + color);
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

        //if (playerOperate != null)
        //{
        //    playerOperate.IsFinishColorChangeCoolTime = false;
        //    playerOperate.IsColorChangeCoolTime = true;
        //}

        if (playerBase != null)
        {
            playerBase.IsColorChangeCool = true;
        }
    }
}
