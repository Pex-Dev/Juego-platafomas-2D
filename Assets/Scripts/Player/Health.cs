using UnityEngine;

public class Health : MonoBehaviour
{
    public int life = 5;//Vida del personaje
    private int maxLife;

    [SerializeField] private PlayerMovement pm;
    [SerializeField] private DieAnimation dieAnimation; //Script de animación de muerte

    private UIController uIController;

    void Start()
    {        
        uIController = GameObject.Find("Canvas").GetComponent<UIController>();
        maxLife = life;
    } 

    public void TakeDamage(int damage, Vector2 knockback)
    {
        int newLife = life - damage;
        if(newLife < 0) newLife = 0;
        life = newLife;
        uIController.SetLife(newLife);
        pm.AddKnockBack(knockback);
        if(life <= 0){
            dieAnimation.StartAnimation();
            Destroy(gameObject);
            uIController.SetLife(0);
            uIController.ShowDeathScreen(true);
        }
    }
}
