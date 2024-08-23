using UnityEngine;
using System.Collections;
using System.ComponentModel.Design;
using Unity.Burst.CompilerServices;
using UnityEngine.InputSystem;
using Cinemachine;
using Unity.VisualScripting;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    //components PROPIOS
    BoxCollider2D _boxCollider;
    GameObject _imagen;
    Animator _animator;
    SpriteRenderer _spriteRenderer;
    GameObject _rotatePoint;
    GameObject _mira;
    Apuntar _rotatePointApuntar;
    CinemachineImpulseSource _impulseSource;



    //controlador movimiento y su layer
    Controller2D _controller;
    [SerializeField]
    LayerMask collisionPiso;

    //DETECTOR COLISIONES OBJETOS
    DetectorColisiones _detectorColisiones;


    //Sistemas particulas
    GameObject _dustTrailObject;
    ParticleSystem _dustTrailPS;
    GameObject _jumpParticlesObject;
    ParticleSystem _jumpParticlesPS;




    //AUDIO SOURCE Y CLIPS
    [SerializeField]
    AudioSource audioJugador;
    [SerializeField]
    AudioClip salto, dashlisto, dasheando;

    //COMPONENTES AJENOS
    [SerializeField]
    HealthManager _healthManager;

    GameObject _ultimoObjetoDestruidoVertical = null;
    GameObject _ultimoObjetoDestruidoHorizontal = null;


    //Variables para golpeo
    bool _jugadorGolpeado = false;
    bool GolpeadoIzquierda = false;
    bool GolpeadoDerecha = false;
    bool GolpeadoArriba = false;
    bool GolpeadoAbajo = false;
    float _tiempoGolpeadoPiso = 0.8f;
    float _timerJugadorGolpeado = 1f;


    [SerializeField]
    float _maxJumpHeight = 1.8F;
    [SerializeField]
    float _minJumpHeight = 0.5F;
    [SerializeField]
    float timeToJumpApex = 0.4f;
    float aceleracionPuntoMasAlto = 0.075f;
    float accelerationTimeGrounded = 0.005f;
    float moveSpeed = 7f;
    float velocidadtTiempoExtraAire = 0.5f;
    float multiplicadorGravedadCaida = 1.5f;
    float multiplicadorGravedadPuntoMasAlto = 0.5f;

    //VARIABLE DE ORIENTACION PARA EL SPRITE
    int orientacionX = 1;

    static float gravity;
    float _maxFallVelocity = -10f;
    float maxJumpVelocity;
    float minJumpVelocity;

    float _reboteVelocity;

    Vector2 velocity;
    
    //PARA EL SMOOHTING DE ACELERACION
    float velocityXSmoothing;
    
    //COYOTE
    float tiempoCoyote = -1f;
    bool tiempoCoyoteON = false;
    

    //VARIABLES DE SALTO
    int _jumpApretado;
    float tiempoJump1 = -1;
    bool _jumpSoltado = false;
    int _saltosRealizados = 0;
    int _saltosMaximos = 1;
    bool _isJumping = false;
  
    //DASH
    int _dashTotales = 0;
    int _dashMaximos = 1;
    bool _dasheando = false;
    int _dashApretado = 0;
    bool _dashSoltado = false;
    Vector2 _dashvelocitydirection;
    float timerdash = 1f;
    float dashVelocity = 15f;

    //LO QUE TARDA EN CAMBIAR EL SPRITE FLASH DE CUANDO ME PEGAN
    float flashTime = 0.008f;
    [SerializeField]
    Ghost ghost;

    //nuevo movimiento
    [SerializeField]
    Vector2 _directionalInput;

    //timer para poner idle (soluciona volteo sprite)
    float _directionalInputX0Timer;
    float _directionalInputX0Time = 0.06f;


    Vector2 _attackDirection;

    float _shakeForce = 1f;




    private void Awake()
    {
        

        //componentes PROPIOS
        _controller = GetComponent<Controller2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _detectorColisiones = GetComponent<DetectorColisiones>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();

        //componentes PROPIOS HIJOS
        _imagen = transform.Find("Imagen").gameObject;
        _spriteRenderer = _imagen.GetComponent<SpriteRenderer>();
        _animator = _imagen.GetComponent<Animator>();
        _rotatePoint = transform.Find("RotatePoint").gameObject;
        _rotatePointApuntar = _rotatePoint.GetComponent<Apuntar>();
        _mira = _rotatePoint.transform.Find("SlicePoint").gameObject;
        _dustTrailObject = transform.Find("DustTrail").gameObject;
        _dustTrailPS = _dustTrailObject.GetComponent<ParticleSystem>();
        _jumpParticlesObject = transform.Find("JumpParticles").gameObject;
        _jumpParticlesPS = _jumpParticlesObject.GetComponent<ParticleSystem>();





        //CACULA GRAVEDAD EN BASE A LOS VALORES DE ALTURA MAXIMA Y TIEMPO PARA LLEGAR A ELLA
        gravity = -(2 * _maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        //CALCULA VELOCIDAD DEL SALTO COMO GRAVEDAD * TIEMPO PARA LLEGAR ARRIBA
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        //SETEA REBOTE VELOCITY COMO VELOCIDAD SALTO DIVIDIDO 4
        _reboteVelocity = maxJumpVelocity / 1.7f;
        //CALCULA LA VELOCIDAD MINIMA EN BASE A GRAVEDAD Y SALTO MINIMO
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * _minJumpHeight);

    }


    void Update()
    {
        // AGREGA GRAVEDAD
        velocity.y += gravity * Time.deltaTime;

        //SETEA TARGET VELOCITY COMO EL MOVESPEED TOTAL CON SINGO POSITIVO/NEGATIVO
        float targetVelocityX = (_directionalInput.x * moveSpeed);
        //Hace que uno vaya acelerando y que no sea 0 100
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTimeGrounded);

        //VOLTEA EL SPRITE SEGUN DONDE VOY
        CambiarDireccionSprite();


        _timerJugadorGolpeado += Time.deltaTime;
        //SI ME GOLPEARON
        if (_jugadorGolpeado == true)
        {
            FlashRed();
        }




        //CheckeaColisiones
        HandleCollisions();

        //SETEA ANIMACIONES
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
        if (_dashApretado > 0 && _dashTotales < _dashMaximos)
        {
            timerdash = 0;
            _dashTotales += 1;
            _dashApretado = 0;
            _jumpApretado = 0;
            _isJumping = false;
            _saltosRealizados = 1;
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

                if (_jumpApretado > 0)
                {
                    _dasheando = false;
                    _jumpApretado = 0;
                    _jumpSoltado = false;
                    _isJumping = true;
                    velocity.x = 0;
                    velocity.y = maxJumpVelocity;
                    _saltosRealizados = 2;
                    timerdash = 1f;
                }
                
                
            }
            else
            {

                _dasheando = false;
            }


            

            //SALTO
            //SI APRETE JUMP, ESTOY TOCANDO PISO o RECIEN LO TOQUE, Y ESTOY DENTRO DEL TIEMPO BUFFER
            if (_jumpApretado > 0 && ((_controller.collisions.below && (_controller.collisions.objetoGolpeadoVertical.tag == "Piso" ||
                _controller.collisions.objetoGolpeadoVertical.tag == "Plataforma")) ||
                tiempoCoyoteON) && Time.time - tiempoJump1 < 0.15 && _saltosRealizados == 0)
            {
                ResetJugadorGolpeado();
                Saltar();
                _jumpParticlesPS.Play();
            }

            //DOBLE SALTO
            //SI APRETE JUMP, NO ESTOY TOCANDO SUELO, NI DENTRO DEL BUFFER DE COYOTE Y ME QUEDAN SALTOS
            if (_jumpApretado > 0 && !_controller.collisions.below && !tiempoCoyoteON && _saltosRealizados < _saltosMaximos)
            {
                Saltar();
            }




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

            if (tiempoCoyote > 0 && _saltosRealizados == 0)
            {
                tiempoCoyoteON = true;
            }
            else
            {
                tiempoCoyoteON = false;
            }



            //PONE UN LIMITE MAXIMO A LA VELOCIDAD DE CAIDA
            if (velocity.y <= _maxFallVelocity)
            {
                velocity.y = _maxFallVelocity;
            }





            //LLAMA A LA FUNCION MOVE, PARA QUE SE MUEVA DETECTANDO COLISION
            //CHECKEA PRIMERO OVERRIDE DEL DASH QUE FIJA OTRO MOVIMIENTO
            if (_dasheando == true)
            {
                velocity = _dashvelocitydirection;
            }

            _detectorColisiones.CheckEnemigos();
            _controller.Move(velocity * Time.deltaTime, tiempoCoyoteON, collisionPiso);
            



        
    }


    void Saltar()
    {
        _dustTrailPS.Stop();
        audioJugador.clip = salto;
        audioJugador.Play();
        _saltosRealizados += 1;
        velocity.y = maxJumpVelocity;
        _jumpApretado = 0;
        _isJumping = true;
        _jumpSoltado = false;
    }

    void Rebotar()
    {
        _jumpApretado = 0;
        _saltosRealizados = 0;
        _dashTotales = 0;
        _jumpSoltado = false;
        //falso para que no baje la velocidad por soltar jump
        _isJumping = false;
        velocity.y = _reboteVelocity;
        _impulseSource.GenerateImpulseWithForce(_shakeForce);
    }

    void RecibeGolpe()
    {
        _jugadorGolpeado = true;
        _timerJugadorGolpeado = 0;
        _healthManager.SetCurrentHealth(-1);
        ComboCounter.Instance.ResetComboCount();
        _saltosRealizados = _saltosMaximos;
        _jumpSoltado = false;
        _boxCollider.isTrigger = true;
        velocity.y = _maxFallVelocity;
    }



    void HandleCollisions()
    {
        //MANEJAR CASOS DE COLISION CON PISO
        if (_controller.collisions.hayGolpe)
        {
            if(_controller.collisions.objetoGolpeadoVertical !=null)
            {
                if (_controller.collisions.below)
                {
                    //particulas para cuando toca piso
                    //_dustTrailPS.Play();
                    _saltosRealizados = 0;
                    _dashTotales = 0;
                    _boxCollider.isTrigger = false;
                    ComboCounter.Instance.ResetComboCount();
                    tiempoCoyote = 0.15f;
                    _isJumping = false;
                    
                    if (_timerJugadorGolpeado > _tiempoGolpeadoPiso)
                    {
                        ResetJugadorGolpeado();
                    }
                }

            }
        }


        //manejar casos de colision CON ENEMIGOS Y COSAS


        if (_detectorColisiones.enemigos.hayGolpe)
        {
            if (_detectorColisiones.enemigos.objetoGolpeadoHorizontal != null)
            {
                switch (_detectorColisiones.enemigos.objetoGolpeadoHorizontal.tag)
                {
                    case "Enemigo":
                        if (_jugadorGolpeado == false)
                        {
                            if (_detectorColisiones.enemigos.edge == true)
                            {
                                if (_ultimoObjetoDestruidoHorizontal != _detectorColisiones.enemigos.objetoGolpeadoHorizontal && _ultimoObjetoDestruidoVertical != _detectorColisiones.enemigos.objetoGolpeadoHorizontal)
                                {
                                    _detectorColisiones.enemigos.objetoGolpeadoHorizontal.GetComponent<IGolpeable>().Golpear();

                                    Debug.Log("DESTRUIDO POR DETECTORCOLISIONEs HORIZONAL DISTINTO DE NULL Y TAG ENEMIGO Y EDGE");

                                    Rebotar();

                                    _ultimoObjetoDestruidoHorizontal = _detectorColisiones.enemigos.objetoGolpeadoHorizontal;
                                    Invoke(nameof(ResetUltimosEnemigosGolpeados), 0.1f);
                                }
                            }
                            else
                            {
                                if (_detectorColisiones.enemigos.left)
                                {
                                    GolpeadoIzquierda = true;
                                }
                                else
                                {
                                    GolpeadoDerecha = true;
                                }

                                RecibeGolpe();
                                Debug.Log("Bajo vida por horizontal");
                            }

                        }
                        break;
                }
            }
            else if(_detectorColisiones.enemigos.objetoGolpeadoVertical != null)
            {
                switch (_detectorColisiones.enemigos.objetoGolpeadoVertical.tag)
                {

                    case "Enemigo":
                        if (_jugadorGolpeado == false)
                        {
                            if (_detectorColisiones.enemigos.below)
                            {
                                if (_ultimoObjetoDestruidoVertical != _detectorColisiones.enemigos.objetoGolpeadoVertical && _ultimoObjetoDestruidoHorizontal != _detectorColisiones.enemigos.objetoGolpeadoVertical)
                                {
                                    _detectorColisiones.enemigos.objetoGolpeadoVertical.GetComponent<IGolpeable>().Golpear();

                                    Debug.Log("DESTRUIDO POR DETECTORCOLISIONE VERTICAL DISTINTO DE NULL Y TAG ENEMIGO Y BELOW");

                                    Rebotar();
                                    _ultimoObjetoDestruidoVertical = _detectorColisiones.enemigos.objetoGolpeadoVertical;
                                    Invoke(nameof(ResetUltimosEnemigosGolpeados), 0.1f);
                                }
                            }
                            else
                            {
                                GolpeadoArriba = true;
                                RecibeGolpe();
                                Debug.Log("Bajo vida por Vertical");
                            }
                        }
                        break;
                }
            }
        }
    }


    void ResetUltimosEnemigosGolpeados()
    {
        _ultimoObjetoDestruidoHorizontal = null;
        _ultimoObjetoDestruidoVertical = null;
    }

    void ResetJugadorGolpeado()
    {
        _jugadorGolpeado = false;
        GolpeadoAbajo = false;
        GolpeadoArriba = false;
        GolpeadoDerecha = false;
        GolpeadoIzquierda = false;
        _boxCollider.isTrigger = false;
    }


    void FlashRed()
    {
        _spriteRenderer.color = Color.black;
        Invoke(nameof(ResetColor), flashTime);
    }

    void ResetColor()
    {
        _spriteRenderer.color = Color.white;
    }

    private void OnDestroy()
    {
        

    }



    //seteo de animaciones
    void AnimarElPJ()
    {     
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


    }

    //CAMBIA LA DIRECCION DEL SPRITE DEL PLAYER
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
        if (orientacionX == 1 && _spriteRenderer.flipX == true)
        {
            if (_controller.collisions.below)
            {
                _dustTrailPS.Play();
            }
            _spriteRenderer.flipX = false;
        }
        else if (orientacionX == -1 && _spriteRenderer.flipX == false)
        {
            if (_controller.collisions.below)
            {
                _dustTrailPS.Play();
            }
            _spriteRenderer.flipX = true;
        }
    }



    //ACA ESTAN TODOS LOS INGRESOS DE INPUT QUE TOMA DE INPUTJULIAN
    public void SetDirectionalInput(Vector2 input)
    {
        _directionalInput = input;
    }

    public void SetAttackDirection(Vector2 direction)
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


}


