using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int life = 5;//Vida del personaje
    private int maxLife;

    public float speed = 10f; //Velocidad del jugador al moverse
    public float jumpForce = 12f; //Fuerza con la que salta

    private Rigidbody2D rb; //Componente RigidBody2D;
    private Animator anim; //Componente Animator;
    private SpriteRenderer sr; //Componente SpriteRenderer

    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundLayer;

    private bool isGrounded; //Si el jugador toca el suelo

    private ArmAim arm; //Brazo del personaje
    private SpriteRenderer srLeftArm; //Componente SpriteRenderer del brazo izquierdo
    
    [SerializeField] private int armDirection = 1;

    private float coyoteTime = 0.2f; //Tiempo máximo del jugador en el aire para poder saltar luego de caer por un borde
    private float coyoteTimeCounter;

    public bool isKnockback = false; //Si el jugador ha sido empujado o golpeado por un enemigo, entonces no debe poder moverse para no contrarestrar la fuerza del empuje 
    private float knockbackTimer = 0.8f;//Tiempo que dura el knockback
    private float knockbackTimerCounter;

    private UIController uIController;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        arm = transform.Find("ArmR").gameObject.GetComponent<ArmAim>();
        srLeftArm = transform.Find("ArmL").gameObject.GetComponent<SpriteRenderer>();
        uIController = GameObject.Find("Canvas").GetComponent<UIController>();
    }

    // Update is called once per frame
    void Update()
    {    
        CheckKnockback();
        CheckIsGroud();
        Move();
        Jump();
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

    void CheckIsGroud()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundRadius,
            groundLayer
        );

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal"); //Obtiene el input -1.0 y 1.0

        //No debe poder moverse si esta siendo empujado
        if (!isKnockback)
        {
            rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y); //Añadir velocidad en el eje x            
        }
        anim.SetBool("isMoving", moveInput != 0 && isGrounded);


        //Si el jugador esta apretando las teclas para mover
        if(moveInput != 0)
        {   
            //Si se mueve en la misma dirección que apunta, continuar
            if(armDirection == Math.Sign(moveInput))
            {                
                sr.flipX = moveInput < 0;
                srLeftArm.enabled = moveInput < 0;
            }//Usar dirección de apuntado
            else
            {
                sr.flipX = armDirection < 0;
                srLeftArm.enabled = armDirection < 0;
            }
        }//Usar dirección de apuntado
        else
        {
            sr.flipX = armDirection < 0;
            srLeftArm.enabled = armDirection < 0;
        }
    }

    void Jump()
    {
        //Saltar si se presiona espacio y esta tocando el suelo
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded ||  coyoteTimeCounter >= 0f))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); //Añadir velocidad en el eje y
        }
    }

    //Establecer dirección de apuntado
    public void SetArmDirection(int direction = 1)
    {
        armDirection = direction;
    }

    public void AddKnockBack(Vector2 knockback)
    {   
        if(isKnockback)return;        
        isKnockback = true;
        rb.linearVelocity = new Vector2(0f, 0f);
        rb.AddForce(knockback, ForceMode2D.Impulse);
        knockbackTimerCounter = knockbackTimer;      
    }

    public void TakeDamage(int damage, Vector2 knockback)
    {
        int newLife = life - damage;
        if(newLife < 0) newLife = 0;
        life = newLife;
        uIController.SetLife(newLife);
        AddKnockBack(knockback);
    }
}
