using System.Collections.Generic;
using UnityEngine;

public class BreakableBox : MonoBehaviour
{
    public int life;

    [SerializeField] private DieAnimation dieAnimation;
    [SerializeField] private AudioClip breakSound; //Sonido que se reproduce al romper

    private Health playerHealth;

    [SerializeField] private GameObject corazonVida;

    [System.Serializable]
    public class DropConfig
    {
        public GameObject prefab;
        public int cantidadMin = 3;
        public int cantidadMax = 5;
    }

    [Header("Configuración de Recompensas")]
    public List<DropConfig> posiblesDrops;

    void Start()
    {
        playerHealth = GameObject.Find("Player")?.GetComponent<Health>();
    }

    public void TakeDamage(int damage)
    {
        life -= damage;
        if (life <= 0)
        {
            dieAnimation.StartAnimation();

            if(breakSound!=null) ScenesManager.instance.PlaySound(breakSound);

            DropItems();

            if (playerHealth != null)
            {
                if(Random.Range(0,100)<=30 && playerHealth.life < playerHealth.maxLife)
                {
                    Instantiate(corazonVida, transform.position, Quaternion.identity);
                }
            }

            Destroy(gameObject);
        }
    }
    private void DropItems()
    {
        foreach (DropConfig drop in posiblesDrops)
        {
            // Calculamos una cantidad aleatoria basada en el rango
            int cantidad = Random.Range(drop.cantidadMin, drop.cantidadMax + 1);

            for (int i = 0; i < cantidad; i++)
            {
                // Instanciamos el objeto en la posición actual
                Instantiate(drop.prefab, transform.position, Quaternion.identity);
            }
        }
    }
}