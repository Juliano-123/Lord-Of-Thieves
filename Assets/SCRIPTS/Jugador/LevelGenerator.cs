using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject spawnObjetoPiso;
    float ultimaPosicionSpawnPiso;
    // 1 es caja piso 2 escaja bounce
    public GameObject[] spawnObjetoBox = new GameObject[2];
    public GameObject spawnObjetoGema;
    public GameObject spawnObjetoVacio;

    public GameObject LineadeFuego;

    public static Vector3 primeraPosicion;
    public static Vector3 ultimaPosicionSpawn;

    public static float distanciaTotalX = 0;
    public static float distanciaTotalY = 0;

    //los 32 lugares de spawneo se guardan aca
    Vector3[] lugarspawn = new Vector3[33];

    //la decision de spawnear se guarda aca
    int[] spawnboxsiono = new int[33];
    //la decision de spawnear gemas se guarda aca
    int[] spawngemasiono = new int[33];

    //para checkear el overlapbox
    Vector2 sizeacheckear = new Vector2(3,3);
    
    //La separacion X entre cajas es 2.5
    float separacionX = 2.5f;
    //La separacion Y entre cajas es 2.5
    float separacionY = 2.6f;
    //Separacion para que se pare el pj sobre la caja
    float separacionpjcajaY = 0.53f;
    //Separacion entre secciones de piso
    float separacionPiso = 9.87f;

    float distanciaDeltaX;
    float distanciaDeltaY;

    void Awake()
    {
        primeraPosicion = transform.position;
        ultimaPosicionSpawn = transform.position;
        ultimaPosicionSpawnPiso = transform.position.x;
        distanciaTotalX = 0;
        distanciaTotalY = 0;

        for (int i = 1; i <= 32; i++)
        {
            lugarspawn[i].z = 0;       
        }

    }

    void Update()
    {
        distanciaTotalX = transform.position.x - primeraPosicion.x;
        distanciaTotalY = transform.position.y - primeraPosicion.y;

        distanciaDeltaX = transform.position.x - ultimaPosicionSpawn.x;
        distanciaDeltaY = transform.position.y - ultimaPosicionSpawn.y;
            
        SpawnPiso();

        bool spawnear = false;

        spawnear = DecidirSpawnear();



        if (spawnear)
        {
            CalcularLugaresSpawneo();

            //Genera un 0 o 1 para cada lugar de spawn de box
            for (int i = 1; i <= 32; i++)
            {
                spawnboxsiono[i] = Random.Range(0,2);
            }

            //Genera un 0 a 2 para cada lugar de spawn de gema
            for (int i = 1; i <= 32; i++)
            {
                spawngemasiono[i] = Random.Range(0, 3);
            }

            //los dos primeros espacios los instanceo segun el 0 o 1 de spawnsiono
            for (int  i = 1; i <= 2; i++)
            {
                if (!Physics2D.OverlapBox(lugarspawn[i], sizeacheckear, 0) && (lugarspawn[i].y > primeraPosicion.y)) 
                {
                    if (spawnboxsiono[i] == 0)
                    {
                        GameObject ObjetoSpawneado = Instantiate(spawnObjetoVacio, lugarspawn[i], Quaternion.identity);
                        ObjetoSpawneado.GetComponent<DestruccionFuego>().LineadeFuego = LineadeFuego;
                    }
                    else
                    {
                        GameObject ObjetoSpawneado = Instantiate(spawnObjetoBox[1], lugarspawn[i], Quaternion.identity);
                        ObjetoSpawneado.GetComponent<DestruccionFuego>().LineadeFuego = LineadeFuego;
                    }

                    if (spawngemasiono[i] == 0)
                    {
                        GameObject ObjetoSpawneado = Instantiate(spawnObjetoGema, new Vector3(lugarspawn[i].x, lugarspawn[i].y + 1, lugarspawn[i].z), Quaternion.identity);
                        ObjetoSpawneado.GetComponent<DestruccionFuego>().LineadeFuego = LineadeFuego;
                    }
                }

            }

            //Los lugares 3 en adelante tienen en cuenta lo anterior
            for (int i = 3; i <= 32; i++)
            {
                int cajaASpawnear = Random.Range(1, 3);

                if (!Physics2D.OverlapBox(lugarspawn[i], sizeacheckear, 0) && (lugarspawn[i].y > primeraPosicion.y))
                {
                    //Decido que hacer segun que no se haya spawneado 0 o 1 o 2
                    switch (spawnboxsiono[i - 1] + spawnboxsiono[i - 2])
                    {
                        case 0:
                            GameObject ObjetoSpawneado = Instantiate(spawnObjetoBox[cajaASpawnear], lugarspawn[i], Quaternion.identity);
                            ObjetoSpawneado.GetComponent<DestruccionFuego>().LineadeFuego = LineadeFuego;
                            break;

                        case 1:
                            if (spawnboxsiono[i] == 0)
                            {
                                GameObject ObjetoSpawneado1 = Instantiate(spawnObjetoVacio, lugarspawn[i], Quaternion.identity);
                                ObjetoSpawneado1.GetComponent<DestruccionFuego>().LineadeFuego = LineadeFuego;
                            }
                            else
                            {
                                GameObject ObjetoSpawneado1 = Instantiate(spawnObjetoBox[cajaASpawnear], lugarspawn[i], Quaternion.identity);
                                ObjetoSpawneado1.GetComponent<DestruccionFuego>().LineadeFuego = LineadeFuego;
                            }
                                
                        break;

                        case 2:
                            GameObject ObjetoSpawneado3 = Instantiate(spawnObjetoVacio, lugarspawn[i], Quaternion.identity);
                            ObjetoSpawneado3.GetComponent<DestruccionFuego>().LineadeFuego = LineadeFuego;
                        break;
                    }

                    if (spawngemasiono[i] == 0)
                    {
                        GameObject ObjetoSpawneado = Instantiate(spawnObjetoGema, new Vector3(lugarspawn[i].x, lugarspawn[i].y + 1, lugarspawn[i].z), Quaternion.identity);
                        ObjetoSpawneado.GetComponent<DestruccionFuego>().LineadeFuego = LineadeFuego;
                    }
                }

            }

            spawnear = false;

        }       
    }

    void SpawnPiso()
    {
        //spawnear suelo despues de una distancia fija y darle para que se destruya con fuego
        if (distanciaTotalX - ultimaPosicionSpawnPiso >= separacionPiso)
        {
            GameObject suelo = Instantiate(spawnObjetoPiso, new Vector3(distanciaTotalX + separacionPiso * 1.7f, primeraPosicion.y - separacionpjcajaY), Quaternion.identity);
            suelo.GetComponent<DestruccionFuegoPiso>().LineadeFuego = LineadeFuego;
            ultimaPosicionSpawnPiso = distanciaTotalX;
        }

    }

    bool DecidirSpawnear ()
    {
        bool spawnear = false;
        //decide spawnear si la distancia recorrida es mayor a la separacion prevista en ejes x e y
        if (Mathf.Abs(distanciaDeltaX) >= separacionX)
        {
            ultimaPosicionSpawn.x = transform.position.x;
            spawnear = true;
        }

        if (Mathf.Abs(distanciaDeltaY) >= separacionY)
        {
            ultimaPosicionSpawn.y = transform.position.y;
            spawnear = true;
        }

        return spawnear;
    }

    void CalcularLugaresSpawneo ()
    {
        //Calcula el primer lugar, la primera caja que aparece enfrente de el (fuera de la vista)
        lugarspawn[1].x = primeraPosicion.x + separacionX * 4 + separacionX * Mathf.FloorToInt(distanciaTotalX / separacionX);
        lugarspawn[1].y = primeraPosicion.y - separacionpjcajaY + separacionY * Mathf.FloorToInt(distanciaTotalY / separacionY);

        //Calcula el segundo y siguientes tomando como referencia el primer lugarspawn
        lugarspawn[2].x = lugarspawn[1].x;
        lugarspawn[2].y = lugarspawn[1].y - separacionY;

        lugarspawn[3].x = lugarspawn[1].x;
        lugarspawn[3].y = lugarspawn[1].y - separacionY * 2;

        lugarspawn[4].x = lugarspawn[1].x;
        lugarspawn[4].y = lugarspawn[1].y - separacionY * 3;

        lugarspawn[5].x = lugarspawn[1].x;
        lugarspawn[5].y = lugarspawn[1].y - separacionY * 4;

        lugarspawn[6].x = lugarspawn[1].x - separacionX;
        lugarspawn[6].y = lugarspawn[1].y - separacionY * 4;

        lugarspawn[7].x = lugarspawn[1].x - separacionX * 2;
        lugarspawn[7].y = lugarspawn[1].y - separacionY * 4;

        lugarspawn[8].x = lugarspawn[1].x - separacionX * 3;
        lugarspawn[8].y = lugarspawn[1].y - separacionY * 4;

        lugarspawn[9].x = lugarspawn[1].x - separacionX * 4;
        lugarspawn[9].y = lugarspawn[1].y - separacionY * 4;

        lugarspawn[10].x = lugarspawn[1].x - separacionX * 5;
        lugarspawn[10].y = lugarspawn[1].y - separacionY * 4;

        lugarspawn[11].x = lugarspawn[1].x - separacionX * 6;
        lugarspawn[11].y = lugarspawn[1].y - separacionY * 4;

        lugarspawn[12].x = lugarspawn[1].x - separacionX * 7;
        lugarspawn[12].y = lugarspawn[1].y - separacionY * 4;

        lugarspawn[13].x = lugarspawn[1].x - separacionX * 8;
        lugarspawn[13].y = lugarspawn[1].y - separacionY * 4;

        lugarspawn[14].x = lugarspawn[1].x - separacionX * 8;
        lugarspawn[14].y = lugarspawn[1].y - separacionY * 3;

        lugarspawn[15].x = lugarspawn[1].x - separacionX * 8;
        lugarspawn[15].y = lugarspawn[1].y - separacionY * 2;

        lugarspawn[16].x = lugarspawn[1].x - separacionX * 8;
        lugarspawn[16].y = lugarspawn[1].y - separacionY;

        lugarspawn[17].x = lugarspawn[1].x - separacionX * 8;
        lugarspawn[17].y = lugarspawn[1].y;

        lugarspawn[18].x = lugarspawn[1].x - separacionX * 8;
        lugarspawn[18].y = lugarspawn[1].y + separacionY;

        lugarspawn[19].x = lugarspawn[1].x - separacionX * 8;
        lugarspawn[19].y = lugarspawn[1].y + separacionY * 2;

        lugarspawn[20].x = lugarspawn[1].x - separacionX * 8;
        lugarspawn[20].y = lugarspawn[1].y + separacionY * 3;

        lugarspawn[21].x = lugarspawn[1].x - separacionX * 8;
        lugarspawn[21].y = lugarspawn[1].y + separacionY * 4;

        lugarspawn[22].x = lugarspawn[1].x - separacionX * 7;
        lugarspawn[22].y = lugarspawn[1].y + separacionY * 4;

        lugarspawn[23].x = lugarspawn[1].x - separacionX * 6;
        lugarspawn[23].y = lugarspawn[1].y + separacionY * 4;

        lugarspawn[24].x = lugarspawn[1].x - separacionX * 5;
        lugarspawn[24].y = lugarspawn[1].y + separacionY * 4;

        lugarspawn[25].x = lugarspawn[1].x - separacionX * 4;
        lugarspawn[25].y = lugarspawn[1].y + separacionY * 4;

        lugarspawn[26].x = lugarspawn[1].x - separacionX * 3;
        lugarspawn[26].y = lugarspawn[1].y + separacionY * 4;

        lugarspawn[27].x = lugarspawn[1].x - separacionX * 2;
        lugarspawn[27].y = lugarspawn[1].y + separacionY * 4;

        lugarspawn[28].x = lugarspawn[1].x - separacionX;
        lugarspawn[28].y = lugarspawn[1].y + separacionY * 4;

        lugarspawn[29].x = lugarspawn[1].x;
        lugarspawn[29].y = lugarspawn[1].y + separacionY * 4;

        lugarspawn[30].x = lugarspawn[1].x;
        lugarspawn[30].y = lugarspawn[1].y + separacionY * 3;

        lugarspawn[31].x = lugarspawn[1].x;
        lugarspawn[31].y = lugarspawn[1].y + separacionY * 2;

        lugarspawn[32].x = lugarspawn[1].x;
        lugarspawn[32].y = lugarspawn[1].y + separacionY;
    }
}
