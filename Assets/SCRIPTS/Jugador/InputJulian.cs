using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class InputJulian : MonoBehaviour
{
    Player player;
    Apuntar apuntar;
    PlayerInput playerInput;

    Vector2 _directionalInput;
    Vector2 _mouseInput;
    Vector3 _mousePosition;

    [SerializeField]
    Camera _maincam;

    private void Start()
    {
        player = GetComponent<Player>();
        apuntar = GetComponentInChildren<Apuntar>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        player.SetDirectionalInput(_directionalInput);

        
        if (playerInput.currentControlScheme == "Gamepad")
            {
                apuntar.SetDirectionalInput(_directionalInput);
            }

        if (playerInput.currentControlScheme == "KeyMouse")
            {

            //con mouse
            _mousePosition = _maincam.ScreenToWorldPoint(new Vector3 (_mouseInput.x, _mouseInput.y, 0));

            Vector3 Rotation = _mousePosition - transform.position;


            apuntar.SetDirectionalInput(new Vector2 (Rotation.x, Rotation.y));
            }

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

        if (context.canceled)
        {
            player.SetDashSoltado();
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

    public void Aim(InputAction.CallbackContext context)
    {
        _mouseInput = context.ReadValue<Vector2>();
    }

}











