using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField]
    GameObject _jugador;

    [SerializeField]
    GameObject[] Levels;

    [SerializeField]
    TextMeshProUGUI _enemiesStomped;
    [SerializeField]
    TextMeshProUGUI _leftAlive;


    public void Awake()
    {

        _enemiesStomped.text = UIPersistantData.Instance.GetMostrosStompeados() + " Enemies Stomped";
        _leftAlive.text = UIPersistantData.Instance.GetMostrosFaltaDestruir() + " Left Alive";
        gameObject.SetActive(true);
    }

    public void RestartButton()
    {
        UIPersistantData.Instance.ResetAllData();

        foreach (GameObject Level in Levels)
        {
            Level.SetActive(false);
        }

        Levels[0].SetActive(true);
        _jugador.SetActive(true);


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
