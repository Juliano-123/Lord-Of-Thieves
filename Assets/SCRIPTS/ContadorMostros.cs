using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContadorMostros : MonoBehaviour
{

    TMP_Text textoContador;

    // Start is called before the first frame update
    void Start()
    {
        textoContador = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //textoContador.text = "x" + Player.gemasContadas;
    }
}
