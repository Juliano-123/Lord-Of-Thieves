using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COLORFONDO : MonoBehaviour
{
    new Camera camera;
    GameObject lineafuego;

    SpriteRenderer spriteRenderer;

    float diferenciadistancia;
    float escalaporcentual;

    Color near = new Color32(141, 56, 103, 255);
    Color far = new Color32(51, 56, 103, 255);
    Color lerpedColor;


    private void Start()
    {
        lineafuego = GameObject.Find("LINEA MORTAL");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

        diferenciadistancia = GameManager.ultimaPosicion.x - lineafuego.transform.position.x;
        escalaporcentual = 1 - diferenciadistancia / 17;

        if (diferenciadistancia > 17)
        {
            spriteRenderer.color = far;
        }
        else if (diferenciadistancia > 0) {
            lerpedColor = Color.Lerp(far, near, escalaporcentual);
            spriteRenderer.color = lerpedColor;        
        }
        else
        {
            spriteRenderer.color = near;
        }
  

    }
}


