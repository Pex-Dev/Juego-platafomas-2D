using System;
using UnityEngine;

public enum EnemyType { Meele, Range}

public class Enemy : MonoBehaviour
{
    public bool isActive = false; //Define si el personaje puede interactuar con el mundo y el jugador
    public EnemyType enemyType = EnemyType.Meele;
    public bool flyingEnemy = false; //Si el enemigo es volador 


    public float totalLife = 15; //Vida todal del personaje
    public float currentLife; //Vida actual del personaje

    public int attack = 1;
    public float attackSpeed = 1f;
    private float attackSpeedTimer = 0;
    [SerializeField] private bool canAttack = true;

    public GameObject bullet; //Bala o proyectil que lanza si es un enemigo rango

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

    [SerializeField] private AudioClip[] hurtSounds;
    [SerializeField] private AudioClip deathSound;

    void Start()
    {
        currentLife = totalLife;
        currentJumpForce = jumpForce;
        currentSpeed = speed;

        bc = gameObject.GetComponent<BoxCollider2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        if (flyingEnemy)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            knockbackTimer = 0.2f;
        }

        if (!target)
        {
            target = GameObject.Find("Player");
        }        
    }

    // Update is called once per frame
    void Update()
    {
        sr.flipX = direction <= 0;
        CheckIsGroud();
        ChaseTarget();
        CheckKnockback();
        CanAttack();
    }

    void CanAttack()
    {
        if (!isActive || Time.deltaTime == 0)return; 
        
        if(attackSpeedTimer > 0 && !canAttack){
            attackSpeedTimer -= Time.deltaTime;
            if(attackSpeedTimer <= 0 ) canAttack = true;
        }
        else
        {   
            if(canAttack)return;
            attackSpeedTimer = attackSpeed;
        }
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
        

        direction = targetPosition.x < ownPosition.x ? -1 : 1;

        //Ataque meele
        if (enemyType == EnemyType.Meele)
        {
            float distanceX = Mathf.Abs(targetPosition.x - ownPosition.x);
            if (distanceX <= colliderWidth) 
            {
                StopMoving();
                return;
            }
        }
        else if(enemyType == EnemyType.Range)
        {
            float targetDistance = Vector2.Distance(target.transform.position, transform.position);
            if(targetDistance <= 8f && GetComponent<Renderer>().isVisible)
            {
                RangeAttack();
                StopMoving();
                rb.linearVelocity = new Vector2(rb.linearVelocity.x,0);
                return;
            }
        }
        
        
        if(flyingEnemy)
        {
            FlyingEnemyMovement(targetPosition,direction);
        }
        else
        {
            TerrestialEnemyMovement(direction);
        }

        
    }

