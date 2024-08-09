using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class MounstruoVuela : MonoBehaviour
{
    [SerializeField]
    public GameObject _target;
    float _velocidad = 3;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _target.transform.position, _velocidad*Time.deltaTime);
    }
}
