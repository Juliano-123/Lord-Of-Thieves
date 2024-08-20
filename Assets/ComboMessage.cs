using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboMessage : MonoBehaviour
{
    public static ComboMessage Instance;

    TextMeshProUGUI _textoComboMessage;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _textoComboMessage = GetComponent<TextMeshProUGUI>();
    }

    public void ShowComboMessage(int ComboCount)
    {
        switch (ComboCount)
        {
            case int i when i <= 4:
                _textoComboMessage.text = "Good!";
                break;
            case int i when i <= 8:
                _textoComboMessage.text = "Great!!";
                break;
            case int i when i >= 9:
                _textoComboMessage.text = "Amazing!!!";
                break;
        }

        if(ComboCount > 1)
        {
            StartCoroutine(ShowMessage());
        }
    }

    IEnumerator ShowMessage()
    {
        while (transform.position.x < 200)
        {
            Debug.Log("moviendoTransform corru");
            transform.Translate(new Vector3(1100, 0, 0) * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.9f);

        while (transform.position.x > -240)
        {
            transform.Translate(new Vector3(-1100, 0, 0) * Time.deltaTime);
            yield return null;
        }
        Debug.Log("termino la corru");
        yield break;
    }
}
