using UnityEngine;

public class Greencross : MonoBehaviour
{
    [SerializeField] private float startxpos;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.SetParent(GameObject.FindWithTag("Player").transform); // �v���C���[�̎q�I�u�W�F�N�g�ɂ���
        transform.Translate(Vector3.right * startxpos); //�v���C���[�̑O�ɏo��悤�ɂ���

      

   
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
