using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YouWinScript : MonoBehaviour
{

    [SerializeField]
    TextMeshProUGUI _enemiesStomped;

    [SerializeField]
    HealthManager _healthManager;

    [SerializeField]
    ContadorPuntos _contadorPuntos;

    public void Awake()
    {

        _enemiesStomped.text = CreadorMounstruos.Instance.GetMostrosStompeados() + " Enemies Stomped";
        gameObject.SetActive(true);
    }

    public void ContinueButton()
    {
        SceneManager.LoadScene("Prueba wall running");

        ResetConfiner.Instance.ResetConfinerExterno();

        UIPersistantData.Instance.SetMaxHealth(_healthManager.GetMaxHealth());
        UIPersistantData.Instance.SetCurrentHealth(_healthManager.GetCurrentHealth());
        UIPersistantData.Instance.SetPuntosTotales(_contadorPuntos.GetPuntos());
        Debug.Log(UIPersistantData.Instance.GetPuntosTotales());
        gameObject.SetActive(false);
    }

    public void QuitButton()
    {
        Application.Quit();
    }


}
