using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _enemiesStomped;
    [SerializeField]
    TextMeshProUGUI _leftAlive;

    [SerializeField]
    CreadorMounstruos _creadorMounstruos;


    public void Activate()
    {
        _enemiesStomped.text = _creadorMounstruos.GetMostrosStompeados() + " Enemies Stomped";
        _leftAlive.text = _creadorMounstruos.GetMostrosRestantes() + " Left Alive";
        gameObject.SetActive(true);
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("Prueba wall running");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
