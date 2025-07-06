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
        SitukoiText.text = string.Format("�������J�E���^�[: {0}", SitukoiCounter);
        SitukoiCounter = 0; // ������
    }

    private void Update()
    {
        // �J�E���^�[�̒l���e�L�X�g�ɍX�V
        SitukoiText.text = string.Format("�������J�E���^�[: {0}", SitukoiCounter);
    }


    public void PlaySound()
    {
        if (soundClip != null && audioSource != null)
        {
            float volume = PlayerPrefs.GetFloat("SoundVolume", 1.0f); 
            audioSource.volume = volume;
            if (SitukoiCounter < 4f) // SitukoiCounter��1�����̂Ƃ��̂ݍĐ�
            {
                audioSource.Stop(); // ���ɍĐ����̉����~
                audioSource.PlayOneShot(soundClip);
                SitukoiCounter++;
            }
            else if (SitukoiCounter >= 4f && SitukoiSource != null) // 5��ȏ�Đ����ꂽ�ꍇ��SitukoiSource���Đ�
            {
                audioSource.Stop(); // ���ɍĐ����̉����~
                audioSource.PlayOneShot(SitukoiSource);
                SitukoiCounter = 0; // �J�E���^�[�����Z�b�g
            }
            
        }
    }
}
