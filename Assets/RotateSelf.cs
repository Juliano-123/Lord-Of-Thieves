using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSelf : MonoBehaviour
{
    bool _isRotating;

    [SerializeField]
    float _rotateSpeed =5f;

    [SerializeField]
    GameObject _moon;

    Vector3 _zAxis = new Vector3(0, 0, 1); 

    void Update()
    {

        transform.RotateAround(_moon.transform.position, _zAxis, _rotateSpeed * Time.deltaTime);

    }
}
