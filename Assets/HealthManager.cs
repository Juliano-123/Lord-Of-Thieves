using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class HealthManager : MonoBehaviour
{

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
        _maxHealth = 3;
        _currentHealth = 3;

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

    public int GetMaxHealth() { return _maxHealth; }

    public int GetCurrentHealth() { return _currentHealth; }

}
