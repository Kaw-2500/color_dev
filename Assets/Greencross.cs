using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Greencross : MonoBehaviour, IAttackStrategy
{
    [SerializeField] private float startxpos;

    private bool Attacked = false;
    private PlayerColorManager playerColorManager;
    private GameObject owner;
    private float direction = 1f; // �����̏����l

    private float ScaleX; // �X�P�[����X������ێ����邽�߂̕ϐ�
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Awake()
    {
                ScaleX = transform.localScale.x; // �����X�P�[����X������ۑ�
    }
    
    public void Execute(GameObject Owner, float dir)
    {
        // ���owner���Z�b�g���Ȃ���NullReference�ɂȂ�
        owner = Owner;
        direction = dir;

        if (owner == null)
        {
            Debug.LogError("Execute�ɓn���ꂽowner��null�ł��B");
            return;
        }

        playerColorManager = owner.GetComponent<PlayerColorManager>();
        if (playerColorManager == null)
        {
            Debug.LogError("PlayerColorManager��������܂���Bowner: " + owner.name);
            return;
        }

        Vector2 newpos = new Vector2(transform.position.x + startxpos * direction, transform.position.y);
        transform.position = newpos;

         transform.localScale = new Vector3(ScaleX * direction,
                                  transform.localScale.y, 
                                  transform.localScale.z); // �����ɉ����ăX�P�[���𒲐�

        this.transform.SetParent(owner.transform);
    }

    void FinishAttackDestroy()//animation����Ă�
    {
        this.transform.SetParent(null); // �e���������Ă���j��
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHitDamage enemy = collision.gameObject.GetComponent<EnemyHitDamage>();
            if (Attacked) return;

            Debug.Log("Fireball hit an enemy: " + enemy.name);
            enemy.HitAttackDamageOnEnemy(playerColorManager.GetCurrentData().NormalAttackPower);
            Attacked = true; // ��x�U��������t���O�𗧂Ă�

        }
        else
        {
        
        }
    }
}
