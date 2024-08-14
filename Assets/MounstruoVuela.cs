using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class MounstruoVuela : MonoBehaviour
{
    [SerializeField]
    public GameObject _target;
    

    const float _forcePower = 10f;
    [SerializeField]
    float _speed = 4f;
    [SerializeField]
    float _force = 15f;
        
    float velocitySmoothing;
    
    [SerializeField]
    float accelerationTime = 0.005f;

    Vector2 _moveForce;
    Vector2 _directionTowardsTarget;

    Rigidbody2D _rb;
    SpriteRenderer _spriteRenderer;
    BoxCollider2D _boxCollider2D;

    void Start()
    {
        
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();


        Color StartColor = _spriteRenderer.color;
        StartColor.a = 0;
        _spriteRenderer.color = StartColor;

        gameObject.tag = "Untagged";
        gameObject.layer = 0;
        _boxCollider2D.isTrigger = true;

    }

    void Update()
    {
        if (_spriteRenderer.color.a == 0)
        {
            StartCoroutine(FadeIn());
        }

        _directionTowardsTarget = (_target.transform.position - transform.position).normalized;

    }

    
    void FixedUpdate()
    {
        if (_spriteRenderer.color.a >= 1)
        {
            MoveToTarget(_directionTowardsTarget);
        }

    }

    
    void MoveToTarget(Vector2 Direction)
    {
        Vector2 desiredVelocity = Direction * _speed;
        Vector2 deltaVelocity = desiredVelocity - _rb.velocity;
        _moveForce = deltaVelocity * (_force * _forcePower * Time.fixedDeltaTime);
        _moveForce.x = Mathf.SmoothDamp(_moveForce.x, desiredVelocity.x, ref velocitySmoothing, accelerationTime);
        _moveForce.y = Mathf.SmoothDamp(_moveForce.y, desiredVelocity.y, ref velocitySmoothing, accelerationTime);
        _rb.AddForce(_moveForce);
    }


    IEnumerator FadeIn()
    {
        float alphaVal = _spriteRenderer.color.a;
        Color tmp = _spriteRenderer.color;

        while (_spriteRenderer.color.a < 1)
        {
            alphaVal += 0.01f;
            tmp.a = alphaVal;
            _spriteRenderer.color = tmp;
        
            yield return new WaitForSeconds(0.005f);
        }


        if (_spriteRenderer.color.a >= 1)
        {
            gameObject.tag = "Enemigo";
            gameObject.layer = 9;
            _boxCollider2D.isTrigger = false;
            yield break;
        }

    }
}
