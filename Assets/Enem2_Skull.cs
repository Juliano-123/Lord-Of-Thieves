using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enem2_Skull : MonoBehaviour
{
    [SerializeField]
    bool _isCalculatingAttack = false;
    [SerializeField]
    bool _isAttacking = false;

    [SerializeField]
    GameObject _target;

    SpriteRenderer _spriteRenderer;
    Rigidbody2D _rigidBody2D;


    float velocitySmoothing;

    [SerializeField]
    float accelerationTime = 0.005f;

    [SerializeField]
    float accelerationTimeHit = 0.005f;

    [SerializeField]
    float _attackSpeed = 10f;
    Vector2 desiredVelocity;
    Vector2 deltaVelocity;
    Vector2 _moveForce;

    const float _forcePower = 10f;
    [SerializeField]
    float _force = 15f;

    [SerializeField]
    float _retrocederSpeed = 3f;

    [SerializeField]
    float _speedHit = 30;

    [SerializeField]
    float _timer = 3;

    [SerializeField]
    Vector2 _directionTowardsTarget;
    [SerializeField]
    Vector2 _positionToStrike;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        LookatTarget();
    }

    private void FixedUpdate()
    {
        _timer -= Time.deltaTime;

        if (_isAttacking == false)
        {
            if (_timer < 0)
            {
                _isCalculatingAttack = true;
                Debug.Log("calcula attack");

            }
            else
            {
                _directionTowardsTarget = (transform.position - _target.transform.position).normalized;
                MoveToTarget(_retrocederSpeed);
                Debug.Log("entro retrocede");
            }


        }


        if (_isCalculatingAttack == true && _isAttacking == false)
        {
            _directionTowardsTarget = (_target.transform.position - transform.position).normalized;
            _positionToStrike = _target.transform.position;
            _isAttacking = true;
            _isCalculatingAttack = false;
            _timer = Random.RandomRange(7, 10);
            Debug.Log("calculo attack");
        }

        if (_isAttacking == true )
        {
            MoveToTarget(_attackSpeed);
            Debug.Log("ataca");
            if (transform.position.x < -18f || transform.position.x > 18f || transform.position.y < 1f || transform.position.y > 22f)
            {
                _isAttacking = false;
                _rigidBody2D.velocity = Vector3.zero;
                Debug.Log("seteo false is attacking");
            }

        }


    }


    void MoveToTarget(float Speed)
    {
        desiredVelocity = _directionTowardsTarget * Speed;
        deltaVelocity = desiredVelocity - _rigidBody2D.velocity;
        _moveForce = deltaVelocity * (_force * _forcePower * Time.fixedDeltaTime);
        _moveForce.x = Mathf.SmoothDamp(_moveForce.x, desiredVelocity.x, ref velocitySmoothing, accelerationTime);
        _moveForce.y = Mathf.SmoothDamp(_moveForce.y, desiredVelocity.y, ref velocitySmoothing, accelerationTime);
        _rigidBody2D.AddForce(_moveForce);
    }


    void LookatTarget()
    {
        if (_target.transform.position.x - transform.position.x < 0)
        {
            _spriteRenderer.flipY = true;
        }
        else
        {
            _spriteRenderer.flipY = false;
        }

        transform.right = _target.transform.position - transform.position;
    }
}
