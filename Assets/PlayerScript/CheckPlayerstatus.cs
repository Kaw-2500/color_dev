using UnityEngine;

public class CheckPlayerstatus : MonoBehaviour
{
    public bool IsFlying { get; private set; } = false;

    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private float yVelocityThreshold = 0.05f;

    void Reset()
    {
        if (playerRb == null)
            playerRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (playerRb == null)
        {
            Debug.LogWarning("CheckPlayerstatus: Rigidbody2D�����ݒ�ł�");
            return;
        }

        IsFlying = Mathf.Abs(playerRb.linearVelocity.y) > yVelocityThreshold;
    }
}
