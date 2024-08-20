using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public static GameOverScreen Instance;

    [SerializeField]
    TextMeshProUGUI _enemiesStomped;
    [SerializeField]
    TextMeshProUGUI _leftAlive;


    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _enemiesStomped.text = CreadorMounstruos.Instance.GetMostrosStompeados() + " Enemies Stomped";
        _leftAlive.text = CreadorMounstruos.Instance.GetMostrosRestantes() + " Left Alive";
        gameObject.SetActive(true);
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("Prueba wall running");
        gameObject.SetActive(false);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
    public void Desactivar()
    {
        gameObject.SetActive(false);
    }

    public void Activar()
    {
        gameObject.SetActive(true);
    }

}
