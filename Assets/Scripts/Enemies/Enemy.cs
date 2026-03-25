using System;
using NUnit.Framework;
using Unity.VisualScripting;
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

    private BoxCollider2D bc; //Componente boxcollider2d del personaje
    private SpriteRenderer sr; //Componente SpriteRenderer del personaje
    private Rigidbody2D rb; //Componente rigidBody2D del personaje
    private Animator anim;


    private int direction = 1; //Dirección a la que mira el personaje

    [SerializeField] private bool isGrounded; //Si el personaje toca el suelo
    public float groundRadius = 2f;
    public LayerMask groundLayer;


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

        if(targetPosition.x < ownPosition.x + colliderWidth || targetPosition.x > ownPosition.x + colliderWidth)
        {
            //Si esta cayendo o saltando que siga nomas
            if (!isGrounded)
            {
                Move(direction);
                return;
            }

            //Verificar si debería saltar
            bool mustJump = (isWallAhead(direction) && IsTargetAvobe()) || !isGroundAhead(direction) && ThereIsGroundIfJump();

            //Si no debe saltar
            if (!mustJump)
            {
                //Moverse solo si hay suelo adelante
                if (isGroundAhead(direction))
                {
                    Move(direction);
                }
                else
                {
                    rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                    anim.SetBool("isMoving",false);
                }
                return;
            }

            Jump();
            Move(direction);//Moverse
            return;
                        
        }else{
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            anim.SetBool("isMoving",false);
        }
    }

    private void Move(int direction)
    {       
        float targetDistance = Vector2.Distance(transform.position, target.transform.position);
        
        //float speedBonus = targetDistance > 6f ? 0.3f : 0.3f;//Aumentar la velocida para el salto

        //float movSpeed = isGrounded ? speed : speed + speed*speedBonus;
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


    private bool isWallAhead(int direction)
    {
        float colliderHeigth = bc.size.y; //Altura del collider del personaje
        float colliderWidth = bc.size.x; //Ancho del collider del personaje

        Vector2 checkOrigen = direction == 1 ? 
                                    new Vector2(transform.position.x + colliderWidth/2, transform.position.y + colliderHeigth/2):
                                    new Vector2(transform.position.x - colliderWidth/2, transform.position.y + colliderHeigth/2);

        Vector2 checkDirection = direction == 1 ?  Vector2.right : Vector2.left;


        RaycastHit2D hit = Physics2D.Raycast(
                            checkOrigen, 
                            checkDirection, 
                            2f
                            ,groundLayer);
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

            other.GetComponent<PlayerMovement>().AddKnockBack(knockback * 2);
        }
    }


    public void TakeDamage(float damage )
    {
        currentLife -= damage;
        if(currentLife <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnBecameVisible() {
        isActive = true;
    }
}
