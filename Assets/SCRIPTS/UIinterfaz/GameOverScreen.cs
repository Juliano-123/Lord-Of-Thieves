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


    private void OnEnable()
    {
        _enemiesStomped.text = UIPersistantData.Instance.GetMostrosStompeados() + " Enemies Stomped";
        _leftAlive.text = UIPersistantData.Instance.GetMostrosFaltaDestruir() + " Left Alive";
        Time.timeScale = 0f;

    }

    public void RestartButton()
    {
        Levels[UIPersistantData.Instance.GetStage() - 1].GetComponent<IMostrosDestruibles>().DestruirMostros();

        UIPersistantData.Instance.ResetAllData();
        HealthManager.Instance.ResetHealth();

        foreach (GameObject Level in Levels)
        {
            Level.SetActive(false);
        }

        Levels[0].SetActive(true);
        _jugador.SetActive(true);
        _jugador.GetComponent<Player>().enabled = true;
        _jugador.GetComponent<IReseteable>().Resetear();
        Time.timeScale = 1f;
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
