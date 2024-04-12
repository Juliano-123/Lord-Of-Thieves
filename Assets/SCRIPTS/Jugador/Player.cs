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

    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;
    public float accelerationTimeAirborne = .9f;
    public float aceleracionPuntoMasAlto = .1f;
    public float accelerationTimeGrounded = .1f;
    public float moveSpeed = 8;
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
    float tiempoJump1 = -10;
    float tiempoJump2 = -1;
    public bool jumpSoltado = false;
    public bool yasalte = false;
    public bool yaSaltex2 = false;
    public bool flotando = false;
    float timerdash = 1f;
    public static bool rebotin = false;
    public static int boxContados = 0;
    public static int gemasContadas = 0;
    public static float timeGemasDecay = 5;
    

    int dashApretado = 0;
    public float dashVelocity = 15f;
    bool dashSoltado = false;
    bool yaDashee = false;
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
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        //CALCULA VELOCIDAD DEL SALTO COMO GRAVEDAD * TIEMPO PARA LLEGAR ARRIBA
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        //CALCULA LA VELOCIDAD MINIMA EN BASE A GRAVEDAD Y SALTO MINIMO
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

    }

    void Update()
    {

        float targetVelocityX;

        //TOMA LA DIRECCION DEL MOVIMIENTO
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //VOLETEA EL SPRITE
        if (input.x > 0 )
        {
            playerSpriteRenderer.flipX = false;
            orientacionX = 1;
        }
        else if (input.x < 0 )
        {
            playerSpriteRenderer.flipX = true;
            orientacionX = -1;
        }

        //SETEA TARGET VELOCITY COMO EL MOVESPEED TOTAL CON SINGO POSITIVO/NEGATIVO
        targetVelocityX = (input.x * moveSpeed);
        
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);


        //SALTO TOMAR INPUT
        if (Input.GetButtonDown("Jump"))
        {
            jumpApretado = jumpApretado+1;
            jumpSoltado = false;
            tiempoJump1 = tiempoJump2;
            tiempoJump2 = Time.time;
        }

        //SUELTO SALTO TOMAR INPUT
        if (Input.GetButtonUp("Jump")) {
            jumpSoltado = true;
            flotando = false;
        }

        //DASH TOMAR INPUT
        if (Input.GetButtonDown("Fire3") && yaDashee == false && timeForNextDash <= 0)
        {
            dashApretado = dashApretado + 1;
            dashSoltado = false;
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

        
        //SALTO
        //SI APRETE JUMP, ESTOY TOCANDO PISO, Y ESTOY DENTRO DEL TIEMPO BUFFER
        if (jumpApretado > 0 && (controller.collisions.below || tiempoCoyoteON) && Time.time-tiempoJump2 < 0.15)
        {
            animator.SetBool("Saltando", true);
            audioJugador.clip = salto;
            audioJugador.Play();
            yasalte = true;
            velocity.y = maxJumpVelocity;
            jumpApretado = 0;
        }

        //DOBLE SALTO
        //SI APRETE JUMP, NO ESTOY TOCANDO SUELO, NO SALTE DOS VECES
        if (jumpApretado > 0 && !controller.collisions.below && !tiempoCoyoteON && !yaSaltex2 && yasalte)
        {
            animator.SetBool("Saltando", true);
            audioJugador.clip = salto;
            audioJugador.Play();

            velocity.y = maxJumpVelocity;
            yaSaltex2 = true;
            jumpApretado = 0;
        }

        //DECIDIR QUE DASHEO
        if (dashApretado > 0)
        {
            timerdash = 0;
            dashApretado = 0;
            yaSaltex2 = false;
            yaDashee = true;
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
            yasalte = true;
            controller.Move(velocity * Time.deltaTime, tiempoCoyoteON);
            timeForNextDash = delayForDash;
            return;
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


            //SI SUELTO SALTO ME BAJA LA VELOCIDAD
            if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > minJumpVelocity)
            {
                velocity.y = minJumpVelocity;
            }
        }

        //SI CAIGO BAJA VELOCIDAD
        if (velocity.y <= 0)
        {
            
            velocity.y = velocity.y * multiplicadorGravedadCaida;
            
        }

        // GRAVEDAD
        velocity.y += gravity * Time.deltaTime;

        //Tiempo de coyote
        if (controller.collisions.below)
        {
            tiempoCoyote = 0.15f;
        }

        tiempoCoyote -= Time.deltaTime;

        if (tiempoCoyote > 0 && !yasalte)
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

    private void FixedUpdate()


    {
        //ESTO HAY QUE HACERLO EN FIXED PARA QUE DETECTE ESTAS COLISIONES 1 VEZ POR MOVIMIENTO
        
        
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
                                yasalte = false;
                                yaSaltex2 = false;
                                yaDashee = false;
                                boxContados = 0;

                            }
                            break;
                        }

                    //SI SALTO SOBRE CAJA ACTIVO SU SCRIPT Y REBOTO
                    case "CUBO":
                        if (controller.collisions.below)
                        {
                            controller.collisions.objetoGolpeado.transform.GetComponent<EnemigoGolpeado>().enabled = true;
                            velocity.y = maxJumpVelocity;
                            boxContados++;
                            yasalte = true;
                            yaSaltex2 = false;
                            yaDashee = false;
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


