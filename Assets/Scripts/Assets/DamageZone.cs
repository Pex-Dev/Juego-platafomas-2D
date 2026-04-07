using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public int damage = 1;
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

            other.GetComponent<Health>()?.TakeDamage(damage, knockback * 2);
        }
        if(other.CompareTag("Enemy"))
        {
            
            float dir = Mathf.Sign(other.transform.position.x - transform.position.x);

            Vector2 knockback = new Vector2(dir * 3f, 3f);

            other.GetComponent<Enemy>()?.TakeDamage(5, knockback * 2);
        }
    }
}
