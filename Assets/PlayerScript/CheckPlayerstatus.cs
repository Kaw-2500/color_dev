using UnityEngine;

public class CheckPlayerstatus : MonoBehaviour
{
    [SerializeField] private float NearGroundamount = 0.1f; // 近接判定の距離（m）

    [SerializeField] private GameObject player;
 [SerializeField]   private Transform playerTransform;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            if (player == null)
            {
                Debug.LogError("Player GameObject not found. Please assign it in the inspector or ensure it has the 'Player' tag.");
            }
        }

        if (player != null)
        {
            playerTransform = player.transform;
        }
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
                Debug.Log($"Raycast hit floor: {hit.collider.name} at Y={hit.point.y}");

                if (hit.point.y > highestY)
                {
                    highestY = hit.point.y;
                    foundFloor = true;
                }
            }
        }

        if (foundFloor) return highestY;

        Debug.LogWarning("No ground floor detected by raycast.");
        return float.NaN;
    }

    public bool CheckNearFloor(float groundYpos)
    {
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform is not assigned.");
            return false;
        }

        float distanceToGround = Mathf.Abs(playerTransform.position.y - groundYpos);
        if (distanceToGround <= NearGroundamount)
        {
            Debug.Log("Player is near the ground floor.");
            return true;
        }
        else
        {
            Debug.Log("Player is not near the ground floor.");
            return false;
        }
    }
}
