using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YouWinScript : MonoBehaviour
{
    public static YouWinScript Instance;

    [SerializeField]
    TextMeshProUGUI _enemiesStomped;


    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _enemiesStomped.text = CreadorMounstruos.Instance.GetMostrosStompeados() + " Enemies Stomped";
        gameObject.SetActive(true);
    }

    public void ContinueButton()
    {
        SceneManager.LoadScene("Prueba wall running");
        gameObject.SetActive(false);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void Activar()
    {
        gameObject.SetActive(true);
    }

    public void Desactivar()
    {
        gameObject.SetActive(false);
    }
}
