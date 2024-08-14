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
    CreadorMounstruos _creadorMounstruos;

    public void Activate()
    {
        _enemiesStomped.text = _creadorMounstruos.GetMostrosStompeados() + " Enemies Stomped";
        gameObject.SetActive(true);
    }

    public void ContinueButton()
    {
        SceneManager.LoadScene("Prueba wall running");
    }

    public void QuitButton()
    {
        Application.Quit();
    }

}
