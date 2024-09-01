using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance;

    // Start is called before the first frame update
    int _currentHealth = 3;
    int _maxHealth = 3;

    [SerializeField]
    Image[] _hearts;


    [SerializeField]
    Sprite _fullHeart;
    [SerializeField]
    Sprite _emptyHeart;

    [SerializeField]
    GameOverScreen _gameOverScreen;

    [SerializeField]
    GameObject _jugador;

    bool _jugadorMuriendo = false;

    [SerializeField]
    Animator _jugadorAnimator;

    [SerializeField]
    AudioSource _audioJugador;
    [SerializeField]
    AudioClip _loseSound;

    float _newRotation = 0;

    [SerializeField]
    float _rotationIncrease = 7f;

    float _newYPosition = 0;

    float _newXPosition = 0;

    [SerializeField]
    float _positionUpdateInterval = 0.001f;

    [SerializeField]
    float _newRotationMultiplier = 2f;

    float _firstYPosition = 0;
    float _firstXPosition = 0;
    float _timer = 0;
    float _maxLoseTime = 3;

    [SerializeField]
    float _maxDieHeight = 5;

    [SerializeField]
    EventSystem _eventSystem;
    [SerializeField]
    GameObject _restartButton;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        _maxHealth = UIPersistantData.Instance.GetMaxHealth();
        _currentHealth = UIPersistantData.Instance.GetCurrentHealth();
    }



    // Update is called once per frame
    void Update()
    {
        //RESETEA LOS HEARTS A DESACTIVADOS Y VACIOS
        foreach (Image heart in _hearts)
        {
            heart.enabled = false;
            heart.sprite = _emptyHeart;
        }

        //ACTIVA SEGUN EL MAX HEALTH
        for (int i = 0; i < _maxHealth; i++)
        {
            _hearts[i].enabled = true;
        }

        //SETEA FULL HEART SEGUN CURRENT HEALTH
        for (int i = 0; i < _currentHealth; i++)
        {
            _hearts[i].sprite = _fullHeart;
        }

        if (_currentHealth == 0)
        {
            ComboCounter.Instance.ResetComboCount();

            if (_jugadorMuriendo == false)
            {
                _jugadorMuriendo = true;
                _jugadorAnimator.SetBool("Cayendo", false);
                _jugadorAnimator.SetBool("Idle", true);
                _jugador.GetComponent<Player>().enabled = false;

                _newRotation = 0;
                _timer = 0;
                _firstYPosition = _jugador.transform.position.y;
                _firstXPosition = _jugador.transform.position.x;
                _audioJugador.clip = _loseSound;
                _audioJugador.Play();
                StartCoroutine(LoseRoutine());
            }
           
        }
    }

    IEnumerator LoseRoutine()
    {
        while (_timer < _maxLoseTime)
        {
            _timer += Time.deltaTime;
            _newRotation += _rotationIncrease;
            _newYPosition = _newRotation / 100;
            _newYPosition += _newYPosition * (2 - _newYPosition / _maxDieHeight); 
            _newXPosition = _newRotation / 100;
            _jugador.transform.rotation = Quaternion.Euler(0, 0, _newRotation);
            if (_newYPosition <= _maxDieHeight)
            {
                _jugador.transform.position = new Vector3(_firstXPosition - _newXPosition, _firstYPosition + _newYPosition, _jugador.transform.position.z);
            }
            else
            {
                _jugador.transform.position = new Vector3(_firstXPosition - _newXPosition, _firstYPosition + _maxDieHeight - _newYPosition/15, _jugador.transform.position.z);
            }
            yield return new WaitForSeconds(_positionUpdateInterval);
        }
        _eventSystem.SetSelectedGameObject(_restartButton);
        _gameOverScreen.gameObject.SetActive(true);
        _jugador.SetActive(false);
        yield break;

    }



    public void SetCurrentHealth(int _changeHealth)
        {
        _currentHealth += _changeHealth;

        }

    public void ResetHealth()
    {
        _maxHealth = UIPersistantData.Instance.GetMaxHealth();
        _currentHealth = UIPersistantData.Instance.GetCurrentHealth();
        _jugadorMuriendo = false;
    }
}
