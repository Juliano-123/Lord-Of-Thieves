using UnityEngine;
using System.Collections;
using System.ComponentModel.Design;
using Unity.Burst.CompilerServices;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    public GameOverScreen gameOverScreen;
    public AudioSource audioJugador;
    public AudioSource audioGemas;
    public AudioClip salto, dashlisto, dasheando, agarrogema;

    public static GameObject elJugador;

    Vector2 _input;

    float _maxJumpHeight = 1.7F;
    float _minJumpHeight = 0.5F;
    float timeToJumpApex = 0.4f;
    float accelerationTimeAirborne = 0.1f;
    float aceleracionPuntoMasAlto = 0.075f;
    float accelerationTimeGrounded = 0.005f;
    float moveSpeed = 4.5f;
    public static int orientacionX = 1;
    public float velocidadtTiempoExtraAire = 0.5f;
    public float multiplicadorGravedadCaida = 1.5f;
    public float multiplicadorGravedadPuntoMasAlto = 0.5f;


    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    Vector2 velocity;
    float velocityXSmoothing;
    public float tiempoCoyote = -1f;
    public bool tiempoCoyoteON = false;
    bool toquePiso = true;

    int jumpApretado;
    float tiempoJump1 = -1;
    public bool jumpSoltado = false;
    public int _saltosTotales = 0;
    float timerdash = 1f;

    public static int boxContados = 0;
    public static int gemasContadas = 0;

    

    int dashApretado = 0;
    public float dashVelocity = 15f;
    float timeForNextDash = 0;
    public float delayForDash = 1f;
    bool yaSonoElDash = true;




    public static float timerGolpeadoIzquierda = 1f;
    public static float timerGolpeadoDerecha = 1f;
    public static float timerGolpeadoArriba = 1f;
    public static float timerGolpeadoAbajo = 1f;


    float flashTime = 0.008f;

    public Animator animator;

    SpriteRenderer playerSpriteRenderer;

    Controller2D controller;

    public Ghost ghost;



    private void Awake()
    {
        gemasContadas = 0;
    }



    void Start()
    {
        controller = GetComponent<Controller2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        elJugador = gameObject;

        //CACULA GRAVEDAD EN BASE A LOS VALORES DE ALTURA MAXIMA Y TIEMPO PARA LLEGAR A ELLA
        gravity = -(2 * _maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        //CALCULA VELOCIDAD DEL SALTO COMO GRAVEDAD * TIEMPO PARA LLEGAR ARRIBA
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        //CALCULA LA VELOCIDAD MINIMA EN BASE A GRAVEDAD Y SALTO MINIMO
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * _minJumpHeight);

    }

    void Update()
    {

        // GRAVEDAD
        velocity.y += gravity * Time.deltaTime;

        //TOMA LA DIRECCION DEL MOVIMIENTO
        _input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //GUARDA LA ORIENTACION SEGUN ESO
        if (_input.x > 0)
        {
            orientacionX = 1;
        }
        else if (_input.x < 0)
        {
            orientacionX = -1;
        }

        CambiarDireccionSprite();

        float targetVelocityX;

        //SETEA TARGET VELOCITY COMO EL MOVESPEED TOTAL CON SINGO POSITIVO/NEGATIVO
        targetVelocityX = (_input.x * moveSpeed);
        
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);


        HandleCollisions();

        //SALTO TOMAR INPUT
        if (Input.GetButtonDown("Jump"))
        {
            jumpApretado = jumpApretado+1;
            jumpSoltado = false;
            tiempoJump1 = Time.time;
        }

        //SUELTO SALTO TOMAR INPUT
        if (Input.GetButtonUp("Jump")) {
            jumpSoltado = true;
        }

        //DASH TOMAR INPUT
        if (Input.GetButtonDown("Fire3") && timeForNextDash <= 0)
        {
            dashApretado = dashApretado + 1;
        }

        timeForNextDash -= Time.deltaTime;

        if (timeForNextDash <= 0) {
            ghost.makeGhost = true;
            if (yaSonoElDash == false) {
                audioJugador.clip = dashlisto;
                audioJugador.Play();
                yaSonoElDash = true;
            }                        
        }
        else if (timerdash >= 0.3f) {
            ghost.makeGhost = false;
        }

        //DECIDIR QUE DASHEO
        if (dashApretado > 0 && timeForNextDash <= 0)
        {
            timerdash = 0;
            dashApretado = 0;
            _saltosTotales = 1;
            audioJugador.clip = dasheando;
            audioJugador.Play();
            yaSonoElDash = false;
        }


        //QUE HAGO SI DASHEO
        if (timerdash <= 0.3f)
        {
            timerdash += Time.deltaTime;
            velocity.y = 0;
            velocity.x = dashVelocity * orientacionX;
            timeForNextDash = delayForDash;
            if (jumpApretado > 0)
            {
                jumpApretado = 0;
                velocity.x = 0;
                velocity.y = maxJumpVelocity;
                _saltosTotales = 2;
                timerdash = 1f;
            }
            controller.Move(velocity * Time.deltaTime, false);
            return;
        }

        //SALTO
        //SI APRETE JUMP, ESTOY TOCANDO PISO o RECIEN LO TOQUE, Y ESTOY DENTRO DEL TIEMPO BUFFER
        if (jumpApretado > 0 && ((controller.collisions.below && controller.collisions.objetoGolpeado.tag == "Piso") || tiempoCoyoteON) && Time.time-tiempoJump1 < 0.15 && _saltosTotales == 0)
        {
            animator.SetBool("Saltando", true);
            audioJugador.clip = salto;
            audioJugador.Play();
            _saltosTotales += 1;
            velocity.y = maxJumpVelocity;
            jumpApretado = 0;
        }

        //DOBLE SALTO
        //SI APRETE JUMP, NO ESTOY TOCANDO SUELO, NO SALTE DOS VECES
        if (jumpApretado > 0 && !controller.collisions.below && !tiempoCoyoteON && _saltosTotales == 1)
        {
            animator.SetBool("Saltando", true);
            audioJugador.clip = salto;
            audioJugador.Play();

            velocity.y = maxJumpVelocity;
            _saltosTotales += 1;
            jumpApretado = 0;
        }



        //GOLPEADO

        if (timerGolpeadoIzquierda <= 0.1f)
        {
            timerGolpeadoIzquierda += Time.deltaTime;
            FlashRed();
            velocity.y = 3f;
            velocity.x = 6f;
            controller.Move(velocity * Time.deltaTime, tiempoCoyoteON);
            return;
        }

        if (timerGolpeadoDerecha <= 0.1f)
        {
            timerGolpeadoDerecha += Time.deltaTime;
            FlashRed();
            velocity.y = 3f;
            velocity.x = -6f;
            controller.Move(velocity * Time.deltaTime, tiempoCoyoteON);
            return;
        }


        //    //SI SUELTO SALTO ME BAJA LA VELOCIDAD
        //    if (jumpSoltado = true)
        //{
        //    if (velocity.y > minJumpVelocity)
        //    {
        //        velocity.y = minJumpVelocity;
        //    }
        //}




        //Tiempo de coyote
        tiempoCoyote -= Time.deltaTime;

        if (tiempoCoyote > 0 && _saltosTotales == 0)
        {
            tiempoCoyoteON = true;
        }
        else
        {
            tiempoCoyoteON = false;
        }


 

        if (velocity.y <= -10)
        {
            velocity.y = -10;
        }

        //LLAMA A LA FUNCION MOVE, PARA QUE SE MUEVA DETECTANDO COLISION
        controller.Move(velocity * Time.deltaTime, tiempoCoyoteON);
    }

    void CambiarDireccionSprite()
    {
        //VOLTEA EL SPRITE
        if (orientacionX == 1)
        {
            playerSpriteRenderer.flipX = false;
        }
        else if (orientacionX == -1)
        {
            playerSpriteRenderer.flipX = true;
        }
    }



    void HandleCollisions()
    {

        //manejar casos de colision
        if (controller.collisions.below || controller.collisions.above || controller.collisions.right || controller.collisions.left)
        {
            if (controller.collisions.objetoGolpeado != null)
            {
                switch (controller.collisions.objetoGolpeado.tag)
                {

                    //SI ESTOY TOCANDO ABAJO y objeto piso MANDA EL CONTADOR A 0, LE DA FALSO AL YA SALTE X 2 y AL YA DASHEE
                    case "Piso":
                        {
                            if (controller.collisions.below)
                            {
                                animator.SetBool("Saltando", false);
                                _saltosTotales = 0;
                                boxContados = 0;
                                //Tiempo de coyote
                                tiempoCoyote = 0.15f;


                            }
                            break;
                        }

                    //SI SALTO SOBRE CAJA ACTIVO SU SCRIPT Y REBOTO
                    case "CUBO":
                        if (controller.collisions.below)
                        {
                            _saltosTotales = 1;
                            controller.collisions.objetoGolpeado.transform.GetComponent<EnemigoGolpeado>().enabled = true;
                            jumpApretado = 0;
                            velocity.y = maxJumpVelocity;
                            boxContados++;
                            audioJugador.clip = salto;
                            audioJugador.Play();
                        }
                        break;

                    //SI TOCO GEMA la destruyo, la sumo y mando a 0 el timer de lentitud
                    case "GEMA":
                        Destroy(controller.collisions.objetoGolpeado);
                        gemasContadas++;
                        audioGemas.clip = agarrogema;
                        audioGemas.Play();
                        break;

                }
            }
        }

    }

    void FlashRed()
    {
        playerSpriteRenderer.color = Color.black;
        Invoke("ResetColor", flashTime);
    }

    void ResetColor()
    {
        playerSpriteRenderer.color = Color.white;
    }

    private void OnDestroy()
    {
        gameOverScreen.Activate();
        Time.timeScale = 0.05f;

    }


}


