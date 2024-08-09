using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreadorMounstruos : MonoBehaviour
{

    [SerializeField]
    public GameObject _jugador;

    [SerializeField]
    public GameObject _objetoMounstruo;

    public GameObject[] lugarspawn = new GameObject[5];

    float _timeNextSpawn = 2;
    float _timerNextSpawn = 0;

    void Start()
    {
        
    }

    void Update()
    {
        _timerNextSpawn += Time.deltaTime;
        if (_timerNextSpawn > _timeNextSpawn )
        {
            int i = Random.Range(0, 6);

            GameObject ObjetoSpawneado = Instantiate(_objetoMounstruo, lugarspawn[i].transform.position, Quaternion.identity);
            ObjetoSpawneado.GetComponent<MounstruoVuela>()._target = _jugador;

            _timerNextSpawn = 0;
        }


    }
}
