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

    [SerializeField]
    Vector2 _stickValue;

    PlayerInput _playerInput;
    InputAction _aim;

    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _aim = _playerInput.actions["AIM"]; 
    }

    // Update is called once per frame
    void Update()
    {

        _stickValue = _aim.ReadValue<Vector2>();


        _mousePosition = _maincam.ScreenToWorldPoint(_aim.ReadValue<Vector2>());

        Vector3 Rotation = _mousePosition - transform.position;

        rotZ = Mathf.Atan2(Rotation.y, Rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0,0,rotZ);

    }
}
