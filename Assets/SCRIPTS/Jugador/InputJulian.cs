using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class InputJulian : MonoBehaviour
{
    Player player;

    Vector2 _directionalInput;
    private void Start()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        player.SetDirectionalInput(_directionalInput);
    }

    private void Move(InputAction.CallbackContext context)
    {


    }


    private void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

        }

        if (context.canceled)
        {

        }

    }


    private void Dash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

        }
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

        }
    }
}

//los botones de input que tenia definidos

//_playerInput = GetComponent<PlayerInput>();
//_moveAction = _playerInput.actions["MOVE"];
//_jumpAction = _playerInput.actions["JUMP"];
//_dashAction = _playerInput.actions["DASH"];
//_shootAction = _playerInput.actions["SHOOT"];
//_aim = _playerInput.actions["AIM"];
//_jumpReleasedAction = _playerInput.actions["JUMPRELEASED"];


//todo lo que hacian las tomas de input

////SALTO TOMAR INPUT
//if (_jumpAction.triggered) //WasPressedThisFrame())
//{
//    _jumpApretado = _jumpApretado + 1;
//    jumpSoltado = false;
//    tiempoJump1 = Time.time;
//}

////SUELTO SALTO TOMAR INPUT
//if (_jumpReleasedAction.triggered)
//{
//    jumpSoltado = true;
//}

////DASH TOMAR INPUT
//if (_dashAction.WasPressedThisFrame() && timeForNextDash <= 0)
//{
//    _dashApretado = _dashApretado + 1;

//}

////SHOOT TOMAR INPUT
//if (_shootAction.WasPressedThisFrame())
//{
//    _shootApretado = _shootApretado + 1;



