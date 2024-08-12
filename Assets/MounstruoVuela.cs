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

    void Start()
    {
        
        _rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 IrHacia = Vector2.MoveTowards(transform.position, _target.transform.position, _velocidad*Time.deltaTime);
        _rb.MovePosition(new Vector3(IrHacia.x, IrHacia.y, 0));

    }
}
