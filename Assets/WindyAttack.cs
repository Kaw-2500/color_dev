using UnityEngine;

// �G�����Ԋu��Windy�i���e�j���v���C���[�����֍U������X�N���v�g
public class WindyAttack : MonoBehaviour
{
    [SerializeField] private WeakEnemy weakEnemy; // �Q�Ɛ�̓G
    [SerializeField] private GameObject WindyPrefab; // ���e�v���n�u

    [SerializeField] private float fadeDuration = 2f; // �����ɂȂ�܂ł̎���
    [SerializeField] private float attackInterval = 2f; // �U���Ԋu
    [SerializeField] private float windySpeed = 10f; // ���e�̑��x

    private float timer = 0f;

    private bool isPlayerRight = true;

    void Start()
    {
        if (weakEnemy == null)
        {
            Debug.LogError("WindyAttack: weakEnemy���ݒ肳��Ă��܂���B");
        }
        if (WindyPrefab == null)
        {
            Debug.LogError("WindyAttack: WindyPrefab���ݒ肳��Ă��܂���B");
        }
    }

    void Update()
    {
        if (weakEnemy == null || WindyPrefab == null) return;

        timer += Time.deltaTime;

        if (timer >= attackInterval)
        {
            timer = 0f;
            AttackWindy();
        }
    }

    private void AttackWindy()
    {
        // �v���C���[�̑��Έʒu�Ɋ�Â��U�������𔻒�
        CheckPlayerDirection();

        // ���e�̐���
        GameObject windyInstance = Instantiate(WindyPrefab, weakEnemy.transform.position, Quaternion.identity);

        Rigidbody2D rb = windyInstance.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("WindyAttack: WindyPrefab��Rigidbody2D������܂���B");
            Destroy(windyInstance);
            return;
        }

        // ���e���v���C���[�����֗͂Ŕ���
        Vector2 forceDirection = isPlayerRight ? Vector2.right : Vector2.left;
        rb.AddForce(forceDirection * windySpeed, ForceMode2D.Impulse);

        // �t�F�[�h�A�E�g�ݒ�
        WindyFadeout fadeout = windyInstance.GetComponent<WindyFadeout>();
        if (fadeout != null)
        {
            fadeout.SetFadeDuration(fadeDuration);
        }
        else
        {
            Debug.LogWarning("WindyAttack: WindyPrefab��WindyFadeout���A�^�b�`����Ă��܂���B");
    
            Destroy(windyInstance);
        }

        // �K�v�ɉ����ăX�v���C�g�̍��E���]�ȂǂȂǁA�A�A
        //windyInstance.transform.localScale = new Vector3(isPlayerRight ? 1 : -1, 1, 1);
    }

    private void CheckPlayerDirection()
    {
        var playerPos = weakEnemy.GetPlayerRelativePosition();

        if (playerPos == WeakEnemy.PlayerRelativePosition.Right ||
            playerPos == WeakEnemy.PlayerRelativePosition.NearRight)
        {
            isPlayerRight = true;
        }
        else if (playerPos == WeakEnemy.PlayerRelativePosition.Left ||
                 playerPos == WeakEnemy.PlayerRelativePosition.NearLeft)
        {
            isPlayerRight = false;
        }
        else
        {
            Debug.LogWarning("WindyAttack: �v���C���[�̈ʒu���s���ł��B�U������������ł��܂���B");
            return;
        }
    }
}
