using UnityEngine;

public class ThunderBar : MonoBehaviour
{
    [SerializeField] public GameObject GrayThunder;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        LocateThunder();
    }

    void LocateThunder()
    {
        transform.position = new Vector3(GrayThunder.transform.position.x, GrayThunder.transform.position.y, 0);
    }
}
