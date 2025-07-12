using UnityEngine;

public class CheckPlayerstatus : MonoBehaviour
//float,lbool型の値を管理し、他クラスに提供するクラス
//あくまでも値のみを測定するだけで、条件分岐や、計算は他のクラスに任せます。
//1行で終わらないif文や、数字の計算は行わないでください
{
    [SerializeField] private float NearGroundamount = 0.1f;

    [SerializeField] private GameObject player;
    [SerializeField] private Transform playerTransform;

    void Start()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player");

        if (player == null)
            Debug.LogError("CheckPlayerstatus: Playerが見つかりません");

        if (player != null)
            playerTransform = player.transform;
    }

    public float GroundYposCheck()
    {
        LayerMask layerMask = ~LayerMask.GetMask("Player");
        Vector2 origin = playerTransform.position;
        Vector2 direction = Vector2.down;
        float maxDistance = 200f;

        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, maxDistance, layerMask);
        Debug.DrawRay(origin, direction * maxDistance, Color.red);

        float highestY = float.MinValue;
        bool foundFloor = false;

        foreach (var hit in hits)
        {
            if (hit.collider == null) continue;

            //Debug.Log("Ray hit: " + hit.collider.name + " tag: " + hit.collider.tag);

            string tag = hit.collider.tag;
            if (tag == "Redfloor" || tag == "Bluefloor" || tag == "Greenfloor" || tag == "naturalfloor")
            {

                if (hit.point.y > highestY)
                {
                    highestY = hit.point.y;
                    foundFloor = true;
                }
            }
        }

        if (foundFloor)
            return highestY;

        Debug.LogWarning("CheckPlayerstatus: 地面を検出できません");
        return float.NaN;
    }

    public bool CheckNearFloor(float groundYpos)//0.7以上のamountじゃないと、プレイヤーに当たっていて地面に当たらないからisgroundがfalseになる
    {
        if (playerTransform == null)
        {
            Debug.LogError("CheckPlayerstatus: playerTransformが未設定");
            return false;
        }

        float distanceToGround = Mathf.Abs(playerTransform.position.y - groundYpos);
        if (NearGroundamount < 0.7f)
        {
            Debug.LogWarning("CheckPlayerstatus: NearGroundamountが0.6以下です。地面に当たらない可能性があります。");
        }
        return distanceToGround <= NearGroundamount;

    }
}