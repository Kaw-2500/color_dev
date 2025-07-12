using System.Collections;
using UnityEditor;
using UnityEngine;

public class PlayerHitDamage : MonoBehaviour
{
    [SerializeField] PlayerStateManager playerStateManager;
    [SerializeField] private PlayerColorManager playerColorManager;
    private GameObject player;

    Rigidbody2D rb2d;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = this.gameObject;

        rb2d = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnHitDamage(float damage)
    {
        playerStateManager.ApplyDamage(damage);
    }

    public void ApplyWindKnockback(Vector2 knockback)
    {
        if (playerStateManager.IsBlown) return;

        float levity;

        levity = playerColorManager.GetCurrentData().levity;

        knockback.x *= levity; // •—‚Ì‚«”ò‚Î‚µ—Í‚ÉŒy‚³‚ğ”½‰f
        knockback.y *= levity; // •—‚Ì‚«”ò‚Î‚µ—Í‚ÉŒy‚³‚ğ”½‰f

        // ‚«”ò‚Î‚µ‚Ì•¨—“I‚È—Í‚ğ‰Á‚¦‚é
        rb2d.AddForce(knockback, ForceMode2D.Impulse);

        playerStateManager.SetBlown(true);
        StartCoroutine(ResetBlown(0.5f));
    }

    private IEnumerator ResetBlown(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerStateManager.SetBlown(false);
    }

}