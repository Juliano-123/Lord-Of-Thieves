using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreadorMounstruos : MonoBehaviour
{

    [SerializeField]
    TMP_Text _contadorMostros;

    int _mostrosTotales = 30;
    int _mostrosRestantes = 30;
    int _mostrosSpawneados = 0;

    [SerializeField]
    public GameObject _jugador;

    [SerializeField]
    public GameObject _objetoMounstruo;

    public GameObject[] lugarspawn = new GameObject[5];

    float _timeNextSpawn = 5;
    float _timerNextSpawn = 3;

    void Awake()
    {
        _contadorMostros.text = _mostrosTotales.ToString();

    }

    void Update()
    {
        _contadorMostros.text = _mostrosRestantes.ToString();
        _timerNextSpawn += Time.deltaTime;
        if (_timerNextSpawn > _timeNextSpawn && _mostrosRestantes != 0)
        {
            for (int i = 0; i < 6 && _mostrosSpawneados < _mostrosTotales; i++)
            {
                GameObject ObjetoSpawneado = Instantiate(_objetoMounstruo, lugarspawn[i].transform.position, Quaternion.identity);
                ObjetoSpawneado.GetComponent<MounstruoVuela>()._target = _jugador;
                _mostrosSpawneados += 1;
            }
            _timerNextSpawn = 0;
        }

    }

    public void RestarMostros(int NaRestar)
    {
        _mostrosRestantes -= NaRestar;
    }
}
