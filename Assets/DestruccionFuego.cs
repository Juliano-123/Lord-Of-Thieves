using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestruccionFuego : MonoBehaviour
{
    public GameObject LineadeFuego;

    void Start()
    {
        LineadeFuego = GameObject.Find("LINEA MORTAL");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
     if (transform.position.x < LineadeFuego.transform.position.x)
        {
            Destroy(gameObject);
        }
        

    }
}
