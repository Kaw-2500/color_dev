using UnityEngine;

public class Greencross : MonoBehaviour
{
    [SerializeField] private EventManager eventManager; // EventManager�̎Q��  


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.SetParent(GameObject.Find("Player").transform); // �v���C���[�̎q�I�u�W�F�N�g�ɂ���
        transform.Translate(Vector3.right * 1.2f); //�v���C���[�̑O�ɏo��悤�ɂ���

      

        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        if (eventManager == null)
        {
            Debug.LogError("EventManager not found in the scene.");
        }
        else
        {
            //eventManager.IsPlAttack = true; // �v���C���[�̍U����Ԃ�ݒ�
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
