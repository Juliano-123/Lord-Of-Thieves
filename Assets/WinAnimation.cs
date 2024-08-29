using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinAnimation : MonoBehaviour
{
    float _newRotation = 0;

    [SerializeField]
    float _rotationIncrease = 0.5f;
    [SerializeField]
    float _newYPosition = 0;

    [SerializeField]
    float _positionUpdateInterval = 0.1f;

    [SerializeField]
    float _intervalDecrease = 0.0005f;

    float _firstYPosition = 0;

    [SerializeField]
    AudioSource _audioJugador;
    [SerializeField]
    AudioClip _winSound;

    // Start is called before the first frame update
    void Awake()
    {
        _newRotation = 0;
        _firstYPosition = transform.position.y;
        _audioJugador.clip = _winSound;
        _audioJugador.Play();
        StartCoroutine(WinRoutine());        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WinRoutine()
    {

        while (_newRotation < 360)
        {
            _newRotation += _rotationIncrease;
            _newYPosition = _newRotation /100;
            _positionUpdateInterval = _positionUpdateInterval / _intervalDecrease;
            transform.rotation = Quaternion.Euler(0, 0, _newRotation);
            if (_newRotation <= 180)
            {
                transform.position = new Vector3 (transform.position.x, _firstYPosition + _newYPosition, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, _firstYPosition + 3.60f - _newYPosition, transform.position.z);

            }

            yield return new WaitForSeconds(_positionUpdateInterval);
        }

        if (_newRotation >= 360)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        yield break;

    }
}
