using UnityEngine;
using System.Collections;

public class BlueSlashManager : MonoBehaviour, IAttackComponent
{
    [SerializeField] private float damageAmount = 10f;

    private Collider2D playerCollider = null;
    private Rigidbody2D rb2d;
    private Enemy enemy;

    private bool isAttacked = false;
    private bool isCharged = true;

    private bool playerInTrigger = false;

    private ParryState parryState = ParryState.None;

    private enum ParryState
    {
        None,         // ������ԁF�p���B�ҋ@�O
        Parryable,    // �p���B�\���Ԓ�
        Parried,      // �p���B����
        ParryFailed   // �p���B���s�i���Ԍo�߁j
    }

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        isCharged = true;
        isAttacked = false;
        parryState = ParryState.None;
    }

    public void Init(float direction, float force, float damage, Enemy enemy)
    {
        this.enemy = enemy;
        if (this.enemy != null)
        {
            this.enemy.OnDamagedByPlayer += OnEnemyDamagedByPlayer;
        }

        damageAmount = damage;
        isAttacked = false;
        isCharged = true;
        parryState = ParryState.None;

        rb2d.AddForce(Vector2.right * direction * force, ForceMode2D.Impulse);
    }

    private void OnDestroy()
    {
        if (enemy != null)
        {
            enemy.OnDamagedByPlayer -= OnEnemyDamagedByPlayer;
        }
    }

    // �A�j���[�V�����C�x���g����Ă΂��
    public void Fncharge()
    {
        isCharged = false;

        Debug.Log("�p���B���J�n");
        StartCoroutine(ParryWindowCoroutine(0.4f));
    }

    private IEnumerator ParryWindowCoroutine(float duration)
    {
        parryState = ParryState.Parryable;
        yield return new WaitForSeconds(duration);

        if (parryState == ParryState.Parryable)
        {
            Debug.Log("�p���B���ԏI�� �� ���s");
            parryState = ParryState.ParryFailed;
        }
    }

    // �A�j���[�V�����I���C�x���g����Ă΂��
    public void OnAnimationEnd()
    {
        Destroy(gameObject);
    }

    private void StartParry()
    {
        if (isCharged) return; // �`���[�W���͂܂��������Ȃ�
        if (parryState != ParryState.Parryable) return;

        Debug.Log("�p���B�������J�n�I");
        parryState = ParryState.Parried;
        // �p���B�������̃G�t�F�N�g�⏈���������ɒǉ����Ă��ǂ�
    }

    private void OnEnemyDamagedByPlayer()
    {
        if (parryState == ParryState.Parryable)
        {
            Debug.Log("�p���B�����IEnemy����U�����ꂽ���Ƀp���B������󂯎��܂����B");
            StartParry();
        }
        else
        {
            Debug.Log("�p���B���s�F�p���B�\���ԊO�ɍU������܂����B");
        }
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
        if (!collision.CompareTag("Player")) return;

        playerInTrigger = true;
        playerCollider = collision;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        playerInTrigger = false;
        playerCollider = null;
    }

    private void TryApplyDamage(Collider2D collision)
    {
        if (isAttacked) return;
        if (parryState == ParryState.Parried)
        {
            Debug.Log("�p���B�������̂��߃_���[�W����");
            return;
        }

        var hitDamage = collision.GetComponent<PlayerHitDamage>();
        if (hitDamage != null)
        {
            hitDamage.OnHitDamage(damageAmount);
            isAttacked = true;
        }
    }
}
