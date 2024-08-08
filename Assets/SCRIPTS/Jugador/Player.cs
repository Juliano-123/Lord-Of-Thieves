using UnityEngine;
using System.Collections;
using System.ComponentModel.Design;
using Unity.Burst.CompilerServices;
using UnityEngine.InputSystem;
using TMPro.EditorUtilities;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    //debug
    bool _logRotation = false;


    //components
    GameObject _imagen;
    Animator _animator;
    SpriteRenderer _spriteRenderer;
    GameObject _rotatePoint;
    Apuntar _rotatePointApuntar;
    //GameObject _slicePoint;
    //Animator _slicePointAnimator;
    //SpriteRenderer _slicePointSpriteRenderer;
    Controller2D _controller;
    BoxCollider2D _collider;
    

    //Vector2 _originalColliderSize;
    //Vector2 _originalColliderOffset;

    public GameOverScreen gameOverScreen;
    public AudioSource audioJugador;
    public AudioSource audioGemas;
    public AudioClip salto, dashlisto, dasheando, agarrogema;


    //para golpeo

    public bool _jugadorGolpeado = false;
    public float timerGolpeadoIzquierda = 1f;
    public float timerGolpeadoDerecha = 1f;
    float timerGolpeadoArriba = 1f;
    float timerGolpeadoAbajo = 1f;


    [SerializeField]
    float _maxJumpHeight = 1.8F;
    [SerializeField]
    float _minJumpHeight = 0.5F;
    [SerializeField]
    float timeToJumpApex = 0.4f;
    float accelerationTimeAirborne = 0.1f;
    float aceleracionPuntoMasAlto = 0.075f;
    float accelerationTimeGrounded = 0.005f;
    float moveSpeed = 7f;
    int orientacionX = 1;
    float velocidadtTiempoExtraAire = 0.5f;
    float multiplicadorGravedadCaida = 1.5f;
    float multiplicadorGravedadPuntoMasAlto = 0.5f;


    public static float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    [SerializeField]
    Vector2 velocity;
    float velocityXSmoothing;
    
    //COYOTE
    float tiempoCoyote = -1f;
    bool tiempoCoyoteON = false;
    


    int _jumpApretado;
    float tiempoJump1 = -1;
    bool _jumpSoltado = false;
    int _saltosTotales = 0;
    bool _isJumping = false;
    

    int boxContados = 0;
    int gemasContadas = 0;

    //DASH
    bool _dasheando = false;
    int _dashApretado = 0;
    bool _dashSoltado = false;
    Vector2 _dashvelocitydirection;

    bool _dashCambioRotacionImagen = false;
    float timerdash = 1f;
    float dashVelocity = 15f;


    //int _shootApretado = 0;
    //int _shootSoltado = 0;
    //[SerializeField]
    //GameObject _spawnObjetoDaga;
    //[SerializeField]
    GameObject _mira;
    Vector2 _lugarSpawn;
    //float _shootTime = 1f;
    //float _shootTimer = 0.55f;





    float flashTime = 0.008f;
    public Ghost ghost;

    //nuevo movimiento
    [SerializeField]
    Vector2 _directionalInput;

    //timer para poner idle (soluciona volteo sprite)
    float _directionalInputX0Timer;
    float _directionalInputX0Time = 0.06f;


    Vector2 _attackDirection;

    ////Wallrunning
    //bool _isWallTouching = false;
    //[SerializeField]
    //bool _isWallRunning = false;
    
    //float wallClimbDistanceX = 30f;
    
    //float wallLeapDistanceX = 50f;
    //float _wallStickTimer = 0f;
    //float _wallStickTime = 0.25f;
    //float _wallStickDirection = 0f;
    //bool _wallCambioRotacionImagen = false;


    //bool _idleAttack;


    private void Awake()
    {
        gemasContadas = 0;
        _controller = GetComponent<Controller2D>();
        _imagen = transform.Find("Imagen").gameObject;
        _spriteRenderer = _imagen.GetComponent<SpriteRenderer>();
        _animator = _imagen.GetComponent<Animator>();
        _rotatePoint = transform.Find("RotatePoint").gameObject;
        _rotatePointApuntar = _rotatePoint.GetComponent<Apuntar>();
        //_slicePoint = _rotatePoint.transform.Find("SlicePoint").gameObject;
        //_slicePointSpriteRenderer = _slicePoint.GetComponent<SpriteRenderer>();
        //_slicePointAnimator = _slicePoint.GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
    }



    void Start()
    {


        //CACULA GRAVEDAD EN BASE A LOS VALORES DE ALTURA MAXIMA Y TIEMPO PARA LLEGAR A ELLA
        gravity = -(2 * _maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        //CALCULA VELOCIDAD DEL SALTO COMO GRAVEDAD * TIEMPO PARA LLEGAR ARRIBA
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        //CALCULA LA VELOCIDAD MINIMA EN BASE A GRAVEDAD Y SALTO MINIMO
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * _minJumpHeight);

        //_originalColliderSize = _collider.size;
        //_originalColliderOffset = _collider.offset;



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


            ////MANEJAR EL WALLRUNNING
            //HandleWallRunning();

            //VOLTEA EL SPRITE SEGUN DONDE VOY
            CambiarDireccionSprite();


            //CheckeaColisiones
            HandleCollisions();

            AnimarElPJ();





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
                timerdash = 0;
                _dashApretado = 0;
                _jumpApretado = 0;
                _isJumping = false;
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
                    
                    float rotZ = Mathf.Atan2(_dashvelocitydirection.y, _dashvelocitydirection.x) * Mathf.Rad2Deg;
                    Debug.Log("rotZ es" + rotZ);
                    Debug.Log("Image rotation antes " + _imagen.transform.rotation);
                    _imagen.transform.rotation = Quaternion.Euler(0, 0, rotZ);
                    Debug.Log("Image rotation despues " +  _imagen.transform.rotation);
                    _logRotation = true;
                    _dashCambioRotacionImagen = true;
                    _dasheando = true;
                }

                if (_jumpApretado > 0)
                {
                    _dasheando = false;
                    _jumpApretado = 0;
                    _jumpSoltado = false;
                    _isJumping = true;
                    velocity.x = 0;
                    velocity.y = maxJumpVelocity;
                    _saltosTotales = 2;
                    timerdash = 1f;
                }
                
                
            }
            else
            {

                _dasheando = false;
            }

            if (_dashCambioRotacionImagen == true && _dasheando == false)
            {
                _imagen.transform.rotation = Quaternion.identity;
                _dashCambioRotacionImagen = false;
                Debug.Log("dash reseteo rotation");
            }


            

            //SALTO
            //SI APRETE JUMP, ESTOY TOCANDO PISO o RECIEN LO TOQUE, Y ESTOY DENTRO DEL TIEMPO BUFFER
            if (_jumpApretado > 0 && ((_controller.collisions.below && _controller.collisions.objetoGolpeado.tag == "Piso") || tiempoCoyoteON) && Time.time - tiempoJump1 < 0.15 && _saltosTotales == 0)
            {
                audioJugador.clip = salto;
                audioJugador.Play();
                _saltosTotales += 1;
                velocity.y = maxJumpVelocity;
                _jumpApretado = 0;
                _isJumping = true;
                _jumpSoltado = false;
            }

            //DOBLE SALTO
            //SI APRETE JUMP, NO ESTOY TOCANDO SUELO, NO SALTE DOS VECES
            //if (_jumpApretado > 0 && !_controller.collisions.below && !tiempoCoyoteON && _saltosTotales == 1)
            //{
            //    audioJugador.clip = salto;
            //    audioJugador.Play();

            //    velocity.y = maxJumpVelocity;


            //    _saltosTotales += 1;
            //    _jumpApretado = 0;
            //    _jumpSoltado = false;
            //}



                                 
            //si suelto salto me baja la velocidad
            if (_jumpSoltado == true && _isJumping == true)
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





            //LLAMA A LA FUNCION MOVE, PARA QUE SE MUEVA DETECTANDO COLISION
            //CHECKEA PRIMERO OVERRIDES
            if (_dasheando == true)
            {
                velocity = _dashvelocitydirection;
            }
           
            Vector2 MoveAmount = _controller.Move(velocity * Time.deltaTime, tiempoCoyoteON);

            if (_logRotation == true)
            {

                Debug.Log("Image rotation al final del script " + _imagen.transform.rotation);
                _logRotation = false;

            }

        }
    }

    public void SetDirectionalInput(Vector2 input)
    {
        _directionalInput = input;
    }

    public void SetAttackDirection (Vector2 direction)
    {
        _attackDirection = direction;
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
        //if (_shootTime >= _shootTimer)
        //{
        //    _shootApretado = input;
        //}
    }

    public void SetShootSoltado(int input)
    {
        //_shootSoltado = input;
    }

       
    void AnimarElPJ()
    {
        //seteo de animaciones

        _animator.SetBool("Idle", false);
        _animator.SetBool("Corriendo", false);

        //timer para idle
        if (_directionalInput.x == 0)
        {
            _directionalInputX0Timer += Time.deltaTime;

        }
        else
        {
            _directionalInputX0Timer = 0;
        }
        //seteo de idle segun timer
        if (_directionalInputX0Timer >= _directionalInputX0Time && _controller.collisions.below == true)
        {
            _animator.SetBool("Idle", true);
        }
        

        //corriendo en piso
        if (_directionalInput.x != 0 && _controller.collisions.below == true)
        {
            _animator.SetBool("Corriendo", true);
        }
                
        
        //subiendo y cayendo
        if (velocity.y > 0)
        {
            _animator.SetBool("Subiendo", true);
            _animator.SetBool("Cayendo", false);

        }

        if (velocity.y < 0 && _controller.collisions.below == false)
        {
            _animator.SetBool("Cayendo", true);
            _animator.SetBool("Subiendo", false);
        }

        if (_controller.collisions.below == true)
        {
            _animator.SetBool("Subiendo", false);
            _animator.SetBool("Cayendo", false);
        }

        if (_dasheando == true)
        {
            _animator.SetBool("Dasheando", true);
            _animator.SetBool("Subiendo", false);
            _animator.SetBool("Cayendo", false);
        }
        else if (_dasheando == false)
        {
            _animator.SetBool("Dasheando", false);

        }

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
            if (_controller.collisions.objetoGolpeado != null)
            {
                switch (_controller.collisions.objetoGolpeado.tag)
                        {
                            //SI TOCO GEMA la destruyo, la sumo y mando a 0 el timer de lentitud
                            case "GEMA":
                                Destroy(_controller.collisions.objetoGolpeado);
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
                                        _isJumping = false;


                                    }
                                    break;
                                }

                            //SI SALTO SOBRE CAJA ACTIVO SU SCRIPT Y REBOTO
                            case "CUBO":
                                if (_controller.collisions.below)
                                {
                                    _controller.collisions.objetoGolpeado.transform.GetComponent<EnemigoGolpeado>().enabled = true;
                                    _saltosTotales = 1;
                                    _jumpApretado = 0;
                                    _jumpSoltado = false;
                                    _isJumping = false;
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


