using UnityEngine;

public class ObjectAbsorption : MonoBehaviour
{
    private UIController uIController;
    
    void Start()
    {        
        uIController = GameObject.Find("Canvas").GetComponent<UIController>();
    }

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

    public void AddCoins(int nCoins)
    {
        if(!uIController)return;
        uIController.AddCoins(nCoins);
    }
}
