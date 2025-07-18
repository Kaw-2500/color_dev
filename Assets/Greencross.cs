using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Greencross : MonoBehaviour
{
    [SerializeField] private float startxpos;

    private bool Attacked = false;
    private PlayerColorManager playerColorManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.SetParent(GameObject.FindWithTag("Player").transform); // �v���C���[�̎q�I�u�W�F�N�g�ɂ���
        transform.Translate(Vector3.right * startxpos); //�v���C���[�̑O�ɏo��悤�ɂ���

        playerColorManager = GameObject.FindWithTag("Player").GetComponent<PlayerColorManager>();



    }

    // Update is called once per frame
    void Update()
    {

    }

    void FinishAttackDestroy()//animation����Ă�
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (Attacked) return;

            Debug.Log("Fireball hit an enemy: " + enemy.name);
            enemy.ApplyDamage(playerColorManager.GetCurrentData().NormalAttackPower);
            Attacked = true; // ��x�U��������t���O�𗧂Ă�

        }
    }
}
