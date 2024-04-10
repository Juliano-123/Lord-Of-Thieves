using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineaMortal : MonoBehaviour
{
    float speed = 0;
    float baseSpeed = 3;
    float accel = 0.12f;
    float desaccel = 0.24f;
    public float distanciaDelta;
    float distanciaCatchUp = 18f;
    
    int gemasAplicadas;

    public GameObject Jugador;
    float ultimaPosicionY;
    float actualPosicionY;

    public Vector2 velocity;


    private void Start()
    {
        speed = baseSpeed;
        gemasAplicadas = 0;
    }


    void FixedUpdate()
    {
        speed = speed + accel * Time.deltaTime;

        if (Player.gemasContadas > gemasAplicadas)
        {
            speed = speed - desaccel * (Player.gemasContadas - gemasAplicadas);
            gemasAplicadas = Player.gemasContadas;
        }


        distanciaDelta = Jugador.transform.position.x - transform.position.x;
        if (distanciaDelta >= distanciaCatchUp)
        {
            velocity = new Vector2(speed*4, 0);
        }
        else
        {
            velocity = new Vector2(speed, 0);
        }

        transform.Translate(velocity * Time.deltaTime);

        if (Jugador.transform.position.y > 0)
        {
            actualPosicionY = Jugador.transform.position.y;
            transform.position = new Vector3(transform.position.x, actualPosicionY + (actualPosicionY - ultimaPosicionY) * Time.deltaTime, transform.position.z);
            ultimaPosicionY = Jugador.transform.position.y;
        }
    }
}
