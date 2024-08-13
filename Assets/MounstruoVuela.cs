using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class MounstruoVuela : MonoBehaviour
{
    [SerializeField]
    public GameObject _target;
    float _velocidad = 3;

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

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_spriteRenderer.color.a == 0)
        {
            StartCoroutine(FadeIn());
            Debug.Log("llame fadein");
        }

        if (_spriteRenderer.color.a >= 1)
        {
            Vector2 IrHacia = Vector2.MoveTowards(transform.position, _target.transform.position, _velocidad * Time.deltaTime);
            _rb.MovePosition(new Vector3(IrHacia.x, IrHacia.y, 0));
        }

    }

    private IEnumerator FadeIn()
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
            Debug.Log("termino corrutina");
            yield break;
        }

    }
}
