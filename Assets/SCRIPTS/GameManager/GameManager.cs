using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject spawnObjetoPiso;
    //public GameObject spawnObjetoBox;
    //public GameObject spawnObjetoGema;
    public GameObject[] spawnObjetoEnemigo;

    public GameObject spawnPointsPiso;
    public GameObject[] spawnPointsGema;
    public GameObject[] spawnPointsBox;
    public GameObject[] spawnPointsEnemigo;

    public static Vector3 ultimaPosicion;
    public static float distanciaTotal = 0;
    float ultimoSpwanCubos = 0;
    float ultimoSpwanPiso = 0;
    
    int randomBoxesTotal = 1;
    //List<int> lugaresBoxesAnteriores = new List<int>();

    int spawngemasino;

    float maxMonsters = 3;
    public static float totalMonsters = 0;

    private void Awake()
    {
        ultimaPosicion = transform.position;
        distanciaTotal = 0;
    }

    void Update()
    {
        float distanciaDelta = transform.position.x - ultimaPosicion.x;

        ultimaPosicion = transform.position;

        distanciaTotal += distanciaDelta;


        //if (distanciaTotal - ultimoSpwanCubos > 2.5f)
        //{
        //    //SPAWNEO DE CAJAS
        //    randomBoxesTotal = Random.Range(0, 2);
        //    lugaresBoxesAnteriores = new List<int>();
        //    for (int i = 0; i <= randomBoxesTotal; i++)
        //    {
        //        int randomBoxesLugar = Random.Range(0, 3);
        //        while (lugaresBoxesAnteriores.Contains(randomBoxesLugar))
        //        {
        //            randomBoxesLugar = Random.Range(0, 3);
        //        }
        //        Instantiate(spawnObjetoBox, spawnPointsBox[randomBoxesLugar].transform.position, Quaternion.identity);
        //        lugaresBoxesAnteriores.Add(randomBoxesLugar);

        //    }

            ////SPAWNEO DE MOUNSTRUOS
            //if (totalMonsters < maxMonsters)
            //{
            //    int randomeEnemigoLugar = Random.Range(0, 4);
            //    Instantiate(spawnObjetoEnemigo[0], spawnPointsEnemigo[randomeEnemigoLugar].transform.position, Quaternion.identity);
            //    totalMonsters += 1;
            //}   

        //    spawngemasino = Random.Range(0, 2);
        //    if (spawngemasino == 0)
        //    {
        //        int randomGemasLugar = Random.Range(0, 2);           
        //        Instantiate(spawnObjetoGema, spawnPointsGema[randomGemasLugar].transform.position, Quaternion.identity);
        //    }

        //    ultimoSpwanCubos = distanciaTotal;
        //}

        //SPAWNEO DE SUELO
        //if (distanciaTotal - ultimoSpwanPiso >= 9.87f)
        //{
        //    Instantiate(spawnObjetoPiso, spawnPointsPiso.transform.position, Quaternion.identity);

        //    ultimoSpwanPiso = distanciaTotal;
        //}

    }
}