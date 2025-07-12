using UnityEngine;

public class CheckPlayerstatus : MonoBehaviour
//float,lbool�^�̒l���Ǘ����A���N���X�ɒ񋟂���N���X
//�����܂ł��l�݂̂𑪒肷�邾���ŁA���������A�v�Z�͑��̃N���X�ɔC���܂��B
//1�s�ŏI���Ȃ�if����A�����̌v�Z�͍s��Ȃ��ł�������
{
    [SerializeField] private float NearGroundamount = 0.1f;

    [SerializeField] private GameObject player;
    [SerializeField] private Transform playerTransform;

    void Start()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player");

        if (player == null)
            Debug.LogError("CheckPlayerstatus: Player��������܂���");

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

        Debug.LogWarning("CheckPlayerstatus: �n�ʂ����o�ł��܂���");
        return float.NaN;
    }

    public bool CheckNearFloor(float groundYpos)//0.7�ȏ��amount����Ȃ��ƁA�v���C���[�ɓ������Ă��Ēn�ʂɓ�����Ȃ�����isground��false�ɂȂ�
    {
        if (playerTransform == null)
        {
            Debug.LogError("CheckPlayerstatus: playerTransform�����ݒ�");
            return false;
        }

        float distanceToGround = Mathf.Abs(playerTransform.position.y - groundYpos);
        if (NearGroundamount < 0.7f)
        {
            Debug.LogWarning("CheckPlayerstatus: NearGroundamount��0.6�ȉ��ł��B�n�ʂɓ�����Ȃ��\��������܂��B");
        }
        return distanceToGround <= NearGroundamount;

    }
}