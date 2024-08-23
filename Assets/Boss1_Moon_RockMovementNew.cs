using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Moon_RockMovementNew : MonoBehaviour, IGolpeable, IReseteable, IExplotable
{

    //LA LUNA SOBRE LA QUE GIRA
    [SerializeField]
    GameObject _moon;

    [SerializeField]
    float _rotateSpeed = 150f;

    bool _faltaRegreso = true;

    private float _verticalMove;
    private Vector3 _rockPosition;

    [SerializeField]
    float _radius = 1.8f;

    [SerializeField]
    float _timer;

    [SerializeField]
    float _timerCambioRadio;

    [SerializeField]
    float _tiempoExpansion = 4;

    [SerializeField]
    float _tiempoRegreso = 8;

    [SerializeField]
    float _rateDeCrecimiento = 2.5f;

    [SerializeField]
    float _tiempoExpansionMaximayExtra = 6;


    [SerializeField]
    AudioSource _audioPiedra;
    [SerializeField]
    AudioClip _enemigoStompeado;

    Rigidbody2D _rigidbody;
    SpriteRenderer _spriteRenderer;
    BoxCollider2D _boxCollider2D;

    //COMPONENTES CHILD
    GameObject _destroyParticlesObject;
    ParticleSystem _destroyParticlesPS;


    [SerializeField]
    bool _isHit = false;
    bool _isRotating = true;
    float _speedHit = 30;

    [SerializeField]
    Vector3 _firstPosition;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();

        //COMPONENTES PROPIOS CHILD
        _destroyParticlesObject = transform.Find("DestroyParticles").gameObject;
        _destroyParticlesPS = _destroyParticlesObject.GetComponent<ParticleSystem>();

        _timer = 0;
        _timerCambioRadio = 0;
        _firstPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isHit == true)
        {
            MoveIsHit();
        }
        else if (_isRotating == true)
        {
            if (_timerCambioRadio <= _tiempoExpansion && _timer < _tiempoRegreso)
            {
                _radius = _radius + _rateDeCrecimiento * Time.deltaTime;
            }
            else if (_timerCambioRadio <= _tiempoExpansion && _timer > _tiempoRegreso)
            {
                _radius = _radius - _rateDeCrecimiento * Time.deltaTime;

            }

            _rockPosition = _radius * Vector3.Normalize(transform.position - _moon.transform.position) + _moon.transform.position;
            transform.position = _rockPosition;
            transform.RotateAround(_moon.transform.position, Vector3.forward, _rotateSpeed * Time.deltaTime);

            _timer = _timer += Time.deltaTime;
            _timerCambioRadio += Time.deltaTime;

            if (_timer >= _tiempoRegreso && _faltaRegreso == true)
            {
                _timerCambioRadio = 0;
                _faltaRegreso = false;
            }

            if (_timer >= _tiempoRegreso + _tiempoExpansionMaximayExtra)
            {
                _timerCambioRadio = 0;
                _timer = 0;
                _faltaRegreso = true;
            }
        }
    }

    public void Explotar()
    {
        _spriteRenderer.enabled = false;
        //Collider2D[] inExplosionRadius = null;
        //float explosionRadius = 5f;
        //float explosionForceMulti = 400f;

        //inExplosionRadius = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        //foreach (Collider2D colliderDetectado in inExplosionRadius)
        //{
        //    Rigidbody2D rigidbody2DDetectado = colliderDetectado.GetComponent<Rigidbody2D>();

        //    if (rigidbody2DDetectado != null)
        //    {
        //        Vector2 distancia = colliderDetectado.transform.position - transform.position;

        //        if (distancia.sqrMagnitude > 0)
        //        {
        //            float explosionForce = explosionForceMulti;
        //            rigidbody2DDetectado.AddForce(distancia.normalized * explosionForce);
        //            Debug.Log("SE APLICO FUERZA");
        //        }
        //    }
        //}
        _destroyParticlesPS.Play();

    }

    public void Golpear()
    {
        _isHit = true;
        _isRotating = false;
        Invoke(nameof(Explotar), 0.3f);
        _audioPiedra.clip = _enemigoStompeado; _audioPiedra.Play();
    }

    void MoveIsHit()
    {
        _boxCollider2D.enabled = false;
        _rigidbody.AddForce(Vector2.down * _speedHit);
    }

    public void Resetear()
    {
        if (_spriteRenderer.enabled == true)
        {
            Explotar();
        }
        _boxCollider2D.enabled = false;
        _isHit = false;
        _isRotating = false;
        _rigidbody.velocity = Vector2.zero;
        StartCoroutine(ResetToBaseStats());
    }

    IEnumerator ResetToBaseStats()
    {
        yield return new WaitForSeconds(1.5f);
        transform.position = _firstPosition;
        Debug.Log("movio a first");
        yield return new WaitForSeconds(0.5f);
        _timer = 0;
        _timerCambioRadio = 0;
        _radius = 1.8f;
        _faltaRegreso = true;
        _spriteRenderer.enabled = true;
        _boxCollider2D.enabled = true;
        _isRotating = true;
        yield break;
    }




}
