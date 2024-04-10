using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestruccionFuegoPiso : MonoBehaviour
{
    public GameObject LineadeFuego;

    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (LineadeFuego.transform.position.x - transform.position.x > 7)
        {
            Destroy(gameObject);
        }


    }
}
