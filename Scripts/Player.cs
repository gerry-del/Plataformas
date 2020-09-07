using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //para acceder a las propiedades single tone
    public static Player obj;

    public int lives = 3; //para las vidas

    //para saber que esta haciendo el personaje
    public bool isGrounded = false; 
    public bool isMoving = false;
    public bool isInmune = false;

    //para que se mueva
    public float speed = 5f;
    public float jumpforce = 3f;
    public float movHor;

    public float inmuneTimeCnt = 0f;
    public float inmuneTime = 0.5f;

    //para conocer si el personaje esta tocando el piso
    public LayerMask groundLayer;
    public float radius = 0.3f;
    public float groundRayDist = 0.5f;

    //para acceder a los componentes del personaje
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spr;



    void Awake()
    {
        obj = this; //configurar el single tone
        
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //para acceder al componente de rigidbody 2D y sus propiedades en rb
        anim = GetComponent<Animator>();  //para acceder al componente animator
        spr = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Game.obj.gamePaused)
        {
            movHor = 0f;
            return;
        }
        movHor = Input.GetAxisRaw("Horizontal");

        isMoving = (movHor != 0f); //para saber si el jugador se esta moviendo

        isGrounded = Physics2D.CircleCast(transform.position, radius, Vector3.down, groundRayDist, groundLayer); // nos devuelve un V o F si esta tocando el piso el jugador, proyecta un circulo hacia abajo

        if (Input.GetKeyDown(KeyCode.UpArrow))
            jump();

        if (isInmune)
        {
            spr.enabled = !spr.enabled;
            inmuneTimeCnt -= Time.deltaTime;

            if(inmuneTimeCnt <= 0)
            {
                isInmune = false;
                spr.enabled = true;
            }
        }

        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);

        flip(movHor);
    }


    private void goInmune()
    {
        isInmune = true;
        inmuneTimeCnt = inmuneTime;
    }

    public void jump()
    {
        if (!isGrounded) return;

        rb.velocity = Vector2.up * jumpforce;
        AudioManager.obj.playJump();
    }

    private void flip(float _xValue) //para girar al personaje en movimiento
    {
        Vector3 theScale = transform.localScale;

        if (_xValue < 0)
            theScale.x = Mathf.Abs(theScale.x) * -1;
        else
        if (_xValue > 0)
            theScale.x = Mathf.Abs(theScale.x);

        transform.localScale = theScale;
    }


    //Todo lo que tiene que ver en fisicas para los personajes se programa aqui
    void FixedUpdate()
    {
        rb.velocity = new Vector2(movHor * speed, rb.velocity.y); //permite mover al personaje mediante su rb se pone dos vectores el x es con velocidad que cambia and y
        
    }

    public void getDamage()
    {
        lives--;
        AudioManager.obj.playHit();
        UIManager.obj.updateLives();

        goInmune();

        if (lives <= 0)
        {
            FXManager.obj.showPop(transform.position);
            Game.obj.GameOver();
        }

    }

    public void addLive()
    {
        lives++;

        if (lives > Game.obj.maxLives)
            lives = Game.obj.maxLives;
    }


    //para el single tone
    void OnDestroy()
    {
        obj = null;
        
    }
}
