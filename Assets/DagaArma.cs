using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DagaArma : MonoBehaviour
{
    Controller2D controller;
    [SerializeField]
    Vector2 _velocity = new Vector2(30f, 0f);

    void Start()
    {
        controller = GetComponent<Controller2D>();


    }

    void Update()
    {


        if ((controller.collisions.left || controller.collisions.right) && controller.collisions.objetoGolpeadoHorizontal.tag == "Enemigo")
        {
            Destroy(controller.collisions.objetoGolpeadoHorizontal);
            Destroy(gameObject);
        }

        if ((controller.collisions.above || controller.collisions.below) && controller.collisions.objetoGolpeadoVertical.tag == "Enemigo")
        {
            Destroy(controller.collisions.objetoGolpeadoVertical);
            Destroy(gameObject);
        }


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
