using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    [SerializeField]
    Vector2 _direction;

    Vector3 _target;

    const float _forcePower = 10f;

    [SerializeField]
    float _minTravelTime = 0.3f;
    [SerializeField]
    float _maxTravelTime = 1f;

    [SerializeField]
    float _distanciaMinima;

    [SerializeField]
    float _speed = 5f;
    [SerializeField]
    float _force = 140f;

    [SerializeField]
    float _speedHit = 30;

    float velocitySmoothing;

    [SerializeField]
    float accelerationTime = 0.005f;

    Vector2 _moveForce;

    Rigidbody2D _rigidbody;

    float _timer = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;



        if (_timer < 0)
        {
            _timer = Random.Range(_minTravelTime, _maxTravelTime);
            //LIMITES DEL CUADRADO - X 0 Y 10 X 15 Y 0
            float xpos = Random.Range(0, 15);
            float ypos = Random.Range(0, 10);

            _target = new Vector3(xpos, ypos, 0);

            _direction = (_target - transform.position).normalized;
            Debug.Log(_direction + " Se calculo");
        }

        if (_target.sqrMagnitude - transform.position.sqrMagnitude < _distanciaMinima)
        {
            //porcentaje de distancia minima
            _target = _target * _distanciaMinima * 100 / (_target.sqrMagnitude - transform.position.sqrMagnitude);
        }


        if (transform.position.x < 0 || transform.position.x > 15 || transform.position.y < 0 || transform.position.x > 10)
        {
            _timer = 0;
        }

    }

    private void FixedUpdate()
    {
        MoveToTarget(_direction);
    }

    void MoveToTarget(Vector2 Direction)
    {
        Vector2 desiredVelocity = Direction * _speed;
        Vector2 deltaVelocity = desiredVelocity - _rigidbody.velocity;
        _moveForce = deltaVelocity * (_force * _forcePower * Time.fixedDeltaTime);
        _moveForce.x = Mathf.SmoothDamp(_moveForce.x, desiredVelocity.x, ref velocitySmoothing, accelerationTime);
        _moveForce.y = Mathf.SmoothDamp(_moveForce.y, desiredVelocity.y, ref velocitySmoothing, accelerationTime);
        _rigidbody.AddForce(_moveForce);
    }

}
