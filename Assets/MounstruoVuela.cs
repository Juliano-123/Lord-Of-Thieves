using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class MounstruoVuela : MonoBehaviour, IExplotable
{
    [SerializeField]
    AudioSource _audioMostro;
    [SerializeField]
    AudioClip _enemigoStompeado;



    [SerializeField]
    public GameObject _target;

    bool _isMoving = false;
    public bool _isHit = false;

    int orientacionX = 0;

    const float _forcePower = 10f;
    [SerializeField]
    float _speed = 4f;
    [SerializeField]
    float _force = 15f;

    [SerializeField]
    float _speedHit = 30;
        
    float velocitySmoothing;
    
    [SerializeField]
    float accelerationTime = 0.005f;

    [SerializeField]
    float accelerationTimeHit = 0.005f;

    Vector2 _moveForce;
    Vector2 _directionTowardsTarget;
    
    //COMPONENTES PROPIOS
    Rigidbody2D _rb;
    SpriteRenderer _spriteRenderer;
    BoxCollider2D _boxCollider2D;

    //COMPONENTES CHILD
    GameObject _destroyParticlesObject;
    ParticleSystem _destroyParticlesPS;

    //COMPONENTES OTROS
    public CreadorMounstruos _creadorMounstruos;

    void Awake()
    {
        //COMPONENTES PROPIOS
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();

        //COMPONENTES PROPIOS CHILD
        _destroyParticlesObject = transform.Find("DestroyParticles").gameObject;
        _destroyParticlesPS = _destroyParticlesObject.GetComponent<ParticleSystem>();

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

        CambiarDireccionSprite();

        _directionTowardsTarget = (_target.transform.position - transform.position).normalized;

    }

    
    void FixedUpdate()
    {
        if (_isHit)
        {
            if(_isMoving)
            {
                _rb.velocity = Vector2.zero;
                _isMoving = false;
                StartCoroutine(Destroy());
            }


            MoveIsHit();
        }

        if (_isMoving)
        {
            MoveToTarget(_directionTowardsTarget);
        }



    }


    void CambiarDireccionSprite()
    {
        //GUARDA LA ULTIMA ORIENTACION
        if (_moveForce.x > 0)
        {
            orientacionX = 1;
        }
        else if (_moveForce.x < 0)
        {
            orientacionX = -1;
        }

        //VOLTEA EL SPRITE
        if (orientacionX == 1 && _spriteRenderer.flipX == true)
        {
            _spriteRenderer.flipX = false;
        }
        else if (orientacionX == -1 && _spriteRenderer.flipX == false)
        {
            _spriteRenderer.flipX = true;
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

    void MoveIsHit()
    {
        //Vector2 desiredVelocity = Vector2.down * _speedHit;
        //_moveForce.x = 0;
        //_moveForce.y = Mathf.SmoothDamp(_moveForce.y, desiredVelocity.y, ref velocitySmoothing, accelerationTimeHit);
        _moveForce = Vector2.down * _speedHit;
        _rb.AddForce(_moveForce);
    }

    public void Explotar()
    {
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

    }



    IEnumerator Destroy()
    {
        _audioMostro.clip = _enemigoStompeado; _audioMostro.Play();
        yield return new WaitForSeconds(0.25f);
        _spriteRenderer.enabled = false;
        _boxCollider2D.enabled = false;
        Explotar();
        _destroyParticlesPS.Play();
        _creadorMounstruos.RestarMostros(1);
        yield return new WaitForSeconds(0.7f);
        Destroy(gameObject);
        yield break;
                    
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
            _isMoving = true;
            yield break;
        }

    }
}
