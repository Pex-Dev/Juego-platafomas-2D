using UnityEngine;
using System.Linq;

public class EnemyTrigger : MonoBehaviour
{
    public GameObject[] enemies;
    void Start()
    {   
        enemies = GetComponentsInChildren<Transform>()
            .Where(t => t != transform)
            .Select(t => t.gameObject)
            .ToArray();

        for (int i = 0; i < enemies.Length; i++)
        {
            ActiveEnemy(enemies[i],false);
        }
    }

    void ActiveEnemy(GameObject enemyGameObject, bool value)
    {   
        if(!enemyGameObject)return;
        
        Enemy enemy = enemyGameObject.GetComponent<Enemy>();
        if(!enemy)return;
        enemy.isActive = value;
        SpriteRenderer sr = enemyGameObject.GetComponent<SpriteRenderer>();

        if (!sr) return;
        sr.enabled = value;

        Rigidbody2D rb = enemyGameObject.GetComponent<Rigidbody2D>();
        if (!rb) return;
         rb.simulated = value;
    }

    void OnTriggerEnter2D(Collider2D other)
    {   
        if(other.CompareTag("Player"))
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                ActiveEnemy(enemies[i],true);
            }
        }
    }
}
