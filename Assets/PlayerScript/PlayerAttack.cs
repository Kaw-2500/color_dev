using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void normalAttack(GameObject AttackPrefab)
    {
        // �}�E�X�̃��[���h���W���擾
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 playerPos = transform.position;

        GameObject attackObj = Instantiate(AttackPrefab, playerPos, Quaternion.identity);

        // BlueNormalAttack �̎Q�Ƃ��擾
        BlueNormalAttack blueAttack = attackObj.GetComponent<BlueNormalAttack>();

        if (blueAttack != null)
        {
            float baseSpeed = Mathf.Abs(blueAttack.Speed); // ����Speed���擾

            float direction = (mouseWorldPos.x >= playerPos.x) ? 1f : -1f;
            blueAttack.SetDirection(baseSpeed * direction,direction);
        }
    }

}
