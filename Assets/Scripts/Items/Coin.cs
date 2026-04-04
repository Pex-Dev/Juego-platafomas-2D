using UnityEngine;

public class Coin : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {   
            collision.gameObject.GetComponent<ObjectAbsorption>()?.AddCoins(5);
            Destroy(gameObject);
        }
    }
}
