using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RUNBUTTON : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene("Principal");
    }
}
