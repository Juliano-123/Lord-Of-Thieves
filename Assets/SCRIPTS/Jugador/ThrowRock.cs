using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowRock : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject _spawnObjetoRoca;

    Vector2 _lugarSpawn;

    public int _dispararApretado = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //CALCULAR LUGAR SPAWN
        //OFFSETS
        //X. 0.000357117
        //Y. 0.03107139

        _lugarSpawn.x = transform.position.x + 0.000357117f;
        _lugarSpawn.y = transform.position.y + 0.03107139f;


        //TIRAR PEIDRA TOMAR INPUT
        if (Input.GetButtonDown("Fire3"))
        {
            _dispararApretado = _dispararApretado + 1;
        }

        //INSTANCIAR PIEDRA
        if (_dispararApretado > 0 )
        {
            Instantiate(_spawnObjetoRoca, _lugarSpawn, Quaternion.identity);
            _dispararApretado = 0;
        }

    }
}
