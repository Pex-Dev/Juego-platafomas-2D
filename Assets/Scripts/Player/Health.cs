using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int life = 5;//Vida del personaje
    private int maxLife;

    [SerializeField] private PlayerMovement pm;
    [SerializeField] private DieAnimation dieAnimation; //Script de animación de muerte

    private SpriteRenderer[] spriteRenderers;
    private UIController uIController;

    public bool invulnerability = false;

    [SerializeField] private AudioClip[] hurtSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip healSound;

    void Start()
    {        
        uIController = GameObject.Find("Canvas").GetComponent<UIController>();
        maxLife = life;

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    IEnumerator StartInvulneraviltyTime()
    {
        yield return new WaitForSeconds(0.2f);
        AllSpriteRenderers(new Color(1f, 1f, 1f, 0f));
        yield return new WaitForSeconds(0.2f);
        AllSpriteRenderers(new Color(1f, 1f, 1f, 1f));
        yield return new WaitForSeconds(0.2f);
        AllSpriteRenderers(new Color(1f, 1f, 1f, 0f));
        yield return new WaitForSeconds(0.2f);
        AllSpriteRenderers(new Color(1f, 1f, 1f, 1f));
        invulnerability = false;
    }

    public void TakeDamage(int damage, Vector2 knockback, bool ignoreInvulneravility = false)
    {
        //Si el jugador esta en su tiempo de invulneravilidad no recibe daño
        if (!ignoreInvulneravility)
        {            
            if(invulnerability)return;
        }
        invulnerability = true;
        
        StartCoroutine(StartInvulneraviltyTime());

        //Recibir daño
        int newLife = life - damage;
        if(newLife < 0) newLife = 0;
        life = newLife;

        //Reproducir sonido
        PlayHurtSound();

        //Actualizar vida en el UI
        uIController.SetLife(newLife);
        
        //Añadir knockback
        pm.AddKnockBack(knockback);

        //Morir si tienes 0 vida
        if(life <= 0){
            if(deathSound != null) ScenesManager.instance.PlaySound(deathSound);
            dieAnimation.StartAnimation();
            Destroy(gameObject);
            uIController.SetLife(0);
            uIController.ShowDeathScreen(true);
        }        
    }

    public bool Heal(int value)
    {
        if(life >= maxLife) return false;
        ScenesManager.instance.PlaySound(healSound);
        life += value;
        if(life >= maxLife) life = maxLife;        
        uIController.SetLife(life);
        return true;
    }

    private void AllSpriteRenderers(Color color)
    {
        foreach (SpriteRenderer sprite in spriteRenderers)
        {
            sprite.color = color;
        }
    }

    public void PlayHurtSound()
    {
        ScenesManager.instance.PlaySound(hurtSound[Random.Range(0,hurtSound.Length)]);
    }
}
