using UnityEngine;

public class DeathZone : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            
            float dir = Mathf.Sign(other.transform.position.x - transform.position.x);

            Vector2 knockback = new Vector2(dir * 3f, 3f);

            other.GetComponent<Health>().TakeDamage(1000, knockback * 2);
        }
    }
}
