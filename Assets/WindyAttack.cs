// WindyAttack.cs
using UnityEngine;

public class WindyAttack : MonoBehaviour, IAttackComponent
{
    private Rigidbody2D rb2d;
    public float Damage { get; private set; }

    private Enemy enemy;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void Init(float direction, float force, float damage, Enemy e)
    {
        enemy = e;
        Damage = damage;
        rb2d.AddForce(Vector2.right * direction * force, ForceMode2D.Impulse);
    }

    public float GetDamage()
    {
        return Damage;
    }
}
