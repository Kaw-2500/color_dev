using UnityEngine;

public class Greencross : MonoBehaviour
{
    [SerializeField] private float startxpos;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.SetParent(GameObject.FindWithTag("Player").transform); // プレイヤーの子オブジェクトにする
        transform.Translate(Vector3.right * startxpos); //プレイヤーの前に出るようにする

      

   
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FinishAttackDestroy()
    {
        Destroy(this.gameObject);
    }
}
