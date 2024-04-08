using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DagaJugador : MonoBehaviour
{
    float timer = 0;
    float cooldown = 0.5f;
    public GameObject objetoDisparado;

    public static int totalDisparado = 0;
    public int totaldisparadodebug;
    int maximoDisparado = 2;

    void Start()
    {
        
    }

    void Update()
    {
        totaldisparadodebug = totalDisparado;
        timer += Time.deltaTime;
        if (timer >= cooldown && totalDisparado < maximoDisparado)
        {
            Instantiate(objetoDisparado, transform.position, Quaternion.identity);
            totalDisparado += 1;
            timer = 0;
        }

    }
}
