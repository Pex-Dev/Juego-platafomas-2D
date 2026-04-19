using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float fireRate = 0.2f;
    [SerializeField] private float fireRateCounter = 0f;
    public float bullerForce = 20f;
    public AudioClip sound; //Sonido del arma al atacar

    public GameObject bulletPrefab;

    void Update()
    {
        fireRateCounter += Time.deltaTime;
        if(fireRateCounter > fireRate)
        {
            fireRateCounter = fireRate;
        }
    }

    public void Attack(Transform shootPoint)
    {
        if(fireRateCounter < fireRate) return;
        fireRateCounter = 0f;

        //Obtener posición del mouse en el mundo
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; 

        //Calcular la direccion
        Vector2 direccion = (mousePos - transform.position).normalized;

        //Instaciar bala
        GameObject bala = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

        //Aplicar la velocidad
        bala.GetComponent<Rigidbody2D>().linearVelocity = direccion * bullerForce;
        ScenesManager.instance.PlaySound(sound);
    }
}
