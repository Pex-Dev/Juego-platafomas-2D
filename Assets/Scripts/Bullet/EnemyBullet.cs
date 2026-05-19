using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public GameObject magicExplosion;
    private Rigidbody2D rb;
    void Start()
    {   
        rb = gameObject.GetComponent<Rigidbody2D>();
        Destroy(gameObject,2f);
    }

    void Update()
    {
        if (!GetComponent<Renderer>().isVisible)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Ground"))
        {
            if(magicExplosion != null)Instantiate(magicExplosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        if(other.CompareTag("Player"))
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
            other.GetComponent<Health>()?.TakeDamage(1,knockback); 
            if(magicExplosion != null)Instantiate(magicExplosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
