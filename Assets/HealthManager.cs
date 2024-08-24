using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            _gameOverScreen.gameObject.SetActive(true);
           
        }
    }

    public void SetCurrentHealth(int _changeHealth)
        {
        _currentHealth += _changeHealth;

        }

    public void ResetHealth()
    {
        _maxHealth = UIPersistantData.Instance.GetMaxHealth();
        _currentHealth = UIPersistantData.Instance.GetCurrentHealth();
    }
}
