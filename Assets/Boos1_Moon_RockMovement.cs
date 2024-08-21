using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boos1_Moon_RockMovement : MonoBehaviour, IGolpeable
{
    Rigidbody2D _rigidbody;

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
    Vector3 _randomTarget;
    //TIMER DE CADA CUANTO RESETEA LA POSICION
    float _timer = 0.2f;
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
    Vector3 _firstPosition;
    [SerializeField]
    float _rotateSpeed = 150f;

    [SerializeField]
    float _returnSpeed = 150f;

    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _firstPosition = transform.position;
        _randomTarget = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (_isRotating == true)
        {
            MoveInRotation();
        }


        _timer -= Time.deltaTime;

        if (_timer <= 0 && _isRotating == false)
        {
            _isCheckingBounds = false;
            CalculateRandomPosition();
            
        }

        if (transform.position.x > _minPosX && transform.position.x < _maxPosX && transform.position.y > _minPosY && transform.position.y < _maxPosY)
        {
            _isCheckingBounds = true;
        }

        CheckBounds();

    }

    private void FixedUpdate()
    {
        if (_isMovingRandom == true)
        {
            MoveToTarget(_direction);
        }

        if (_isHit == true)
        {
            MoveIsHit();
        }

    }

    void MoveToTarget(Vector2 Direction)
    {
        Vector2 desiredVelocity = Direction * _randomSpeed;
        Vector2 deltaVelocity = desiredVelocity - _rigidbody.velocity;
        _moveForce = deltaVelocity * (_force * _forcePower * Time.fixedDeltaTime);
        _moveForce.x = Mathf.SmoothDamp(_moveForce.x, desiredVelocity.x, ref velocitySmoothing, accelerationTime);
        _moveForce.y = Mathf.SmoothDamp(_moveForce.y, desiredVelocity.y, ref velocitySmoothing, accelerationTime);
        _rigidbody.AddForce(_moveForce);
    }

    void CalculateRandomPosition()
    {
        _timer = Random.Range(_minTravelTime, _maxTravelTime);
        //LIMITES DEL CUADRADO - X 0 Y 10 X 15 Y 0
        float xpos = Random.Range(_minPosX, _maxPosX);
        float ypos = Random.Range(_minPosY, _maxPosY);

        _randomTarget = new Vector3(xpos, ypos, 0);

        //if (_randomTarget.magnitude - transform.position.magnitude < _distanciaMinima)
        //{
        //    //porcentaje de distancia minima
        //    _randomTarget = _randomTarget * _distanciaMinima;
        //}

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

    public void Golpear()
    {
        _isHit = true;
        _isRotating = false;
        _isMovingRandom = false;
        Invoke(nameof(ResetMoveToRandom), 0.2f);
    }
}





