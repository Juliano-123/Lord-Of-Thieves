using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class InputJulian : MonoBehaviour
{
    Player player;
    Apuntar apuntar;

    Vector2 _directionalInput;

    private void Start()
    {
        player = GetComponent<Player>();
        apuntar = GetComponentInChildren<Apuntar>();
    }

    private void Update()
    {
        player.SetDirectionalInput(_directionalInput);
        apuntar.SetDirectionalInput(_directionalInput);

    }

    public void Move(InputAction.CallbackContext context)
    {
        _directionalInput = context.ReadValue<Vector2>();

    }


    public void Jump(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            player.SetJumpApretado(1);
        }

        if (context.canceled)
        {
            player.SetJumpSoltado();
            
        }

    }


    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            player.SetDashApretado(1);
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            player.SetShootApretado(1);
        }

        if (context.canceled)
        {
            player.SetShootSoltado(1);
        }
    }
}











