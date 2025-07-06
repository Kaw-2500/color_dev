using UnityEngine;

public class TitleSettingCloseOpen : MonoBehaviour
{
    [SerializeField] private GameObject titleSettingPanel; //�ݒ��ʂ̃p�l����Inspector����ݒ肷�邽�߂̕ϐ�
    [SerializeField] private GameObject GameStartButton; //�Q�[���X�^�[�g�{�^���̃p�l����ݒ��ʂ��J�����ۂɔ�\���ɂ��邽�߂̕ϐ�

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CloseSettingPanel()
    {
        titleSettingPanel.SetActive(false); //�ݒ��ʂ̃p�l�����\���ɂ���
        GameStartButton.SetActive(true); //�Q�[���X�^�[�g�{�^���̃p�l����\��

    }

    public void OpenSettingPanel()
    {
        titleSettingPanel.SetActive(true); //�ݒ��ʂ̃p�l����\������
        GameStartButton.SetActive(false);
    }
}
