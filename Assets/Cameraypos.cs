using UnityEngine;

public class Camerapos : MonoBehaviour
{
    public Transform playerTransform;
    [SerializeField]
    float camerahight = 5f; // player��y���W����ǂꂾ�������ʒu�ɃJ������u����
    [SerializeField]
    float cameradistance = 5f; // player��x���W����ǂꂾ�����ꂽ�ʒu�ɃJ������u����

    public GameObject player;
    [SerializeField]private Player script;

    void Start()
    {
        script = player.GetComponent<Player>();

        if (playerTransform == null)
        {
            Debug.LogError("Player Transform is not assigned.");
        }
    }

    void Update()
    {
        FollowPlayerNormally();
    }

    void FollowPlayerNormally()
    {
        if (playerTransform == null) return;

        float groundypos = script.groundypos;
        //Debug.Log("groundypos: " + groundypos);

        Vector3 newPosition = transform.position;
        newPosition.x = playerTransform.position.x + cameradistance;
        newPosition.y = groundypos + camerahight;

        transform.position = newPosition;
    }
}
