using UnityEngine;

public class ArmAim : MonoBehaviour
{
    
    private SpriteRenderer sr; //Componente SpriteRenderer
    [SerializeField] private SpriteRenderer weaponSr; //Componente SpriteRenderer del arma;
    [SerializeField] private PlayerMovement player; //Componente SpriteRenderer del arma;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Obtener posición del mouse en el mundo del juego
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = mousePos - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);

        //sr.flip = flip;
        

        // Invertir dependiendo si esta el cursor en la izquierda o derecha
        if (mousePos.x < transform.position.x)
        {
            transform.localScale = new Vector3(1, -1, 1);
            sr.sortingOrder = -1;
            weaponSr.sortingOrder = -1;
            player.SetArmDirection(-1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
            sr.sortingOrder = 1;
            weaponSr.sortingOrder = 1;
            player.SetArmDirection(1);
        }
    }
}
