using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class MostroBlanco : MonoBehaviour
{

    Animator animator;
    SpriteRenderer _currentSpriteRenderer;
    Controller2D _controller;
    public GameObject _elJugador;

    [SerializeField]
    Vector2 _velocity = Vector2.zero;

    //La separacion X entre cajas es 2.5
    float separacionX = 2.5f;
    //La separacion Y entre cajas es 2.6
    float separacionY = 2.6f;
    //Separacion para que se pare el pj sobre la caja
    float separacionpjcajaY = 0.53f;

    Vector2[] _lugaresCajasPosibles = new Vector2[15];
    
    bool[] _hayAlgo = new bool[15];
    Vector2 _sizeacheckear = new Vector2(1, 1);
    float _timersalto = 2;
    int _haySalto2 = 0;

    

    void Start()
    {
        _controller = GetComponent<Controller2D>();
        _currentSpriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        
    }


    void Update()
    {
        //APLICA GRAVEDAD
        _velocity.y += Player.gravity * Time.deltaTime;

        //MANDA VELOCIDAD X A 0 CUANDO ATERRIZA
        if ( _controller.collisions.below )
        {
            _velocity.x = 0;
        }

        //Solo checka donde moverse si esta parado sobre algo
        if (_controller.collisions.below && _timersalto < 0)
        {
            CheckearEspacios();

            EnemigoSalta();


        }
        //si le pego al jugador ................
        if (_controller.collisions.left && _controller.collisions.objetoGolpeado.tag == "Player")
        {
            _elJugador.GetComponent<Player>().timerGolpeadoDerecha = 0;
        }

        if (_controller.collisions.right && _controller.collisions.objetoGolpeado.tag == "Player")
        {
            _elJugador.GetComponent<Player>().timerGolpeadoIzquierda = 0;
        }


        if (_timersalto < 0)
        {
            _timersalto = 2;
        }

        _timersalto -= Time.deltaTime;

        //LLAMA A LA FUNCION MOVE, PARA QUE SE MUEVA DETECTANDO COLISION
        _controller.Move(_velocity * Time.deltaTime, false);
        

    }

    void CheckearEspacios()
    {
        //CALCULA EL PRIMER LUGAR SEGUN DONDE ESTA EL PERSONAJE
        _lugaresCajasPosibles[1].x = transform.position.x + separacionX;
        _lugaresCajasPosibles[1].y = transform.position.y - separacionpjcajaY;

        //CALCULA LOS SIGUIENTES EN FUNCION DEL PRIMERO
        _lugaresCajasPosibles[2].x = _lugaresCajasPosibles[1].x + separacionX;
        _lugaresCajasPosibles[2].y = _lugaresCajasPosibles[1].y;

        _lugaresCajasPosibles[3].x = _lugaresCajasPosibles[1].x + separacionX;
        _lugaresCajasPosibles[3].y = _lugaresCajasPosibles[1].y - separacionY;

        _lugaresCajasPosibles[4].x = _lugaresCajasPosibles[1].x;
        _lugaresCajasPosibles[4].y = _lugaresCajasPosibles[1].y - separacionY;

        _lugaresCajasPosibles[5].x = _lugaresCajasPosibles[1].x - separacionX;
        _lugaresCajasPosibles[5].y = _lugaresCajasPosibles[1].y - separacionY;

        _lugaresCajasPosibles[6].x = _lugaresCajasPosibles[1].x - separacionX * 2;
        _lugaresCajasPosibles[6].y = _lugaresCajasPosibles[1].y - separacionY;

        _lugaresCajasPosibles[7].x = _lugaresCajasPosibles[1].x - separacionX * 3;
        _lugaresCajasPosibles[7].y = _lugaresCajasPosibles[1].y - separacionY;

        _lugaresCajasPosibles[8].x = _lugaresCajasPosibles[1].x - separacionX * 3;
        _lugaresCajasPosibles[8].y = _lugaresCajasPosibles[1].y;

        _lugaresCajasPosibles[9].x = _lugaresCajasPosibles[1].x - separacionX * 2;
        _lugaresCajasPosibles[9].y = _lugaresCajasPosibles[1].y;

        _lugaresCajasPosibles[10].x = _lugaresCajasPosibles[1].x - separacionX * 3;
        _lugaresCajasPosibles[10].y = _lugaresCajasPosibles[1].y + separacionY;

        _lugaresCajasPosibles[11].x = _lugaresCajasPosibles[1].x - separacionX * 2;
        _lugaresCajasPosibles[11].y = _lugaresCajasPosibles[1].y + separacionY;

        _lugaresCajasPosibles[12].x = _lugaresCajasPosibles[1].x - separacionX;
        _lugaresCajasPosibles[12].y = _lugaresCajasPosibles[1].y + separacionY;

        _lugaresCajasPosibles[13].x = _lugaresCajasPosibles[1].x;
        _lugaresCajasPosibles[13].y = _lugaresCajasPosibles[1].y + separacionY;

        _lugaresCajasPosibles[14].x = _lugaresCajasPosibles[1].x + separacionX;
        _lugaresCajasPosibles[14].y = _lugaresCajasPosibles[1].y + separacionY;



        //guarda en el array de hay algo si hay algo
        for (int i = 1; i <= 14; i++)
        {
            if (Physics2D.OverlapBox(_lugaresCajasPosibles[i], _sizeacheckear, 0, LayerMask.GetMask("Obstaculo")))
            {
                _hayAlgo[i] = true;
            }
            else
            {
                _hayAlgo[i] = false;
            }

        }

    }

    void EnemigoSalta()
    {
        //GENERA LISTA RANDOM DE NUMEROS 1 AL 14
        int[] lugarRandomacheckear = new int[15];
        for (int i = 1; i <= 14; i++)
        {
            int lugarRandom = Random.Range(1, 15);

            while (lugarRandomacheckear.Contains(lugarRandom))
            {
                lugarRandom = Random.Range(1, 15);
            }
            lugarRandomacheckear[i] = lugarRandom;

        }

        //Se mueve al lugar si hay algo ahi, checkea en el orden de los numeros random generados
        for (int i = 1; i <= 14; i++)
        {
            if (_hayAlgo[lugarRandomacheckear[i]] == true)
            {
                switch (lugarRandomacheckear[i])
                {
                    case 1:
                        {
                            _velocity = new Vector2(3.475f, 7.5f);
                            continue;

                        }

                    case 2:
                        {
                            _velocity = new Vector2(6.7f, 7.9f);
                            continue;

                        }

                    case 3:
                        {
                            _velocity = new Vector2(5.35f, 7f);
                            continue;

                        }

                    case 4:
                        {
                            _velocity = new Vector2(3.2f, 5f);
                            continue;

                        }

                    //falta el caso 5

                    case 6:
                        {
                            _velocity = new Vector2(-3.2f, 5f);
                            continue;

                        }

                    case 7:
                        {
                            _velocity = new Vector2(-5.35f, 7f);
                            continue;

                        }

                    case 8:
                        {
                            _velocity = new Vector2(-6.7f, 7.9f);
                            continue;

                        }

                    case 9:
                        {
                            _velocity = new Vector2(-3.475f, 7.9f);
                            continue;

                        }

                    case 10:
                        {
                            _velocity = new Vector2(-4f, 8f);
                            _haySalto2 = 10;
                            continue;

                        }

                    case 11:
                        {
                            _velocity = new Vector2(-3f, 8f);
                            _haySalto2 = 11;
                            continue;

                        }

                    case 12:
                        {
                            _velocity = new Vector2(4f, 8f);
                            _haySalto2 = 12;
                            continue;

                        }

                    case 13:
                        {
                            _velocity = new Vector2(3f, 8f);
                            _haySalto2 = 13;
                            continue;

                        }

                    case 14:
                        {
                            _velocity = new Vector2(4f, 8f);
                            _haySalto2 = 14;
                            continue;

                        }
                }
            }
        }

        //DA EL SEGUNDO SALTO SI SE SETEO EN EL SWITCH ANTERIOR
        if (_velocity.y < 0 && _haySalto2 != 0)
        {
            switch (_haySalto2)
            {
                case 10:
                    {
                        _velocity = new Vector2(-4.8f, 9f);
                        _haySalto2 = 0;
                        break;
                    }

                case 11:
                    {
                        _velocity = new Vector2(-2f, 9f);
                        _haySalto2 = 0;
                        break;
                    }

                case 12:
                    {
                        _velocity = new Vector2(-2.5f, 8f);
                        _haySalto2 = 0;
                        break;
                    }

                case 13:
                    {
                        _velocity = new Vector2(2f, 9f);
                        _haySalto2 = 0;
                        break;
                    }
                case 14:
                    {
                        _velocity = new Vector2(4.8f, 9f);
                        _haySalto2 = 0;
                        break;
                    }
            }
        }

    }

}
