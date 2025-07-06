using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI; 

public class CheckSound : MonoBehaviour
{
    [SerializeField]
    private AudioClip soundClip;

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip SitukoiSource;

    private float SitukoiCounter ;

    [SerializeField] private Text SitukoiText;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
        }
        SitukoiText.text = string.Format("しつこいカウンター: {0}", SitukoiCounter);
        SitukoiCounter = 0; // 初期化
    }

    private void Update()
    {
        // カウンターの値をテキストに更新
        SitukoiText.text = string.Format("しつこいカウンター: {0}", SitukoiCounter);
    }


    public void PlaySound()
    {
        if (soundClip != null && audioSource != null)
        {
            float volume = PlayerPrefs.GetFloat("SoundVolume", 1.0f); 
            audioSource.volume = volume;
            if (SitukoiCounter < 4f) // SitukoiCounterが1未満のときのみ再生
            {
                audioSource.Stop(); // 既に再生中の音を停止
                audioSource.PlayOneShot(soundClip);
                SitukoiCounter++;
            }
            else if (SitukoiCounter >= 4f && SitukoiSource != null) // 5回以上再生された場合はSitukoiSourceを再生
            {
                audioSource.Stop(); // 既に再生中の音を停止
                audioSource.PlayOneShot(SitukoiSource);
                SitukoiCounter = 0; // カウンターをリセット
            }
            
        }
    }
}
