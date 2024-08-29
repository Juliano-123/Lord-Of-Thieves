using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enem2_Skull : MonoBehaviour, IExplotable, IGolpeable, IJugadorSeteable, ILevelManagerSeteable
{
    GameObject _levelManager;

    [SerializeField]
    AudioSource _audioMostro;
    [SerializeField]
    AudioClip _enemigoStompeado;

    [SerializeField]
    bool _isCalculatingAttack = false;
    [SerializeField]
    bool _isAttacking = false;

    bool _isHit = false;
    bool _isMoving = false;

    [SerializeField]
    GameObject _target;

    SpriteRenderer _spriteRenderer;
    Rigidbody2D _rigidBody2D;
    BoxCollider2D _boxCollider2D;


    float velocitySmoothing;

    [SerializeField]
    float accelerationTime = 0.005f;

    
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
    float _retrocederSpeed = 6f;

    [SerializeField]
    float _speedHit = 30;

    [SerializeField]
    float _timer = 3;

    [SerializeField]
    float _minAttackTime = 2;
    [SerializeField]
    float _maxAttackTime = 3;

    [SerializeField]
    float _minIdleTime = 2;
    [SerializeField]
    float _maxIdleTime = 4;

    [SerializeField]
    float _distanceToTravel;
    [SerializeField]
    float _distanceTraveled;

    [SerializeField]
    Vector2 _directionTowardsTarget;
    [SerializeField]
    Vector3 _positionToStrike;

    GameObject _destroyParticlesObject;
    ParticleSystem _destroyParticlesPS;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();

        //COMPONENTES PROPIOS CHILD
        _destroyParticlesObject = transform.Find("DestroyParticles").gameObject;
        _destroyParticlesPS = _destroyParticlesObject.GetComponent<ParticleSystem>();
        
        Color StartColor = _spriteRenderer.color;
        StartColor.a = 0;
        _spriteRenderer.color = StartColor;

        _boxCollider2D.enabled = false;

    }

    void Update()
    {
        if (_spriteRenderer.color.a == 0)
        {
            StartCoroutine(FadeIn());
        }

        LookatTarget();
    }

    private void FixedUpdate()
    {
        _timer -= Time.deltaTime;

        if (_isHit == true)
        {
            if (_isMoving == true)
            {
                _rigidBody2D.velocity = Vector2.zero;
                _isMoving = false;
                StartCoroutine(Destroy());
            }


            MoveIsHit();
        }

        if (_isMoving == true)
        {
            if (_isAttacking == false)
            {
                if (_timer < 0)
                {
                    _isCalculatingAttack = true;
                    Debug.Log("calcula attack");

                }
                else
                {
                    _directionTowardsTarget = (new Vector3(transform.position.x, 0, 0) - new Vector3(_target.transform.position.x, 0, 0)).normalized;
                    _directionTowardsTarget.y = _retrocederSpeed;
                    MoveToTarget(_retrocederSpeed);
                    Debug.Log("entro retrocede");
                }


            }


            if (_isCalculatingAttack == true && _isAttacking == false)
            {
                _positionToStrike = _target.transform.position;
                _isAttacking = true;
                _isCalculatingAttack = false;
                _rigidBody2D.velocity = Vector3.zero;
                _timer = Random.RandomRange(_minAttackTime, _maxAttackTime);
                Debug.Log("calculo attack");
            }

            if (_isAttacking == true)
            {
                _directionTowardsTarget = (_positionToStrike - transform.position).normalized;
                MoveToTarget(_attackSpeed);
                Debug.Log("ataca");
                if (_timer < 0)
                {
                    _isAttacking = false;
                    _timer = Random.RandomRange(_minIdleTime, _maxIdleTime);
                }
            }

            CheckBounds();
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

    void CheckBounds()
    {
        if (transform.position.x < -18f)
        {
            _isAttacking = false;
            Vector3 CurrentRBVelocity = _rigidBody2D.velocity;
            CurrentRBVelocity.x = 1;
            _rigidBody2D.velocity = CurrentRBVelocity;
        }
        else if  (transform.position.x > 18f)
        {
            _isAttacking = false;
            Vector3 CurrentRBVelocity = _rigidBody2D.velocity;
            CurrentRBVelocity.x = -1;
            _rigidBody2D.velocity = CurrentRBVelocity;
        }

        if (transform.position.y < 1f)
        {
            _isAttacking = false;
            Vector3 CurrentRBVelocity = _rigidBody2D.velocity;
            CurrentRBVelocity.y = 1;
            _rigidBody2D.velocity = CurrentRBVelocity;
        }
        else if (transform.position.y > 16f)
        {
            _isAttacking = false;
            Vector3 CurrentRBVelocity = _rigidBody2D.velocity;
            CurrentRBVelocity.y = -1;
            _rigidBody2D.velocity = CurrentRBVelocity;
        }
    }

    void MoveIsHit()
    {
        _moveForce = Vector2.down * _speedHit;
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
            _boxCollider2D.enabled = true;
            _timer = Random.RandomRange(_minIdleTime, _maxIdleTime);
            _isMoving = true;
            yield break;
        }

    }

    IEnumerator Destroy()
    {

        yield return new WaitForSeconds(0.25f);
        _spriteRenderer.enabled = false;
        _boxCollider2D.enabled = false;
        Explotar();
        _levelManager.GetComponent<IRestarMostros>().RestarMostros(1);
        yield return new WaitForSeconds(0.7f);
        Destroy(gameObject);
        yield break;

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

        _destroyParticlesPS.Play();

    }

    public void Golpear()
    {
        ComboCounter.Instance.AddComboCount();
        _audioMostro.clip = _enemigoStompeado; _audioMostro.Play();
        _isHit = true;
    }

    public void SetearJugador(GameObject Jugador)
    {
        _target = Jugador;
    }

    public void SetearLevelManager(GameObject LevelManager)
    {
        _levelManager = LevelManager;
    }

}
