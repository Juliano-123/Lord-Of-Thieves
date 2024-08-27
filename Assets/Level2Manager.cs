using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level2Manager : MonoBehaviour, IRestarMostros, IMostrosDestruibles
{
    [SerializeField]
    TMP_Text _contadorMostros;

    public int _mostrosTotales = 20;
    int _mostrosFaltaSpawnear;
    int _mostrosFaltaDestruir;
    int _mostrosCurrentWave;


    [SerializeField]
    public GameObject _jugador;

    [SerializeField]
    GameObject[] _objetoMounstruo = new GameObject[2];

    [SerializeField]
    GameObject[] _lugaresSpawn = new GameObject[5];

    int _currentWave;
    int _previousWave;

    [SerializeField]
    YouWinScript _youWinScript;


    void Start()
    {
        _contadorMostros.text = _mostrosTotales.ToString();
        _currentWave = 1;
        _mostrosFaltaSpawnear = _mostrosTotales;
        _mostrosFaltaDestruir = _mostrosTotales;
        UIPersistantData.Instance.SetMostrosStompeados(0);
        UIPersistantData.Instance.SetMostrosFaltaDestruir(_mostrosTotales);


    }

    void OnEnable()
    {
        Start();
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
            _youWinScript.gameObject.SetActive(true);
            _jugador.SetActive(false);
            gameObject.SetActive(false);


        }

    }

    void SpawnearMostros(int NdeMostros)
    {
        List<int> _lugaresSpawnPrevios = new List<int>();

        for (int i = 0; i < NdeMostros; i++)
        {
            int LugarSpawn = Random.Range(0, 6);
            while (_lugaresSpawnPrevios.Contains(LugarSpawn))
            {
                LugarSpawn = Random.Range(0, 6);
            }

            int TipoMonstro = Random.Range(0, 2);

            GameObject ObjetoSpawneado = Instantiate(_objetoMounstruo[TipoMonstro], _lugaresSpawn[LugarSpawn].transform.position, Quaternion.identity);
            ObjetoSpawneado.GetComponent<ILevelManagerSeteable>().SetearLevelManager(gameObject);
            ObjetoSpawneado.GetComponent<IJugadorSeteable>().SetearJugador(_jugador);
            _mostrosFaltaSpawnear -= 1;
            _lugaresSpawnPrevios.Add(LugarSpawn);
        }

        _previousWave = _currentWave;
        _currentWave = 0;

    }





    public int GetMostrosRestantes()
    {
        return _mostrosFaltaDestruir;
    }

    public void RestarMostros(int NaRestar)
    {
        _mostrosFaltaDestruir -= NaRestar;
        UIPersistantData.Instance.SetMostrosStompeados(_mostrosTotales - _mostrosFaltaDestruir);
        UIPersistantData.Instance.SetMostrosFaltaDestruir(_mostrosFaltaDestruir);
    }

    public void DestruirMostros()
    {
        GameObject[] Enemigos = GameObject.FindGameObjectsWithTag("Enemigo");
        foreach (GameObject Enemigo in Enemigos)
        {
            Destroy(Enemigo);
        }
    }
}
