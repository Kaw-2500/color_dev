using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager���g�p���邽�߂ɕK�v�Ȗ��O��Ԃ�ǉ�

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

    // GamePlaye�V�[����ǂݍ��ރ��\�b�h
    public void LoadGamePlayeScene()
    {
        SceneManager.LoadScene("GamePlay");
    }
}
