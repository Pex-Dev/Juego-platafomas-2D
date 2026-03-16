using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject bulletPrefab; //Bala
    public Transform shootPoint; // Desde donde dispara
    public float bullerForce = 20f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Clic izquierdo
        {
            Disparar();
        }
    }

    void Disparar()
    {
        //Obtener posición del mouse en el mundo
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; 

        //Calcular la direccion
        Vector2 direccion = (mousePos - transform.position).normalized;

        //Instaciar bala
        GameObject bala = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

        //Aplicar la velocidad
        bala.GetComponent<Rigidbody2D>().linearVelocity = direccion * bullerForce;
    }
}
