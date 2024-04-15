using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ROCAARMA : MonoBehaviour
{

    Controller2D controller;
    public Vector2 velocity;

    void Start()
    {
        controller = GetComponent<Controller2D>();
        if (Player.orientacionX == 1)
        {
            //velocity = new Vector2(moveSpeed, 0);
        }
        else if (Player.orientacionX == -1)
        {
            velocity.x = velocity.x * -1;
        }


    }

    void Update()
    {
        //APLICA GRAVEDAD
        velocity.y += Player.gravity/2 * Time.deltaTime;
        

        //if ((controller.collisions.left || controller.collisions.right) && controller.collisions.objetoGolpeado.tag == "Enemigo")
        //{
        //    Destroy(controller.collisions.objetoGolpeado);
        //    Destroy(gameObject);
        //}

        controller.Move(new Vector2(velocity.x + Player.velocity.x, velocity.y) * Time.deltaTime, false);

    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {

    }




}
