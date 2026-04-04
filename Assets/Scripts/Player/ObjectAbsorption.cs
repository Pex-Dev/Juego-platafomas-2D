using UnityEngine;

public class ObjectAbsorption : MonoBehaviour
{
    
    void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Coin")) {
            float velocidadAtraccion = 5f;
            other.transform.position = Vector3.MoveTowards(
                other.transform.position, 
                transform.position, 
                velocidadAtraccion * Time.deltaTime
            );
        }
    }
}
