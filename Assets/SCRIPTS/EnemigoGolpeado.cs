using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoGolpeado : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        Destroy(gameObject, 0.6f);
    }
}
