using UnityEngine;

public class WindyAttack : MonoBehaviour, IAttackComponent
{
    private Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void Init(Vector2 direction, float force, float yOffset)
    {
        rb2d.position = new Vector2(rb2d.position.x, rb2d.position.y + yOffset);

        rb2d.AddForce(direction.normalized * force, ForceMode2D.Impulse);
    }


}
