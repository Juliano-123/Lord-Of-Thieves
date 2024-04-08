using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    float longitud;
    float posicion1;
    public GameObject camara;
    public float parallaxEffect;

    void Start()
    {
        posicion1 = transform.position.x;
        longitud = GetComponent<SpriteRenderer>().bounds.size.x;

    }

    void Update()
    {
        float distanciatiempo = (camara.transform.position.x * (1 - parallaxEffect));
        float distancia = (camara.transform.position.x * parallaxEffect);

        transform.position = new Vector3(posicion1 + distancia, transform.position.y, transform.position.z);

        if (distanciatiempo > posicion1 + longitud)
        {
            posicion1 += longitud;
        }
        else if (distanciatiempo < posicion1 - longitud)
        {
            posicion1 -= longitud;
        }
    }
}