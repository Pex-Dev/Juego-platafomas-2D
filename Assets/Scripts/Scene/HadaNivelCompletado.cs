using UnityEngine;

public class HadaNivelCompletado : MonoBehaviour
{
    public bool moveToPlayer = false;
    public float speed = 7.5f;
    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (moveToPlayer && player)
        {
            Vector2 position = new Vector2(player.transform.position.x - 0.6f, player.transform.position.y + 1f);
            transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
        }
    }
}
