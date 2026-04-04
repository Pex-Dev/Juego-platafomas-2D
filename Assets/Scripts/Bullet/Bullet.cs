using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject magicExplosion;
    private Rigidbody2D rb;
    void Start()
    {   
        rb = gameObject.GetComponent<Rigidbody2D>();
        Destroy(gameObject,2f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Ground"))
        {
            if(magicExplosion != null)Instantiate(magicExplosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        if(other.CompareTag("Enemy"))
        {   
            Vector2 knockback = new Vector2(3f, 4f);
            if (rb.linearVelocity.x > 0) 
            {
                knockback = new Vector2(3f, 4f);
            } 
            else if (rb.linearVelocity.x < 0) 
            {
                knockback = new Vector2(-3f, 4f);
            } 
            other.GetComponent<Enemy>()?.TakeDamage(5,knockback); 
            if(magicExplosion != null)Instantiate(magicExplosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        Debug.Log(other.name);
        if(other.CompareTag("BreakableBox"))
        {  
            other.GetComponent<BreakableBox>()?.TakeDamage(5); 
            Destroy(gameObject);
        }
    }
}
