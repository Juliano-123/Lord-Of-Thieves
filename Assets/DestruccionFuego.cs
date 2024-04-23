using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestruccionFuego : MonoBehaviour
{
    public GameObject LineadeFuego;



    // Update is called once per frame
    void Update()
    {
     if (LineadeFuego.transform.position.x - transform.position.x > 0)
        {
            Destroy(gameObject);
        }
        

    }
}
