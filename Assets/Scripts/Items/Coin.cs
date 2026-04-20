using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private AudioClip[] coinSounds;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {   
            if(coinSounds.Length>0)ScenesManager.instance.PlaySound(coinSounds[Random.Range(0,coinSounds.Length)]);
            collision.gameObject.GetComponent<ObjectAbsorption>()?.AddCoins(5);
            Destroy(gameObject);
        }
    }
}
