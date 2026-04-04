using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isActive = false; //Define si el personaje puede interactuar con el mundo y el jugador
    
    public float totalLife = 15; //Vida todal del personaje
    public float currentLife; //Vida actual del personaje

    public float speed = 5f; //Velocidad de movimiento
    private float currentSpeed; //Velocida actual
    public float jumpForce = 12f; //Fuerza con la que salta
    private float currentJumpForce; //Fuerza actual con la que salta
    private float jumpDelay = 0.2f;
    [SerializeField] private float jumpDelayCounter;


    [SerializeField] private GameObject target; //Objetivo del personaje 


    [SerializeField] private DieAnimation dieAnimation; //Script de animación de muerte

    private BoxCollider2D bc; //Componente boxcollider2d del personaje
    private SpriteRenderer sr; //Componente SpriteRenderer del personaje
    private Rigidbody2D rb; //Componente rigidBody2D del personaje
    private Animator anim;


    private int direction = 1; //Dirección a la que mira el personaje

    [SerializeField] private bool isGrounded; //Si el personaje toca el suelo
    public float groundRadius = 2f;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;

    public bool isKnockback = false; //Si el jugador ha sido empujado o golpeado por un enemigo, entonces no debe poder moverse para no contrarestrar la fuerza del empuje 
    private float knockbackTimer = 0.5f;//Tiempo que dura el knockback
    private float knockbackTimerCounter;

    void Start()
    {
        currentLife = totalLife;
        currentJumpForce = jumpForce;
        currentSpeed = speed;

        bc = gameObject.GetComponent<BoxCollider2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        sr.flipX = direction == 0;
        CheckIsGroud();
        ChaseTarget();
        CheckKnockback();
    }


    void CheckIsGroud()
    {   
        float colliderHeigth = bc.size.y;

        Vector2 checkPosition = new Vector2(transform.position.x, transform.position.y - colliderHeigth/2 - 0.1f);

        isGrounded = Physics2D.OverlapCircle(
            checkPosition,
            groundRadius,
            groundLayer
        );
        if (isGrounded)
        {            
            jumpDelayCounter -= Time.deltaTime;
        }
        else
        {
            jumpDelayCounter = jumpDelay;
        }
    }

    private void ChaseTarget()
    {
        if(!isActive || !target)return;
        float colliderWidth = bc.size.x; //Ancho del collider del personaje
        
        Vector2 targetPosition = target.transform.position; //Posición del objetivo
        Vector2 ownPosition = transform.position;//Posicion del personaje
        

        direction = targetPosition.x < ownPosition.x ? 0 : 1;

        //Ver si esta cerca del objetivo
        float distanceX = Mathf.Abs(targetPosition.x - ownPosition.x);
        if (distanceX <= colliderWidth) 
        {
            StopMoving();
            return;
        }

        //Si está en el aire, que siga moviéndose y sale.
        if (!isGrounded)
        {
            Move(direction);
            return;
        }

        //Si detecta que DEBE saltar, salta y se mueve.
        bool mustJump = (isObjectAhead(direction,groundLayer,"top") && IsTargetAvobe()) || 
                        (!isGroundAhead(direction) && ThereIsGroundIfJump());

        if (mustJump)
        {
            Jump();
            Move(direction);
            return;
        }

        //Caminar solo si hay camino adelante y si no hay otro enemigo para que no se acoplen a lo maldito
        if (isGroundAhead(direction) && !isObjectAhead(direction,enemyLayer,"center",0.2f))
        {
            Move(direction);
        }
        else
        {
            StopMoving();
        }
    }

    void StopMoving()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        anim.SetBool("isMoving", false);
    }

    private void Move(int direction)
    {
        if (isKnockback)return;
        float moveDirection = direction == 1 ? 1.0f : -1.0f;
        rb.linearVelocity = new Vector2(moveDirection * speed, rb.linearVelocity.y); //Añadir velocidad en el eje x
        anim.SetBool("isMoving",true);
    }

    void Jump()
    {   
        if(!isActive || !isGrounded) return;        
        if(jumpDelayCounter <= 0){            
            float targetDistance = Vector2.Distance(transform.position, target.transform.position);
            //float jumpBonus = targetDistance > 6f ? 0.2f : 0f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); //Añadir velocidad en el eje y
        }        
    }



    private bool isGroundAhead(int direction)
    {   
        float colliderHeigth = bc.size.y;
        float colliderWidth = bc.size.x; //Ancho del collider del personaje

        Vector2 checkOrigen = direction == 1 ? 
                                    new Vector2(transform.position.x + colliderWidth/2 + 0.2f, transform.position.y - colliderHeigth/2):
                                    new Vector2(transform.position.x - colliderWidth/2 - 0.2f, transform.position.y - colliderHeigth/2);

        float distance = IsTargetBelow() ? 8f : 2f;
        RaycastHit2D hit = Physics2D.Raycast(
                            checkOrigen, 
                            Vector2.down, 
                            distance
                            ,groundLayer);
        return hit.collider != null;
    }


    private bool isObjectAhead(int direction, LayerMask layer, string verticalPosition = "center",float distance = 2f)
    {
        float colliderHeigth = bc.size.y; //Altura del collider del personaje
        float colliderWidth = bc.size.x; //Ancho del collider del personaje

        float positionY = transform.position.y;
        switch (verticalPosition)
        {
            case "center":
                positionY = transform.position.y;
                break;
            case "top":
                positionY = transform.position.y + colliderHeigth/2;
                break;
            case "bottom":
                positionY = transform.position.y - colliderHeigth/2;
                break;
            default:
                positionY = transform.position.y;
                break;
        }

        Vector2 checkOrigen = direction == 1 ? 
                                    new Vector2(transform.position.x + colliderWidth/2, positionY):
                                    new Vector2(transform.position.x - colliderWidth/2, positionY);

        Vector2 checkDirection = direction == 1 ?  Vector2.right : Vector2.left;


        RaycastHit2D hit = Physics2D.Raycast(
                            checkOrigen, 
                            checkDirection, 
                            distance
                            ,layer);

        Color debugColor = hit.collider != null ? Color.green : Color.red;
        Debug.DrawRay(checkOrigen, checkDirection * distance, debugColor);

        return hit.collider != null;
    }

    private bool IsTargetAvobe()
    {   
        if(!target) return false;

        float colliderHeigth = bc.size.y;

        float targetPosY = target.transform.position.y;
        float currentPosY = transform.position.y;

        return targetPosY > currentPosY + colliderHeigth;
    }

    private bool IsTargetBelow()
    {   
        if(!target) return false;

        float colliderHeigth = bc.size.y;

        float targetPosY = target.transform.position.y;
        float currentPosY = transform.position.y;

        return targetPosY < currentPosY - colliderHeigth;
    }

    private bool ThereIsGroundIfJump()
    {   
        //Código extraterreste
        float g = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);//Gravedad
        float tiempoSalto = 2 * jumpForce / g;

        float moveDirection = direction == 1 ? 1.0f : -1.0f;
        float jumpDistance = speed * tiempoSalto;

        float colliderHeigth = bc.size.y; //Altura del collider del personaje

        float alturaMaxima = jumpForce * jumpForce / (2 * g) - colliderHeigth/2;

        Vector2 puntoAterrizaje = new Vector2(
            transform.position.x + moveDirection * jumpDistance,
            transform.position.y + alturaMaxima
        );
        
        RaycastHit2D hit = Physics2D.Raycast(puntoAterrizaje, Vector2.down, 8f, groundLayer);

        Color raycastColor = hit.collider != null ? Color.green : Color.red;

        Debug.DrawRay(puntoAterrizaje, Vector2.down * 8f, raycastColor);
        return hit.collider;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            
            float dir = Mathf.Sign(other.transform.position.x - transform.position.x);

            Vector2 knockback = new Vector2(dir * 3f, 3f);

            other.GetComponent<PlayerMovement>().TakeDamage(1, knockback * 2);
        }
    }

    void CheckKnockback()
    {
        if (isKnockback)
        {
            knockbackTimerCounter -= Time.deltaTime;
            if(knockbackTimerCounter <= 0)
            {
                isKnockback = false;
                knockbackTimerCounter = knockbackTimer;
            }
        }
    }

    public void AddKnockBack(Vector2 knockback)
    {   
        if(isKnockback)return;        
        isKnockback = true;
        rb.linearVelocity = new Vector2(0f, 0f);
        rb.AddForce(knockback, ForceMode2D.Impulse);
        knockbackTimerCounter = knockbackTimer;      
    }

    public void TakeDamage(float damage, Vector2 knockback )
    {   
        AddKnockBack(knockback);
        currentLife -= damage;
        if(currentLife <= 0)
        {   
            dieAnimation.StartAnimation();
            Destroy(gameObject);
        }
    }

    void OnBecameVisible() {
        isActive = true;
    }
}
