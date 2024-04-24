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








    // Update is called once per frame
    void Update()
    {
        //con stick
        //_stickValue = _aim.ReadValue<Vector2>();

        //Vector3 Rotation = _stickValue;


        //con mouse
        //_mousePosition = _maincam.ScreenToWorldPoint(_aim.ReadValue<Vector2>());

        //Vector3 Rotation = _mousePosition - transform.position;

        rotZ = Mathf.Atan2(Player._stickValue.y, Player._stickValue.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0,0,rotZ);

    }
}
