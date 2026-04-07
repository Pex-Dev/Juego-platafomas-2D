using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Weapon weapon;
    public Transform shootPoint; // Desde donde dispara

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Clic izquierdo
        {
            Disparar();
        }
    }

    void Disparar()
    {
        weapon.Attack(shootPoint);
    }
}
