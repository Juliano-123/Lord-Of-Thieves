using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineaMortal : MonoBehaviour
{
    Controller2D controller;
    float speed = 0;
    public float baseSpeed = 2;
    //public float maxSpeed;
    public float accel = 0.5f;
    public float desaccel = 0.5f;
    public float distanciaCatchUp;
    public float multiDesacell;
    int gemasAplicadas;

    public GameObject Jugador;
    float ultimaPosicionY;
    float actualPosicionY;

    public Vector2 velocity;


    private void Start()
    {
        controller = GetComponent<Controller2D>();
        speed = baseSpeed;
        gemasAplicadas = 0;
        //acumuladaSpeed = 0;
    }


    void FixedUpdate()
    {
        //if (tiempolentitudgema >= timerLentitudGema)
        //{
        //    speed = baseSpeed + acumuladaSpeed/2;
        //    timerLentitudGema += Time.deltaTime;
        //}
        //else if (maxSpeed >= speed)
        //{
        //    acumuladaSpeed = acumuladaSpeed + accel * Time.deltaTime;
        //    speed = speed + acumuladaSpeed * Time.deltaTime;
        //    //speed = speed + accel * Time.deltaTime;
        //}


        speed = speed + accel * Time.deltaTime;

        if (Player.gemasContadas > gemasAplicadas)
        {
            speed = speed - desaccel * (Player.gemasContadas - gemasAplicadas);
            gemasAplicadas = Player.gemasContadas;
        }


        float distanciaDelta = GameManager.ultimaPosicion.x - transform.position.x;
        if (distanciaDelta >= distanciaCatchUp)
        {
            velocity = new Vector2(100, 0);
        }
        else
        {
            velocity = new Vector2(speed, 0);
        }


        controller.Move(velocity * Time.deltaTime);

        if (Jugador.transform.position.y > 0)
        {
            actualPosicionY = Jugador.transform.position.y;
            transform.position = new Vector3(transform.position.x, actualPosicionY + (actualPosicionY - ultimaPosicionY) * Time.deltaTime, transform.position.z);
            ultimaPosicionY = Jugador.transform.position.y;
        }



        //Destruir todo
        if (controller.collisions.right)
        {
            Destroy(controller.collisions.objetoGolpeado);
        }
    }


}
