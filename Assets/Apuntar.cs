using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class Apuntar : MonoBehaviour
{



    [SerializeField]
    float rotZ;

    Vector2 _directionalInput;



    // Update is called once per frame
    void Update()
    {
        rotZ = Mathf.Atan2(_directionalInput.y, _directionalInput.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0,0,rotZ);

    }

    public void SetDirectionalInput(Vector2 input)
    {
        _directionalInput = input;
    }

}
