using UnityEngine;
using UnityEngine.SceneManagement; // SceneManagerを使用するために必要な名前空間を追加

public class Startgame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // GamePlayeシーンを読み込むメソッド
    public void LoadGamePlayeScene()
    {
        SceneManager.LoadScene("GamePlay");
    }
}
