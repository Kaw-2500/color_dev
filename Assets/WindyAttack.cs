using UnityEngine;

public class WindyAttack : MonoBehaviour, IAttackComponent
{
    private Rigidbody2D rb2d;
    public float Damage = 10f; // Default damage value
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void Init(Vector2 direction, float force, float yOffset,float damage)
    {
        Damage = damage; 

        rb2d.position = new Vector2(rb2d.position.x, rb2d.position.y + yOffset);

        rb2d.AddForce(direction.normalized * force, ForceMode2D.Impulse);
    }

    public float GetDamage()
    {
        return Damage;
    }
}
