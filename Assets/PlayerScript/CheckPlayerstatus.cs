using UnityEngine;

public class CheckPlayerstatus : MonoBehaviour
{
    [SerializeField] private float NearGroundamount = 0.1f;

    [SerializeField] private GameObject player;
    [SerializeField] private Transform playerTransform;

    void Start()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player");

        if (player == null)
            Debug.LogError("CheckPlayerstatus: Player‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ");

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

        Debug.LogWarning("CheckPlayerstatus: ’n–Ê‚ğŒŸo‚Å‚«‚Ü‚¹‚ñ");
        return float.NaN;
    }

    public bool CheckNearFloor(float groundYpos)
    {
        if (playerTransform == null)
        {
            Debug.LogError("CheckPlayerstatus: playerTransform‚ª–¢İ’è");
            return false;
        }

        float distanceToGround = Mathf.Abs(playerTransform.position.y - groundYpos);
        return distanceToGround <= NearGroundamount;
    }
}
