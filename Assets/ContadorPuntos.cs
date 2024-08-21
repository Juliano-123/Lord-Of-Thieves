using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContadorPuntos : MonoBehaviour
{
    public static ContadorPuntos Instance;

    int _puntosTotales;
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

        _puntosTotales = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _textoPuntosTotales.text = _puntosTotales.ToString("#,##0");

    }

    public void AddPuntos(int puntosAdded)
    {
        _puntosTotales += puntosAdded;
    
    }

    public int GetPuntos()
    {
        return _puntosTotales;
    }

}
