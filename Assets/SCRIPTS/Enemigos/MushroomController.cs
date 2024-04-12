using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class MushroomController : MonoBehaviour
{

    public Animator animator;
    SpriteRenderer playerSpriteRenderer;
    Controller2D controller;

    new Camera camera;

    Vector3 posicionIzquierda;
    Vector3 posicionDerecha;

    Vector2 velocity;
    float moveSpeed = -2;


    void Start()
    {
        controller = GetComponent<Controller2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        camera = Camera.main;
        velocity = new Vector2(moveSpeed,0);
    }

    
    void Update()
    {
        //SI LE PEGO AL JUGADOR ................
        if (controller.collisions.left && controller.collisions.objetoGolpeado.tag == "Player")
        {
            Player.timerGolpeadoDerecha = 0;
        }

        if (controller.collisions.right && controller.collisions.objetoGolpeado.tag == "Player")
        {
            Player.timerGolpeadoIzquierda = 0;
        }

        posicionIzquierda = camera.ScreenToWorldPoint(new Vector3(0, 0, 0));
        posicionDerecha = camera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));

        if (transform.position.x < posicionIzquierda.x)
        {
            transform.Translate(posicionDerecha.x-3.4f,0,0);
        }
     

        //LLAMA A LA FUNCION MOVE, PARA QUE SE MUEVA DETECTANDO COLISION
        controller.Move(velocity * Time.deltaTime, false);

    }

    private void OnDestroy()
    {
        //GameManager.totalMonsters -= 1;
    }
}
