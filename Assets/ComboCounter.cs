using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboCounter : MonoBehaviour
{
    int _comboCount;
    TextMeshProUGUI _textoComboCounter;

    [SerializeField]
    ComboMessage _comboMessage;

    void Awake()
    {
        _comboCount = 0;
        _textoComboCounter = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (_comboCount > 1)
        {
            if (transform.position.x < 200)
            {
                transform.Translate(new Vector3(1100, 0, 0) * Time.deltaTime);
                Debug.Log("MOVIENDO COMBO COUNTER");
            }
            _textoComboCounter.text = _comboCount.ToString() + "x Combo";
        }
        else
        {
            if (transform.position.x > -240)
            {
                transform.Translate(new Vector3(-1100, 0, 0) * Time.deltaTime);
            }
        }

 
    }

    public void AddComboCount()
    {
        _comboCount += 1;

    }

    public void ResetComboCount()
    {
        if(_comboCount != 0)
        {
            _comboMessage.ShowComboMessage(_comboCount);
            _comboCount = 0;
        }

    }
}
