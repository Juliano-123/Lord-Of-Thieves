using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoDisparo1 : MonoBehaviour
{
    [SerializeField]
    GameObject _spawnObjetoBala;
    [SerializeField]
    GameObject _elJugador;

    [SerializeField]
    float _shotDelay = 0.5f;
    float _shotTimer = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _shotTimer += Time.deltaTime;
        if (_shotTimer >= _shotDelay )
        {
            GameObject Bala = Instantiate(_spawnObjetoBala, transform.position, Quaternion.identity);
            Bala.GetComponent<Bala>()._elJugador = _elJugador;
            _shotTimer = 0;
        }


    }
}
