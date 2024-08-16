using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocaArma : MonoBehaviour
{

    Controller2D controller;
    [SerializeField]
    Vector2 _velocity = new Vector2(3.5f, 3f);

    void Start()
    {
        controller = GetComponent<Controller2D>();
        //if (Player.orientacionX == -1)
        //{
        //    _velocity.x = _velocity.x * -1;
        ////    _velocity.x += Player.velocity.x;
        //}
        //else
        //{
        ////    _velocity.x += Player.velocity.x;
        //}


    }

    void Update()
    {
        //APLICA GRAVEDAD
        //_velocity.y += Player.gravity/2 * Time.deltaTime;
        

        //if ((controller.collisions.left || controller.collisions.right) && controller.collisions.objetoGolpeadoHorizontal.tag == "Enemigo")
        //{
        //    Destroy(controller.collisions.objetoGolpeadoHorizontal);
        //    Destroy(gameObject);
        //}
        
        //if ((controller.collisions.above || controller.collisions.below) && controller.collisions.objetoGolpeadoVertical.tag == "Enemigo")
        //{
        //    Destroy(controller.collisions.objetoGolpeadoVertical);
        //    Destroy(gameObject);
        //}


        //controller.Move(_velocity * Time.deltaTime, false);

    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {

    }




}
