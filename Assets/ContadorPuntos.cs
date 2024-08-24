using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContadorPuntos : MonoBehaviour
{
    public static ContadorPuntos Instance;

    TextMeshProUGUI _textoPuntosTotales;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _textoPuntosTotales = GetComponent<TextMeshProUGUI>();

    }

    // Update is called once per frame
    void Update()
    {
        _textoPuntosTotales.text = UIPersistantData.Instance.GetPuntosTotales().ToString("#,##0");

    }
}
