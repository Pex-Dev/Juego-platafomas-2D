using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startPos;
    public GameObject cam;
    public float parallaxEffect; // 0 = se mueve con la cámara, 1 = no se mueve (estático)

    void Start()
    {
        startPos = transform.position.x;
    }

    void Update()
    {
        // Calculamos cuánto se debe haber movido el fondo
        float distance = (cam.transform.position.x * parallaxEffect);
        
        // Aplicamos la nueva posición
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
    }
}

