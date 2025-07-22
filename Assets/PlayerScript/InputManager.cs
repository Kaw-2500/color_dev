using UnityEngine;

public class InputManager : MonoBehaviour
//ゲッターを用いて、入力を管理するクラスです。
//動作、条件分岐などは一切行いません。それはPlayerInputHandler.csにて行います。
{
    public float Horizontal { get; private set; }
    public bool JumpPress { get; private set; }

    public bool ChangeToRed { get; private set; }
    public bool ChangeToBlue { get; private set; }
    public bool ChangeToGreen { get; private set; }

    public bool NormalAttack { get; private set; }

    void Update()
    {
        Horizontal = (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) ? 1f :
                     (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) ? -1f : 0f;

        JumpPress = (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || 
                     Input.GetKeyDown(KeyCode.Space));

        ChangeToRed = Input.GetKeyDown(KeyCode.Alpha1);
        ChangeToBlue = Input.GetKeyDown(KeyCode.Alpha2);
        ChangeToGreen = Input.GetKeyDown(KeyCode.Alpha3);

        NormalAttack = Input.GetMouseButtonDown(0);
    }
}

