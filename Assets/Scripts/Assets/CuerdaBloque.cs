using UnityEngine;

public class CuerdaBloque : MonoBehaviour
{
    [SerializeField] private BloquePiedra bloquePiedra;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("PlayerBullet"))
        {
            bloquePiedra.Activar();
        }
    }
}
