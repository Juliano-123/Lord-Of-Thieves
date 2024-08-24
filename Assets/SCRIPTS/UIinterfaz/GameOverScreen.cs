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
        Levels[UIPersistantData.Instance.GetLevel() - 1].GetComponent<IMostrosDestruibles>().DestruirMostros();

        UIPersistantData.Instance.ResetAllData();
        HealthManager.Instance.ResetHealth();

        foreach (GameObject Level in Levels)
        {
            Level.SetActive(false);
        }

        Levels[0].SetActive(true);
        _jugador.transform.position = new Vector3(-10, 3, 0);
        _jugador.SetActive(true);
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
