using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ROCAARMA : MonoBehaviour
{

    Controller2D controller;
    [SerializeField]
    Vector2 _velocity;

    void Start()
    {
        controller = GetComponent<Controller2D>();
        if (Player.orientacionX == -1)
        {
            _velocity.x = _velocity.x * -1;
            _velocity.x += Player.velocity.x;
        }
        else
        {
            _velocity.x += Player.velocity.x;
        }


    }

    void Update()
    {
        //APLICA GRAVEDAD
        _velocity.y += Player.gravity/2 * Time.deltaTime;
        

        //if ((controller.collisions.left || controller.collisions.right) && controller.collisions.objetoGolpeado.tag == "Enemigo")
        //{
        //    Destroy(controller.collisions.objetoGolpeado);
        //    Destroy(gameObject);
        //}

        controller.Move(_velocity * Time.deltaTime, false);

    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {

    }




}
