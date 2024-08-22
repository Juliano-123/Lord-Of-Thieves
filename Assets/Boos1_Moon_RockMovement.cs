using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boos1_Moon_RockMovement : MonoBehaviour, IGolpeable, IReseteable, IExplotable
{

    //COMPONENTES
    Rigidbody2D _rigidbody;
    SpriteRenderer _spriteRenderer;
    BoxCollider2D _boxCollider2D;

    [SerializeField]
    AudioSource _audioPiedra;
    [SerializeField]
    AudioClip _enemigoStompeado;

    //COMPONENTES CHILD
    GameObject _destroyParticlesObject;
    ParticleSystem _destroyParticlesPS;

    [SerializeField]
    bool _isRotating = true;
    [SerializeField]
    bool _isMovingRandom = false;
    [SerializeField]
    bool _isHit = false;


    //DIRECCION QUE SE CALCULA SEGUN LOS CASOS RANDOM U ORBIT
    Vector2 _direction;


    //LA LUNA SOBRE LA QUE GIRA
    [SerializeField]
    GameObject _moon;


    //VARIABLES RANDOM MOVEMENT
    //LUGAR RANDOM QUE SE CALCULA
    [SerializeField]    
    Vector3 _randomTarget;
    //TIMER DE CADA CUANTO RESETEA LA POSICION
    [SerializeField]
    float _timer = 3f;
    //POSICIONES MINIMAS Y MAXIMAS X E Y
    [SerializeField]
    float _minPosX = 0f;
    [SerializeField]
    float _maxPosX = 15f;
    [SerializeField]
    float _minPosY = 0f;
    [SerializeField]
    float _maxPosY = 10f;
    //RANGO DE TIEMPO QUE SE MUEVE
    [SerializeField]
    float _minTravelTime = 0.3f;
    [SerializeField]
    float _maxTravelTime = 1f;
    //DISTANCIA MINIMA A LA QUE SETEA LA TARGET DEL MOVIMIENTO
    [SerializeField]
    float _distanciaMinima;
    //VELOCIDAD EN RANDOM
    [SerializeField]
    float _randomSpeed = 5f;
    bool _isCheckingBounds = false;
    //FIN VARIABLES RANDOM



    //VARIABLES DE MOMENTUM
    [SerializeField]
    float _force = 140f;
    const float _forcePower = 10f;
    Vector2 _moveForce;

    //VARIABLES DE ACELERACION
    //DONDE SE CALCULA SMOOTHEO DE VELOCIDAD
    float velocitySmoothing;
    //RATIO DE ACELERACION
    [SerializeField]
    float accelerationTime = 0.005f;


    //VELOCIDAD CUANDO LE PEGAN
    [SerializeField]
    float _speedHit = 30;

    //PRIMERA POSICION GUARDADA, PARA VOLVER
    [SerializeField]
    Vector3 _firstPosition;
    [SerializeField]
    float _rotateSpeed = 150f;

    [SerializeField]
    float _returnSpeed = 150f;

    // Start is called before the first frame update
    void Awake()
    {
        //COMPONENTES PROPIOS
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();

        //COMPONENTES PROPIOS CHILD
        _destroyParticlesObject = transform.Find("DestroyParticles").gameObject;
        _destroyParticlesPS = _destroyParticlesObject.GetComponent<ParticleSystem>();

        _firstPosition = transform.position;
        _randomTarget = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;


        if (_timer >= 0 && _isRotating == true)
        {
            MoveInRotation();
            //Debug.Log("LLAMO MOVE IN ROTATION");
        }
        else if (_timer <= 0 && _isRotating == true)
        {
            _isRotating = false;
            _isMovingRandom = true;
        }



        if (_timer <= 0 && _isRotating == false)
        {
            _isCheckingBounds = false;
            CalculateRandomPosition();
            
        }


        if (_isCheckingBounds)
        {
            CheckBounds();

        }

    }

    private void FixedUpdate()
    {
        if (_isMovingRandom == true)
        {
            MoveToRandomTarget(_direction);
            Debug.Log("LLAMO MOVE IN RANDOM");
        }

        if (_isHit == true)
        {
            if (_isMovingRandom == true)
            {
                _rigidbody.velocity = Vector2.zero;
                _isMovingRandom = false;
            }
            MoveIsHit();
        }

    }

    void MoveToRandomTarget(Vector2 Direction)
    {
        Vector2 desiredVelocity = Direction * _randomSpeed;
        Vector2 deltaVelocity = desiredVelocity - _rigidbody.velocity;
        _moveForce = deltaVelocity * (_force * _forcePower * Time.fixedDeltaTime);
        _moveForce.x = Mathf.SmoothDamp(_moveForce.x, desiredVelocity.x, ref velocitySmoothing, accelerationTime);
        _moveForce.y = Mathf.SmoothDamp(_moveForce.y, desiredVelocity.y, ref velocitySmoothing, accelerationTime);
        _rigidbody.AddForce(_moveForce);
        if (transform.position.x > _minPosX && transform.position.x < _maxPosX && transform.position.y > _minPosY && transform.position.y < _maxPosY)
        {
            _isCheckingBounds = true;
        }

    }

    void CalculateRandomPosition()
    {
        _timer = Random.Range(_minTravelTime, _maxTravelTime);
        //LIMITES DEL CUADRADO - X 0 Y 10 X 15 Y 0
        float xpos = Random.Range(_minPosX, _maxPosX);
        float ypos = Random.Range(_minPosY, _maxPosY);

        _randomTarget = new Vector3(xpos, ypos, 0);

        if( _isCheckingBounds )
        {
            if (_randomTarget.magnitude - transform.position.magnitude < _distanciaMinima)
            {
                //porcentaje de distancia minima
                _randomTarget = _randomTarget * _distanciaMinima;
            }

        }


        _direction = (_randomTarget - transform.position).normalized;

    }

    void CheckBounds()
    {
        if (transform.position.x < _minPosX || transform.position.x > _maxPosX || transform.position.y < _minPosY || transform.position.y > _maxPosY)
        {
            _timer = 0;
        }

    }


    void MoveInRotation()
    {
        transform.RotateAround(_moon.transform.position, new Vector3(0,0,1), _rotateSpeed * Time.deltaTime);
    }

    void MoveIsHit()
    {
        _moveForce = Vector2.down * _speedHit;
        _rigidbody.AddForce(_moveForce);
    }


    void ResetMoveToRandom()
    {
        _isMovingRandom = true;
        _isHit = false;
        _isRotating = false;
    }

    IEnumerator ResetPositionAndFadeIn()
    {
        //ESPERA ANTES DE HACER NADA, PA QUE EXPLOTE
        yield return new WaitForSeconds(1f);

        transform.position = _firstPosition;
        Color Transparente = _spriteRenderer.color;
        Transparente.a = 0;
        _spriteRenderer.color = Transparente;
        _boxCollider2D.enabled = true;
        _rigidbody.velocity = Vector2.zero;

        //ESPERA DE VUELTA PARA NO RESETEAR EL COLOR Y POSICION MIL VECES
        yield return new WaitForSeconds(0.001f);
        _spriteRenderer.enabled = true;

        float alphaVal = _spriteRenderer.color.a;
        Color tmp = _spriteRenderer.color;

        _boxCollider2D.isTrigger = true;

        while (_spriteRenderer.color.a < 1)
        {
            alphaVal += 0.01f;
            tmp.a = alphaVal;
            _spriteRenderer.color = tmp;

            yield return new WaitForSeconds(0.005f);
        }


        if (_spriteRenderer.color.a >= 1)
        {
            _boxCollider2D.isTrigger = false;
            _timer = 2;
            _isRotating = true;
            yield break;
        }

    }

    public void Golpear()
    {
        _isHit = true;
        _isMovingRandom = false;
        Invoke(nameof(ResetMoveToRandom), 0.2f);
        _audioPiedra.clip = _enemigoStompeado; _audioPiedra.Play();
    }

    public void Resetear()
    {
        _isMovingRandom = false;
        _isHit = false;
        _isRotating = false;
        _boxCollider2D.enabled = false;
        _rigidbody.velocity = Vector2.zero;
        _isCheckingBounds = false;

        Explotar();
        StartCoroutine(ResetPositionAndFadeIn());
    }

    public void Explotar()
    {
        _spriteRenderer.enabled = false;
        Collider2D[] inExplosionRadius = null;
        float explosionRadius = 5f;
        float explosionForceMulti = 400f;

        inExplosionRadius = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D colliderDetectado in inExplosionRadius)
        {
            Rigidbody2D rigidbody2DDetectado = colliderDetectado.GetComponent<Rigidbody2D>();

            if (rigidbody2DDetectado != null)
            {
                Vector2 distancia = colliderDetectado.transform.position - transform.position;

                if (distancia.sqrMagnitude > 0)
                {
                    float explosionForce = explosionForceMulti;
                    rigidbody2DDetectado.AddForce(distancia.normalized * explosionForce);
                    Debug.Log("SE APLICO FUERZA");
                }
            }
        }

        _destroyParticlesPS.Play();

    }
}





