using UnityEngine;

public class PlayerAttack : MonoBehaviour
{   
    public bool canAttack = true; // Si puede atacar :v
    public Weapon weapon;
    public Transform shootPoint; // Desde donde dispara

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canAttack) // Clic izquierdo
        {
            Disparar();
        }
    }

    void Disparar()
    {
        weapon.Attack(shootPoint);
    }
}
