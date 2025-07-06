using UnityEngine;
using UnityEngine.UI;

public class SoundBar : MonoBehaviour
{
    Slider SoundSlider;
    public float NowSound = 50f;

    void Start()
    {
        SoundSlider = GetComponent<Slider>();

        float MaxSound = 100f;
        SoundSlider.maxValue = MaxSound;

        float savedVolume = PlayerPrefs.GetFloat("SoundVolume", 0.5f); 
        NowSound = savedVolume * 100f;
        SoundSlider.value = NowSound;
    }

    public void SoundChangeMethod()
    {
        NowSound = SoundSlider.value;
        PlayerPrefs.SetFloat("SoundVolume", NowSound / 100f * 1.3f); //1.3ÇÕâπÇ™è¨Ç≥Ç©Ç¡ÇΩÇÃÇ≈è≠ÇµëÂÇ´ÇﬂÇ…
        Debug.Log("åªç›ílÅF" + SoundSlider.value);
    }
}
