using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YouWinScript : MonoBehaviour
{

    public void Activate()
    {
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
