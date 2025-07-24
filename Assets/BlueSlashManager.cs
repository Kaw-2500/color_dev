using UnityEngine;

public class BlueSlashManager : MonoBehaviour, IAttackComponent
{
    [SerializeField] private float damageAmount = 10f;

    private bool isAttacked = false;
    private bool isCharged = true;  // �`���[�W��Ԃ��Ǘ�����t���O
    private bool playerInTrigger = false;
    private Collider2D playerCollider = null;

    private Rigidbody2D rb2d;

    bool InPlayerAttackTrigger = false;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        isCharged = true;
        isAttacked = false;
    }

    public void Init(float direction, float force, float damage)
    {
        isAttacked = false;
        isCharged = true;  // ���������Ƀ`���[�W��Ԃ����Z�b�g

        damageAmount = damage;
        rb2d.AddForce(Vector2.right * direction * force, ForceMode2D.Impulse);
    }

    // AnimationEvent����Ă΂��z��
    void Fncharge()
    {
        isCharged = false;
    }

    // AnimationEvent����Ă΂��z��
    void OnAnimationEnd()
    {
        Destroy(gameObject);
    }

    void StartParry()
    {
        if(!isCharged) return;//�U�����n�߂Ȃ��ƃp���B�͂����Ȃ�


    }



    private void Update()
    {
        if (playerInTrigger && !isAttacked && !isCharged && playerCollider != null)
        {
            TryApplyDamage(playerCollider);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var attackStrategy = collision.GetComponent<IAttackStrategy>();
        if (attackStrategy != null)
        {
            InPlayerAttackTrigger = true;
            Debug.Log("�p���B���邺�I");
        }


        if (!collision.CompareTag("Player")) return;
        playerInTrigger = true;
        playerCollider = collision;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // IAttackStrategy�����������R���|�[�l���g�������Ă��Ȃ���Ζ���
        var attackStrategy = collision.GetComponent<IAttackStrategy>();
        if (attackStrategy != null)
        {
            InPlayerAttackTrigger = false;Debug.Log("�p���B�I���I");
        }

        if (!collision.CompareTag("Player")) return;
        playerInTrigger = false;
        playerCollider = null;
    }

    private void TryApplyDamage(Collider2D collision)
    {
        if (isAttacked) return;

        var hitDamage = collision.GetComponent<PlayerHitDamage>();
        if (hitDamage != null)
        {
            hitDamage.OnHitDamage(damageAmount);
            isAttacked = true;
        }
    }
}
