using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject magicExplosion;
    void Start()
    {
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
            other.GetComponent<Enemy>()?.TakeDamage(5); 
            if(magicExplosion != null)Instantiate(magicExplosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
