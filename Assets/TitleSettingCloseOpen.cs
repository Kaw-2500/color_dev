using UnityEngine;

public class TitleSettingCloseOpen : MonoBehaviour
{
    [SerializeField] private GameObject titleSettingPanel; //設定画面のパネルをInspectorから設定するための変数
    [SerializeField] private GameObject GameStartButton; //ゲームスタートボタンのパネルを設定画面を開いた際に非表示にするための変数

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
        titleSettingPanel.SetActive(false); //設定画面のパネルを非表示にする
        GameStartButton.SetActive(true); //ゲームスタートボタンのパネルを表示

    }

    public void OpenSettingPanel()
    {
        titleSettingPanel.SetActive(true); //設定画面のパネルを表示する
        GameStartButton.SetActive(false);
    }
}
