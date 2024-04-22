using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoPlayerLevelGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject[] _spawnPoints = new GameObject[6];

    [SerializeField]
    GameObject[] _objetoSpawneable = new GameObject[6];

    float _posicionInicial = 0;
    float _posicionActual = 2.8f;
    float _separacionVertical = 2.6f;

    [SerializeField]
    GameObject _objetoReferencia;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_objetoReferencia != null)
        {
            _posicionActual = _objetoReferencia.transform.position.y;

        }

        if (Mathf.Abs(_posicionActual - _posicionInicial) >= _separacionVertical)
        {
            Debug.Log("ENTRE AL IF");
            for (int i = 0; i <=5; i++)
            {
                _objetoReferencia = Instantiate(_objetoSpawneable[0], _spawnPoints[i].transform.position, Quaternion.identity);
                _posicionInicial = _objetoReferencia.transform.position.y;
            }

        }

        

    }
}
