using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float Horizontal { get; private set; }
    public bool JumpPress { get; private set; }

    public bool ChangeToRed { get; private set; }
    public bool ChangeToBlue { get; private set; }
    public bool ChangeToGreen { get; private set; }

    void Update()
    {
        Horizontal = (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) ? 1f :
                     (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) ? -1f : 0f;

        JumpPress = (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow));

        ChangeToRed = Input.GetKeyDown(KeyCode.Alpha1);
        ChangeToBlue = Input.GetKeyDown(KeyCode.Alpha2);
        ChangeToGreen = Input.GetKeyDown(KeyCode.Alpha3);

    }
}

