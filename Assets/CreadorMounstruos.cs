using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreadorMounstruos : MonoBehaviour
{

    [SerializeField]
    TMP_Text _contadorMostros;

    int _mostrosTotales = 20;
    int _mostrosFaltaSpawnear;
    int _mostrosFaltaDestruir;
    int _mostrosCurrentWave;


    [SerializeField]
    YouWinScript _youWinScript;

    [SerializeField]
    public GameObject _jugador;

    [SerializeField]
    GameObject _objetoMounstruo;

    [SerializeField]
    GameObject[] _lugaresSpawn = new GameObject[5];

    int _currentWave;
    int _previousWave;


    void Awake()
    {
        _contadorMostros.text = _mostrosTotales.ToString();
        _currentWave = 1;
        _mostrosFaltaSpawnear = _mostrosTotales;
        _mostrosFaltaDestruir = _mostrosTotales;
    }

    void Update()
    {
        _contadorMostros.text = _mostrosFaltaDestruir.ToString();



        switch (_currentWave)
        {
            case 1:
                _mostrosCurrentWave = 1;
                SpawnearMostros(_mostrosCurrentWave);
                _mostrosCurrentWave += 1;
                break;
            
            case 2:
                SpawnearMostros(_mostrosCurrentWave);
                _mostrosCurrentWave += 1;
                break;

            case 3:
                SpawnearMostros(_mostrosCurrentWave);
                _mostrosCurrentWave += 1;
                break;

            case 4:
                SpawnearMostros(_mostrosCurrentWave);
                _mostrosCurrentWave += 1;
                break;

            case 5:
                SpawnearMostros(_mostrosCurrentWave);
                break;

            case 6:
                SpawnearMostros(_mostrosCurrentWave);
                break;

        }


        if (_mostrosFaltaDestruir == _mostrosFaltaSpawnear)
        {
            _currentWave = _previousWave + 1;
        }


        if (_mostrosFaltaDestruir == 0)
        {
            _youWinScript.Activate();
        }

    }

    void SpawnearMostros (int NdeMostros)
    {
        List<int> _lugaresSpawnPrevios = new List<int>();

        for (int i = 0; i < NdeMostros; i++)
        {
            int _lugarSpawn = Random.Range(0, 6);
            while (_lugaresSpawnPrevios.Contains(_lugarSpawn))
            {
                _lugarSpawn = Random.Range(0, 6);
            }

            GameObject ObjetoSpawneado = Instantiate(_objetoMounstruo, _lugaresSpawn[_lugarSpawn].transform.position, Quaternion.identity);
            ObjetoSpawneado.GetComponent<MounstruoVuela>()._target = _jugador;
            _mostrosFaltaSpawnear -= 1;
            _lugaresSpawnPrevios.Add(_lugarSpawn);
        }

        _previousWave = _currentWave;
        _currentWave = 0;

    }

    public void RestarMostros(int NaRestar)
    {
        _mostrosFaltaDestruir -= NaRestar;
    }

    public int GetMostrosRestantes()
    {
        return _mostrosFaltaDestruir;
    }

    public int GetMostrosStompeados()
    {
        return _mostrosTotales - _mostrosFaltaDestruir;
    }

}
