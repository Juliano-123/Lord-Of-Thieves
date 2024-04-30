using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class Apuntar : MonoBehaviour
{
    [SerializeField]
    Camera _maincam;

    [SerializeField]
    Vector3 _mousePosition = Vector3.zero;

    [SerializeField]
    float rotZ;

    Vector2 _directionalInput;



    // Update is called once per frame
    void Update()
    {

        //Vector3 Rotation = _stickValue;


        //con mouse
        //_mousePosition = _maincam.ScreenToWorldPoint(_aim.ReadValue<Vector2>());

        //Vector3 Rotation = _mousePosition - transform.position;

        //con stick
        rotZ = Mathf.Atan2(_directionalInput.y, _directionalInput.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0,0,rotZ);

    }

    public void SetDirectionalInput(Vector2 input)
    {
        _directionalInput = input;
    }

}