private void FlyingEnemyMovement(Vector2 targetPosition, int direction)
{   
    float margin = 0.2f; 
    
    
    float diffX = targetPosition.x - transform.position.x;
    Vector2 horizontalDir = direction == 1 ? Vector2.right : Vector2.left;

    if (Mathf.Abs(diffX) > margin && !IsObjectAhead(horizontalDir, groundLayer, VerticalOffset.Center, 1f))
    {
        MoveHorizontal(direction);
    }
    else
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); 
    }

    float diffY = targetPosition.y - transform.position.y;
    Vector2 verticalDir = diffY > 0 ? Vector2.up : Vector2.down;

    // Solo se mueve si la distancia vertical es mayor al margen
    if (Mathf.Abs(diffY) > margin && !IsObjectAhead(verticalDir, groundLayer, VerticalOffset.Center, 1f))
    {
        MoveVertical(verticalDir);
    }
    else
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); 
    }
}



    private void TerrestialEnemyMovement(int direction)
    {
        //Si esta en el aire, que siga moviéndose y sale.
        if (!isGrounded)
        {
            MoveHorizontal(direction);
            return;
        }

        Vector2 horizontalDir = direction == 1 ? Vector2.right : Vector2.left;
        //Si detecta que DEBE saltar, salta y se mueve.
        bool mustJump = (IsObjectAhead(horizontalDir,groundLayer,VerticalOffset.Top) && IsTargetAvobe()) || 
                        (!isGroundAhead(direction) && ThereIsGroundIfJump());

        if (mustJump)
        {
            Jump();
            MoveHorizontal(direction);
            return;
        }

        //Caminar solo si hay camino adelante y si no hay otro enemigo para que no se acoplen a lo maldito
        if (isGroundAhead(direction) && !IsObjectAhead(horizontalDir,enemyLayer,VerticalOffset.Center,0.2f))
        {
            MoveHorizontal(direction);
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

    private void MoveHorizontal(int direction)
    {
        if (isKnockback)return;
        float moveDirection = direction == 1 ? 1.0f : -1.0f;
        rb.linearVelocity = new Vector2(moveDirection * speed, rb.linearVelocity.y); //Añadir velocidad en el eje x
        anim.SetBool("isMoving",true);
    }

    private void MoveVertical(Vector2 direction)
    {
        if (isKnockback)return;
        float moveDirection = direction == Vector2.down ? -1f : 1f;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, moveDirection * speed); //Añadir velocidad en el eje y
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

    public void MeeleAttack()
    {
        
    }

    public void RangeAttack()
    {
        if(!isActive && target == null) return;

        float targetDistance = Vector2.Distance(target.transform.position, transform.position);

        if(CanSeeTarget() && canAttack){
            if(!bullet)return;

            GameObject  instantiatedBullet = Instantiate(bullet, transform.position, Quaternion.identity);

            Vector2 direccion = (target.transform.position - transform.position).normalized;

            //Aplicar la velocidad
            instantiatedBullet.GetComponent<Rigidbody2D>().linearVelocity = direccion * 8f;
            //ScenesManager.instance.PlaySound(sound);

            canAttack = false;
            attackSpeedTimer = attackSpeed;
        }
    }


    private bool CanSeeTarget()
    {
        Vector2 origen = transform.position;
        Vector2 direccion = (Vector2)target.transform.position - origen;
        float distancia = direccion.magnitude;

        // Usamos Raycast2D
        RaycastHit2D hit = Physics2D.Raycast(origen, direccion, distancia, groundLayer);

        if (hit.collider == null)
        {
            Debug.DrawRay(transform.position, direccion, Color.green);
            return true;
        }
        Debug.DrawRay(transform.position, direccion, Color.red);
        return false;
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


    public enum VerticalOffset { Center, Top, Bottom }

    private bool IsObjectAhead(Vector2 direction, LayerMask layer, VerticalOffset vPos = VerticalOffset.Center, float distance = 2f)
    {
        // 1. Calculamos el offset perpendicular a la dirección para "Top" y "Bottom"
        // Si vas horizontal (X), el offset es en Y. Si vas vertical (Y), el offset es en X.
        float offsetX = 0;
        float offsetY = 0;

        if (vPos != VerticalOffset.Center)
        {
            float factor = (vPos == VerticalOffset.Top) ? 0.5f : -0.5f;
            
            if (Mathf.Abs(direction.x) > 0) // Movimiento horizontal
                offsetY = bc.size.y * factor;
            else // Movimiento vertical
                offsetX = bc.size.x * factor;
        }

        // 2. Ajustamos el origen al borde del collider según la dirección
        // Multiplicamos el tamaño del collider por la dirección para que el rayo salga del borde exacto
        Vector2 origin = new Vector2(
            transform.position.x + (direction.x * bc.size.x / 2) + offsetX,
            transform.position.y + (direction.y * bc.size.y / 2) + offsetY
        );

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, layer);

        // Debug visual para las 4 direcciones
        Debug.DrawRay(origin, direction * distance, hit ? Color.green : Color.red);

        return hit.collider != null;
    }

    //Verificar si el objetivo esta arriba
    private bool IsTargetAvobe()
    {   
        if(!target) return false;

        float colliderHeigth = bc.size.y;

        float targetPosY = target.transform.position.y;
        float currentPosY = transform.position.y;

        return targetPosY > currentPosY + colliderHeigth;
    }

    //Verificar si el objetivo esta abajo
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
        if(other.CompareTag("Player") && canAttack && enemyType == EnemyType.Meele)
        {
            
            float dir = Mathf.Sign(other.transform.position.x - transform.position.x);

            Vector2 knockback = new Vector2(dir * 3f, 3f);

            other.GetComponent<Health>().TakeDamage(1, knockback * 2);
            canAttack = false;
            attackSpeedTimer = attackSpeed;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Player") && canAttack && enemyType == EnemyType.Meele)
        {
            
            float dir = Mathf.Sign(other.transform.position.x - transform.position.x);

            Vector2 knockback = new Vector2(dir * 3f, 3f);

            other.GetComponent<Health>().TakeDamage(1, knockback * 2);
            canAttack = false;
            attackSpeedTimer = attackSpeed;
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

        //reproducir sonidos de daño
        if(hurtSounds.Length>0)ScenesManager.instance.PlaySound(hurtSounds[ UnityEngine.Random.Range(0,hurtSounds.Length) ]);

        if(currentLife <= 0)
        {   
            dieAnimation.StartAnimation();
            if(deathSound!=null)ScenesManager.instance.PlaySound(deathSound);
            Destroy(gameObject);
        }
    }

    void OnBecameVisible() {
        //Solo activar con cámara del juego
        if (Camera.current != null && Camera.current.name == "SceneCamera")return;
        isActive = true;
    }
}
