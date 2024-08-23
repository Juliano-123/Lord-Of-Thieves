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

    public void Awake()
    {

        _enemiesStomped.text = UIPersistantData.Instance.GetMostrosStompeados() + " Enemies Stomped";
        gameObject.SetActive(true);
    }

    public void ContinueButton()
    {
        Levels[UIPersistantData.Instance.GetLevel() - 1].SetActive(true);
        _jugador.SetActive(true);
        gameObject.SetActive(false);
    }

    public void QuitButton()
    {
        Application.Quit();
    }


}
