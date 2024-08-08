using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Bala : MonoBehaviour
{

    public GameObject _elJugador;

    Controller2D _controller;

    [SerializeField]
    float _speed = 2f;
    Vector2 _velocity = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<Controller2D>();
        _velocity = new Vector2(_elJugador.transform.position.x - transform.position.x, _elJugador.transform.position.y - transform.position.y);
        _velocity = _velocity.normalized * _speed;

    }

    // Update is called once per frame
    void Update()
    {

        //if (_controller.collisions.objetoGolpeadoHorizontal != null)
        //{
        //    if (_controller.collisions.left && _controller.collisions.objetoGolpeadoHorizontal.tag is "Player")
        //    {
        //        _elJugador.GetComponent<Player>().timerGolpeadoDerecha = 0;
        //        _elJugador.GetComponent<Player>()._jugadorGolpeado = true;
        //        Debug.Log("le pego la bala2");
        //    }
        //    else if (_controller.collisions.right && _controller.collisions.objetoGolpeadoHorizontal.tag is "Player")
        //    {
        //        _elJugador.GetComponent<Player>().timerGolpeadoIzquierda = 0;
        //        _elJugador.GetComponent<Player>()._jugadorGolpeado = true;
        //    }
        //    Destroy(gameObject);
        //}

        //if (_controller.collisions.objetoGolpeadoVertical != null)
        //{
        //    if (_controller.collisions.objetoGolpeadoVertical.CompareTag("Player"))
        //    {
        //        _elJugador.GetComponent<Player>().timerGolpeadoDerecha = 0;
        //        _elJugador.GetComponent<Player>()._jugadorGolpeado = true;
        //    }
        //    Destroy(gameObject);
        //}

        _controller.Move(_velocity * Time.deltaTime, false);

    }
}
