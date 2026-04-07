using UnityEngine;

public class BloquePiedra : MonoBehaviour
{
    private Rigidbody2D rb; //Componente RigidBody2D;
    private bool active = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Activar()
    {
        rb.simulated = true;
        active = true;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (active)
            {
                active = false;
                rb.simulated = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {   
        if(!active)return;
        if(other.CompareTag("Player"))
        {
            
            float dir = Mathf.Sign(other.transform.position.x - transform.position.x);

            Vector2 knockback = new Vector2(dir * 3f, 3f);

            other.GetComponent<Health>()?.TakeDamage(1000, knockback * 2);
        }
        if(other.CompareTag("Enemy"))
        {
            
            float dir = Mathf.Sign(other.transform.position.x - transform.position.x);

            Vector2 knockback = new Vector2(dir * 3f, 3f);

            other.GetComponent<Enemy>()?.TakeDamage(1000, knockback * 2);
        }
    }
}
