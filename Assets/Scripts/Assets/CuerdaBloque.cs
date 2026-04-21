using UnityEngine;

public class CuerdaBloque : MonoBehaviour
{
    [SerializeField] private BloquePiedra bloquePiedra;

    private bool activated = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("PlayerBullet") && !activated)
        {
            bloquePiedra.Activar();
            activated = true;
        }
    }
}
