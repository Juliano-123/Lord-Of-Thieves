using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YouWinScript : MonoBehaviour
{
    [SerializeField]
    GameObject _jugador;

    [SerializeField]
    GameObject[] Levels;

    [SerializeField]
    TextMeshProUGUI _enemiesStomped;

    [SerializeField]
    HealthManager _healthManager;

    [SerializeField]
    ContadorPuntos _contadorPuntos;


    private void OnEnable()
    {
        _enemiesStomped.text = UIPersistantData.Instance.GetMostrosStompeados() + " Enemies Stomped";
        Time.timeScale = 0f;
    }

    public void ContinueButton()
    {
        Levels[UIPersistantData.Instance.GetLevel() - 1].SetActive(true);
        _jugador.GetComponent<IReseteable>().Resetear();
        _jugador.SetActive(true);
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    public void QuitButton()
    {
        Application.Quit();
    }


}
