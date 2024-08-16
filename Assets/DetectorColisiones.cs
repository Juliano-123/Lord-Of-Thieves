using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RayCastController;

public class DetectorColisiones : RayCastController
{
    [SerializeField]
    public EnemyCollisionInfo enemigos;

    [SerializeField]
    LayerMask layerEnemigos;
    float rayLength = 6 * skinWidth;

    public override void Start()
    {
        base.Start();
        enemigos.directionX = 1;
        enemigos.directionY = -1;
    }



    public void CheckEnemigos()
    {
        UpdateRaycastOrigins();
        enemigos.Reset();

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOriginDerecha = raycastOrigins.bottomRight;
            Vector2 rayOriginIzquierda = raycastOrigins.bottomLeft;

            rayOriginDerecha += Vector2.up * (horizontalRaySpacing * i);
            rayOriginIzquierda += Vector2.up * (horizontalRaySpacing * i);

            RaycastHit2D hitDerecha = Physics2D.Raycast(rayOriginDerecha, Vector2.right, rayLength, layerEnemigos);
            RaycastHit2D hitIzquierda = Physics2D.Raycast(rayOriginIzquierda, Vector2.left, rayLength, layerEnemigos);

            Debug.DrawRay(rayOriginDerecha, new Vector3(rayLength, 0, 0), Color.blue);
            Debug.DrawRay(rayOriginIzquierda, new Vector3(-rayLength, 0, 0), Color.blue);



            if (hitDerecha)
            {

                //DETECTAR COLISIONES EN EL BORDE
                if (i <= horizontalRayCount / 10)
                {
                    enemigos.edge = true;
                }
                else
                {
                    enemigos.edge = false;
                }

                enemigos.hayGolpe = true;
                enemigos.objetoGolpeadoHorizontal = hitDerecha.transform.gameObject;
                enemigos.right = true;
            }

            if (hitIzquierda)
            {
                //DETECTAR COLISIONES EN EL BORDE
                if (i <= horizontalRayCount / 10)
                {
                    enemigos.edge = true;
                }
                else
                {
                    enemigos.edge = false;
                }


                enemigos.hayGolpe = true;
                enemigos.objetoGolpeadoHorizontal = hitIzquierda.transform.gameObject;
                enemigos.left = true;
            }
        }

        

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOriginArriba = raycastOrigins.topLeft;
            Vector2 rayOriginAbajo = raycastOrigins.bottomLeft;

            rayOriginArriba += Vector2.right * (verticalRaySpacing * i);
            rayOriginAbajo += Vector2.right * (verticalRaySpacing * i);

            RaycastHit2D hitArriba = Physics2D.Raycast(rayOriginArriba, Vector2.up, rayLength, layerEnemigos);
            RaycastHit2D hitAbajo = Physics2D.Raycast(rayOriginAbajo, Vector2.down, rayLength, layerEnemigos);

            Debug.DrawRay(rayOriginArriba, new Vector3(0, rayLength, 0), Color.blue);
            Debug.DrawRay(rayOriginAbajo, new Vector3(0, -rayLength, 0), Color.blue);


            if (hitArriba)
            {
                enemigos.hayGolpe = true;
                enemigos.objetoGolpeadoVertical = hitArriba.transform.gameObject;
                enemigos.above = true;
            }

            if (hitAbajo)
            {
                enemigos.hayGolpe = true;
                enemigos.objetoGolpeadoVertical = hitAbajo.transform.gameObject;
                enemigos.below = true;
            }

        }
    }

    public struct EnemyCollisionInfo
    {
        public bool above, below;
        public bool left, right;
        public bool hayGolpe;
        public bool edge;
        public GameObject objetoGolpeadoHorizontal;
        public GameObject objetoGolpeadoVertical;
        public int directionX;
        public int directionY;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            hayGolpe = false;
            edge = false;
            objetoGolpeadoHorizontal = null;
            objetoGolpeadoVertical = null;
        }
    }
}
