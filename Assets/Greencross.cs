using UnityEngine;

public class Greencross : MonoBehaviour
{
    [SerializeField] private EventManager eventManager; // EventManagerの参照  


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.SetParent(GameObject.Find("Player").transform); // プレイヤーの子オブジェクトにする
        transform.Translate(Vector3.right * 1.2f); //プレイヤーの前に出るようにする

      

        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        if (eventManager == null)
        {
            Debug.LogError("EventManager not found in the scene.");
        }
        else
        {
            //eventManager.IsPlAttack = true; // プレイヤーの攻撃状態を設定
        }
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
