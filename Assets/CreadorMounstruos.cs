using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class CreadorMounstruos : MonoBehaviour, IRestarMostros, IMostrosDestruibles
{
    [SerializeField]
    TMP_Text _contadorMostros;

    int _mostrosTotales = 9;
    int _mostrosFaltaSpawnear;
    int _mostrosFaltaDestruir;
    int _mostrosCurrentWave;

    [SerializeField]
    public GameObject _jugador;

    [SerializeField]
    Animator _jugadorAnimator;

    [SerializeField]
    GameObject _objetoMounstruo;

    [SerializeField]
    GameObject[] _lugaresSpawn = new GameObject[5];

    int _currentWave;
    int _previousWave;

    [SerializeField]
    YouWinScript _youWinScript;


    bool _jugadorFestejando = false;

    float _newRotation = 0;

    [SerializeField]
    float _rotationIncrease = 7f;
    [SerializeField]
    float _newYPosition = 0;

    [SerializeField]
    float _positionUpdateInterval = 0.001f;

    [SerializeField]
    float _newRotationMultiplier = 2f;

    float _firstYPosition = 0;

    [SerializeField]
    AudioSource _audioJugador;
    [SerializeField]
    AudioClip _winSound;

    [SerializeField]
    EventSystem _eventSystem;
    [SerializeField]
    GameObject _continueButton;

    void Awake()
    {
        _contadorMostros.text = _mostrosTotales.ToString();
        _currentWave = 1;
        _mostrosFaltaSpawnear = _mostrosTotales;
        _mostrosFaltaDestruir = _mostrosTotales;
        _jugadorFestejando = false;

    }

    void OnEnable()
    {
        Awake();
    }

    void Update()
    {
        _contadorMostros.text = _mostrosFaltaDestruir.ToString();



        switch (_currentWave)
        {
            case 1:
                _mostrosCurrentWave = 1;
                SpawnearMostros(_mostrosCurrentWave);
                _mostrosCurrentWave += 2;
                break;
            
            case 2:
                SpawnearMostros(_mostrosCurrentWave);
                _mostrosCurrentWave += 2;
                break;

            case 3:
                SpawnearMostros(_mostrosCurrentWave);
                _mostrosCurrentWave += 1;
                break;

            //case 4:
            //    SpawnearMostros(_mostrosCurrentWave);
            //    _mostrosCurrentWave += 1;
            //    break;

            //case 5:
            //    SpawnearMostros(_mostrosCurrentWave);
            //    break;

            //case 6:
            //    SpawnearMostros(_mostrosCurrentWave);
            //    break;

        }


        if (_mostrosFaltaDestruir == _mostrosFaltaSpawnear)
        {
            _currentWave = _previousWave + 1;
        }

        if (_mostrosFaltaDestruir == 0)
        {
            ComboCounter.Instance.ResetComboCount();

            if (_jugadorFestejando == false && _jugador.GetComponent<Controller2D>().collisions.objetoGolpeadoVertical != null &&  _jugador.GetComponent<Controller2D>().collisions.objetoGolpeadoVertical.layer == 10)
            {
                _jugadorFestejando = true;
                _jugadorAnimator.SetBool("Cayendo", false);
                _jugadorAnimator.SetBool("Idle", true);
                _jugador.GetComponent<Player>().enabled = false;

                _newRotation = 0;
                _firstYPosition = _jugador.transform.position.y;
                _audioJugador.clip = _winSound;
                _audioJugador.Play();
                StartCoroutine(WinRoutine());
            }
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
            ObjetoSpawneado.GetComponent<ILevelManagerSeteable>().SetearLevelManager(gameObject);
            ObjetoSpawneado.GetComponent<IJugadorSeteable>().SetearJugador(_jugador);
            _mostrosFaltaSpawnear -= 1;          
            _lugaresSpawnPrevios.Add(_lugarSpawn);
        }

        _previousWave = _currentWave;
        _currentWave = 0;

    }


    IEnumerator WinRoutine()
    {

        while (_newRotation < 360)
        {
            _newRotation += _rotationIncrease;
            _rotationIncrease = _rotationIncrease * _newRotationMultiplier;
            _newYPosition = _newRotation / 100;
            _jugador.transform.rotation = Quaternion.Euler(0, 0, _newRotation);
            if (_newRotation <= 180)
            {
                _jugador.transform.position = new Vector3(_jugador.transform.position.x, _firstYPosition + _newYPosition, _jugador.transform.position.z);
            }
            else
            {
                _jugador.transform.position = new Vector3(_jugador.transform.position.x, _firstYPosition + 3.60f - _newYPosition, _jugador.transform.position.z);

            }

            yield return new WaitForSeconds(_positionUpdateInterval);
        }

        if (_newRotation >= 360)
        {
            _jugador.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        yield return new WaitForSeconds(1f);
        _eventSystem.SetSelectedGameObject(_continueButton);
        _youWinScript.gameObject.SetActive(true);
        _jugador.SetActive(false);
        gameObject.SetActive(false);
        UIPersistantData.Instance.SetLevel(2);
        yield break;

    }


    public int GetMostrosRestantes()
    {
        return _mostrosFaltaDestruir;
    }

    public int GetMostrosStompeados()
    {
        return _mostrosTotales - _mostrosFaltaDestruir;
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
