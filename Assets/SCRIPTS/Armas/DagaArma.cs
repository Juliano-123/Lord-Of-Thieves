using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DagaArma : MonoBehaviour
{
    Controller2D controller;
    float moveSpeed = 8;
    Vector2 velocity;

    void Start()
    {
        controller = GetComponent<Controller2D>();
        if (Player.orientacionX == 1)
        {
            velocity = new Vector2(moveSpeed, 0);
        }
        else if (Player.orientacionX == -1)
        {
            velocity = new Vector2(-moveSpeed, 0);
        }

    }

    void Update()
    {




        if ((controller.collisions.left || controller.collisions.right) && controller.collisions.objetoGolpeado.tag == "Enemigo" )
        {
            Destroy(controller.collisions.objetoGolpeado);
            Destroy(gameObject);
        }



        controller.Move(velocity * Time.deltaTime, false);

    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        DagaJugador.totalDisparado -= 1;
    }
}
