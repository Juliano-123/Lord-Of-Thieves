using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinalScore : MonoBehaviour
{

    TextMeshProUGUI _textoFinalScore;

    // Start is called before the first frame update
    void OnEnable()
    {
        _textoFinalScore = GetComponent<TextMeshProUGUI>();
        _textoFinalScore.text = "Score " + UIPersistantData.Instance.GetPuntosTotales().ToString("#,##0");
    }

}
