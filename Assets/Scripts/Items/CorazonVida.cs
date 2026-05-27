using UnityEngine;

public class CorazonVida : MonoBehaviour
{
    [SerializeField] private AudioClip[] coinSounds;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {   
            Health heal = collision.gameObject.GetComponent<Health>();
            if(!heal)return;
            if (heal.Heal(1))
            {                
                Destroy(gameObject);
            }
        }
    }
}
