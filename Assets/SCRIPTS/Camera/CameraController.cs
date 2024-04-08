using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float ultimaPosicionX;
    float actualPosicionX;
    float ultimaPosicionY;
    float actualPosicionY;



    void Update()
    {
        actualPosicionX = Player.elJugador.transform.position.x;
        actualPosicionY = Player.elJugador.transform.position.y;
        transform.position = new Vector3 (actualPosicionX + (actualPosicionX - ultimaPosicionX) * Time.deltaTime, actualPosicionY + (actualPosicionY - ultimaPosicionY) * Time.deltaTime, transform.position.z);
        ultimaPosicionX = Player.elJugador.transform.position.x;
        ultimaPosicionY = Player.elJugador.transform.position.y;
    }
}
