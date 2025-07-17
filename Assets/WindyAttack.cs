using UnityEngine;

public class WindyAttack : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Vector2 force;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb2d.AddForce(force, ForceMode2D.Impulse);
    }

    public void SetForce(Vector2 f)
    {
        force = f;
    }
}
