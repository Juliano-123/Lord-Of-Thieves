using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContadorPuntos : MonoBehaviour
{
    int _puntosTotales;
    TextMeshProUGUI _textoPuntosTotales;

    void Awake()
    {
        _puntosTotales = 0;
        _textoPuntosTotales = GetComponent<TextMeshProUGUI>();
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

}