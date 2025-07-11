using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HandleMove(float runforce, float maxspeed, float jumpforce, float gravityscale, Rigidbody2D rb2d,float input)
    {
        rb2d.gravityScale = gravityscale;

       
            rb2d.AddForce(new Vector2(input * runforce, 0), ForceMode2D.Impulse);
        
    }

    public void PlayerJump(float jumpForce, Rigidbody2D rb2d)
    {
            rb2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        
    }

}