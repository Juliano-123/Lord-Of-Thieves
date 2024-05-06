using UnityEngine;
using System.Collections;
using System.ComponentModel.Design;
using Unity.Burst.CompilerServices;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    //components
    GameObject _imagen;
    Animator _animator;
    SpriteRenderer _spriteRenderer;
    Controller2D _controller;

    public GameOverScreen gameOverScreen;
    public AudioSource audioJugador;
    public AudioSource audioGemas;
    public AudioClip salto, dashlisto, dasheando, agarrogema;


    //para golpeo

    public bool _jugadorGolpeado = false;
    public float timerGolpeadoIzquierda = 1f;
    public float timerGolpeadoDerecha = 1f;
    public float timerGolpeadoArriba = 1f;
    public float timerGolpeadoAbajo = 1f;


    
    float _maxJumpHeight = 1.8F;
    float _minJumpHeight = 0.5F;
    float timeToJumpApex = 0.4f;
    float accelerationTimeAirborne = 0.1f;
    float aceleracionPuntoMasAlto = 0.075f;
    float accelerationTimeGrounded = 0.005f;
    float moveSpeed = 7f;
    public static int orientacionX = 1;
    public float velocidadtTiempoExtraAire = 0.5f;
    public float multiplicadorGravedadCaida = 1.5f;
    public float multiplicadorGravedadPuntoMasAlto = 0.5f;


    public static float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    [SerializeField]
    Vector2 velocity;
    float velocityXSmoothing;
    
    //COYOTE
    public float tiempoCoyote = -1f;
    public bool tiempoCoyoteON = false;
    


    int _jumpApretado;
    public float tiempoJump1 = -1;
    public bool _jumpSoltado = false;
    public int _saltosTotales = 0;
    float timerdash = 1f;

    public static int boxContados = 0;
    public static int gemasContadas = 0;

    
    bool _dasheando = false;
    public int _dashApretado = 0;
    bool _dashSoltado = false;
    public float dashVelocity = 15f;
    Vector2 _dashvelocitydirection;


    int _shootApretado = 0;
    int _shootSoltado = 0;
    [SerializeField]
    GameObject _spawnObjetoDaga;
    [SerializeField]
    GameObject _mira;
    Vector2 _lugarSpawn;





    float flashTime = 0.008f;
    public Ghost ghost;

    //nuevo movimiento
    Vector2 _directionalInput;

    //Wallrunning
    [SerializeField]
    bool _isWallTouching = false;


    private void Awake()
    {
        gemasContadas = 0;
        _controller = GetComponent<Controller2D>();
        _imagen = transform.Find("Imagen").gameObject;
        _spriteRenderer = _imagen.GetComponent<SpriteRenderer>();
        _animator = _imagen.GetComponent<Animator>();
    }



    void Start()
    {


        //CACULA GRAVEDAD EN BASE A LOS VALORES DE ALTURA MAXIMA Y TIEMPO PARA LLEGAR A ELLA
        gravity = -(2 * _maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        //CALCULA VELOCIDAD DEL SALTO COMO GRAVEDAD * TIEMPO PARA LLEGAR ARRIBA
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        //CALCULA LA VELOCIDAD MINIMA EN BASE A GRAVEDAD Y SALTO MINIMO
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * _minJumpHeight);

    }



    void Update()
    {
        // AGREGA GRAVEDAD
        velocity.y += gravity * Time.deltaTime;

        //SI ME GOLPEARON NO TOMA INPUT Y HACE ESTO
        if (_jugadorGolpeado == true)
        {
            HandleGolpeo();
        }

        //SI NO ME GOLEPARON TOMA LOS INPUTS Y ACTUA EN CONSECUENCIA
        if (_jugadorGolpeado == false)
        {



            //SETEA TARGET VELOCITY COMO EL MOVESPEED TOTAL CON SINGO POSITIVO/NEGATIVO
            float targetVelocityX = (_directionalInput.x * moveSpeed);
            //Hace que uno vaya acelerando y que no sea 0 100
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (_controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);




            //todo walltouching
            if (_controller.collisions.objetoGolpeadoHorizontal != null)
            {

                if (_controller.collisions.objetoGolpeadoHorizontal.CompareTag("Pared") && !_controller.collisions.below)
                {
                    _isWallTouching = true;

                    if (_directionalInput.x > 0 && _controller.collisions.right)
                    {
                        _imagen.transform.rotation = Quaternion.Euler(0,0,90);
                        velocity.y = moveSpeed;
                        velocity.x = 0;

                    }
                    else if (_directionalInput.x < 0 && _controller.collisions.left)
                    {
                        _imagen.transform.rotation = Quaternion.Euler(0, 0, -90);
                        velocity.y = moveSpeed;
                        velocity.x = 0;
                    }
                    else
                    {
                        _imagen.transform.rotation = Quaternion.identity;

                    }



                }
                else
                {
                    _isWallTouching = false;
                }
            }
            else
            {
                _isWallTouching = false;
            }



            //VOLTEA EL SPRITE SEGUN DONDE VOY
            CambiarDireccionSprite();


            //CheckeaColisiones
            HandleCollisions();





            //GENERACION DE FANTASMAS CUANDO PUEDO DASHEAR
            //timeForNextDash -= Time.deltaTime;
            //if (timeForNextDash <= 0)
            //{
            //    ghost.makeGhost = true;
            //    if (yaSonoElDash == false)
            //    {
            //        audioJugador.clip = dashlisto;
            //        audioJugador.Play();
            //        yaSonoElDash = true;
            //    }
            //}
            //else if (timerdash >= 0.3f)
            //{
            //    ghost.makeGhost = false;
            //}

            //DECIDIR QUE DASHEO
            if (_dashApretado > 0)
            {
                Time.timeScale = 0.05f;
            }

            if(_dashSoltado == true)
            {
                Time.timeScale = 1;
                _dashSoltado = false;
                timerdash = 0;
                _dashApretado = 0;
                _jumpApretado = 0;
                _saltosTotales = 1;
                audioJugador.clip = dasheando;
                audioJugador.Play();
            }


            //QUE HAGO SI DASHEO
            if (timerdash <= 0.3f)
            {
                timerdash += Time.deltaTime;
                if (_dasheando == false)
                {
                    _dashvelocitydirection = new Vector2(_mira.transform.position.x - transform.position.x, _mira.transform.position.y - transform.position.y) * dashVelocity;
                    _dasheando = true;
                }

                velocity = _dashvelocitydirection;
                Debug.Log(velocity);

                if (_jumpApretado > 0)
                {
                    _dasheando = false;
                    _jumpApretado = 0;
                    _jumpSoltado = false;
                    velocity.x = 0;
                    velocity.y = maxJumpVelocity;
                    _saltosTotales = 2;
                    timerdash = 1f;
                }
                _controller.Move(velocity * Time.deltaTime, false);
                
                return;
            }
            else
            {
                _dasheando = false;
            }
            



            //INSTANCIAR PIEDRA
            if (_shootApretado > 0)
            {
                //CALCULAR LUGAR SPAWN
                //OFFSETS
                //X. 0.000357117
                //Y. 0.03107139
                _lugarSpawn.x = transform.position.x + 0.000357117f;
                _lugarSpawn.y = transform.position.y + 0.03107139f;

                Instantiate(_spawnObjetoDaga, _lugarSpawn, _mira.transform.rotation);
                _shootApretado = 0;
                _shootSoltado = 0;

            }
            

            //SALTO
            //SI APRETE JUMP, ESTOY TOCANDO PISO o RECIEN LO TOQUE, Y ESTOY DENTRO DEL TIEMPO BUFFER
            if (_jumpApretado > 0 && ((_controller.collisions.below && _controller.collisions.objetoGolpeadoVertical.tag == "Piso") || tiempoCoyoteON) && Time.time - tiempoJump1 < 0.15 && _saltosTotales == 0)
            {
                audioJugador.clip = salto;
                audioJugador.Play();
                _saltosTotales += 1;
                velocity.y = maxJumpVelocity;
                _jumpApretado = 0;
                _jumpSoltado = false;
            }

            //DOBLE SALTO
            //SI APRETE JUMP, NO ESTOY TOCANDO SUELO, NO SALTE DOS VECES
            if (_jumpApretado > 0 && !_controller.collisions.below && !tiempoCoyoteON && _saltosTotales == 1)
            {
                audioJugador.clip = salto;
                audioJugador.Play();

                velocity.y = maxJumpVelocity;


                _saltosTotales += 1;
                _jumpApretado = 0;
                _jumpSoltado = false;
            }

                                 
            //si suelto salto me baja la velocidad
            if (_jumpSoltado == true)
            {
                if (velocity.y > minJumpVelocity)
                {
                    velocity.y = minJumpVelocity;
                }
            }




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



            //PONE UN LIMITE MAXIMO A LA VELOCIDAD DE CAIDA
            if (velocity.y <= -10)
            {
                velocity.y = -10;
            }

            //seteo de animaciones

            _animator.SetBool("Idle", false);
            _animator.SetBool("Corriendo", false);
            _animator.SetBool("Subiendo", false);
            _animator.SetBool("Cayendo", false);


            if (velocity.x == 0 && _controller.collisions.below == true)
            {
                _animator.SetBool("Idle", true);
            }

            //corriendo
            if (_isWallTouching == false)
            {
                if (velocity.x != 0 && _controller.collisions.below == true)
                {
                    _animator.SetBool("Corriendo", true);
                }
            }
            else if (_isWallTouching == true)
            {
                if (velocity.y > 0)
                {
                    _animator.SetBool("Corriendo", true);
                }
            }


            if (velocity.y > 0 && _isWallTouching == false)
            {
                _animator.SetBool("Subiendo", true);
            }

            if (velocity.y < 0 && _controller.collisions.below == false)
            {
                _animator.SetBool("Cayendo", true);
            }

            //LLAMA A LA FUNCION MOVE, PARA QUE SE MUEVA DETECTANDO COLISION
            _controller.Move(velocity * Time.deltaTime, tiempoCoyoteON);
        }
    }

    public void SetDirectionalInput(Vector2 input)
    {
        _directionalInput = input;
    }

    public void SetJumpApretado(int input)
    {
        _jumpApretado += input;
        tiempoJump1 = Time.time;
    }

    public void SetJumpSoltado()
    {
        _jumpSoltado = true;
    }

    public void SetDashApretado(int input)
    {
            _dashApretado += input;

    }

    public void SetDashSoltado()
    {
        _dashSoltado = true;
    }

    public void SetShootApretado(int input)
    {
        _shootApretado = input;
    }

    public void SetShootSoltado(int input)
    {
        _shootSoltado = input;
    }

    void CambiarDireccionSprite()
    {
        //GUARDA LA ULTIMA ORIENTACION
        if (_directionalInput.x > 0)
        {
            orientacionX = 1;
        }
        else if (_directionalInput.x < 0)
        {
            orientacionX = -1;
        }

        //VOLTEA EL SPRITE
        if (orientacionX == 1)
        {
            _spriteRenderer.flipX = false;
        }
        else if (orientacionX == -1)
        {
            _spriteRenderer.flipX = true;
        }
    }



    void HandleGolpeo()
    {
        //GOLPEADO
        if (timerGolpeadoIzquierda <= 0.3f)
        {
            timerGolpeadoIzquierda += Time.deltaTime;
            FlashRed();
            velocity.y = 3f;
            velocity.x = 6f;
            _controller.Move(velocity * Time.deltaTime, tiempoCoyoteON);
            return;
        }


        if (timerGolpeadoDerecha <= 0.3f)
        {
            timerGolpeadoDerecha += Time.deltaTime;
            FlashRed();
            velocity.y = 3f;
            velocity.x = -6f;
            _controller.Move(velocity * Time.deltaTime, tiempoCoyoteON);
            return;
        }

        if (timerGolpeadoDerecha >= 0.3f && timerGolpeadoIzquierda >= 0.3f)
        {
            _jugadorGolpeado = false;

        }
    }

    void HandleCollisions()
    {

        //manejar casos de colision
        if (_controller.collisions.below || _controller.collisions.above || _controller.collisions.right || _controller.collisions.left)
        {
            if (_controller.collisions.objetoGolpeadoHorizontal != null)
            {
                switch (_controller.collisions.objetoGolpeadoHorizontal.tag)
                {
                    //SI TOCO GEMA la destruyo, la sumo y mando a 0 el timer de lentitud
                    case "GEMA":
                        Destroy(_controller.collisions.objetoGolpeadoVertical);
                        Destroy(_controller.collisions.objetoGolpeadoHorizontal);
                        gemasContadas++;
                        audioGemas.clip = agarrogema;
                        audioGemas.Play();
                        break;

                    case "Enemigo":
                        _jugadorGolpeado = true;
                        _saltosTotales = 1;
                        _jumpApretado = 0;
                        _jumpSoltado = false;
                        if (_controller.collisions.left)
                        {
                            timerGolpeadoIzquierda = 0;
                        }
                        else
                        {
                            timerGolpeadoDerecha = 0;
                        }
                        break;

                }
            }


            if (_controller.collisions.objetoGolpeadoVertical != null)
            {
                switch (_controller.collisions.objetoGolpeadoVertical.tag)
                        {
                            //SI TOCO GEMA la destruyo, la sumo y mando a 0 el timer de lentitud
                            case "GEMA":
                                Destroy(_controller.collisions.objetoGolpeadoVertical);
                                Destroy(_controller.collisions.objetoGolpeadoHorizontal);
                                gemasContadas++;
                                audioGemas.clip = agarrogema;
                                audioGemas.Play();
                                break;

                            case "Enemigo":
                                _jugadorGolpeado = true;
                                _saltosTotales = 1;
                                _jumpApretado = 0;
                                _jumpSoltado = false;
                                if (_controller.collisions.left)
                                {
                                    timerGolpeadoIzquierda = 0;
                                }
                                else
                                {
                                    timerGolpeadoDerecha = 0;
                                }
                                break;



                            //SI ESTOY TOCANDO ABAJO y objeto piso MANDA EL CONTADOR A 0, LE DA FALSO AL YA SALTE X 2 y AL YA DASHEE
                            case "Piso":
                                {
                                    if (_controller.collisions.below)
                                    {
                                        _saltosTotales = 0;
                                        boxContados = 0;
                                        //Tiempo de coyote
                                        tiempoCoyote = 0.15f;


                                    }
                                    break;
                                }

                            //SI SALTO SOBRE CAJA ACTIVO SU SCRIPT Y REBOTO
                            case "CUBO":
                                if (_controller.collisions.below)
                                {
                                    _controller.collisions.objetoGolpeadoVertical.transform.GetComponent<EnemigoGolpeado>().enabled = true;
                                    _saltosTotales = 1;
                                    _jumpApretado = 0;
                                    _jumpSoltado = false;
                                    velocity.y = maxJumpVelocity;
                                    boxContados++;
                                    audioJugador.clip = salto;
                                    audioJugador.Play();
                                }
                                break;

                }
            }
        }

    }

    void FlashRed()
    {
        _spriteRenderer.color = Color.black;
        Invoke("ResetColor", flashTime);
    }

    void ResetColor()
    {
        _spriteRenderer.color = Color.white;
    }

    private void OnDestroy()
    {
        //gameOverScreen.Activate();
        Time.timeScale = 0.05f;

    }


}


