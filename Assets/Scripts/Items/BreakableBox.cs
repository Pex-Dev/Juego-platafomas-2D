using System.Collections.Generic;
using UnityEngine;

public class BreakableBox : MonoBehaviour
{
    public int life;

    [SerializeField] private DieAnimation dieAnimation;

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
        
    }

    public void TakeDamage(int damage)
    {
        life -= damage;
        if (life <= 0)
        {
            dieAnimation.StartAnimation();
            DropItems();
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