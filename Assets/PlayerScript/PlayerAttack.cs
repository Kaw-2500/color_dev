using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void normalAttack(GameObject AttackPrefab)
    {
        //‚¹‚¢‚¹‚¢‚¶‚Ìˆ—‚ğ‚±‚±‚ÅÀs
        Instantiate(AttackPrefab, transform.position, Quaternion.identity);
    }
}
