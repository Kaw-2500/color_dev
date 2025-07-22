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
        // マウスのワールド座標を取得
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 playerPos = transform.position;

        GameObject attackObj = Instantiate(AttackPrefab, playerPos, Quaternion.identity);

        // BlueNormalAttack の参照を取得
        BlueNormalAttack blueAttack = attackObj.GetComponent<BlueNormalAttack>();

        if (blueAttack != null)
        {
            float baseSpeed = Mathf.Abs(blueAttack.Speed); // 初期Speedを取得

            float direction = (mouseWorldPos.x >= playerPos.x) ? 1f : -1f;
            blueAttack.SetDirection(baseSpeed * direction,direction);
        }
    }

}
