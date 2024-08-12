using UnityEngine;


public class Controller2D : RayCastController
{
    [SerializeField]
    public CollisionInfo collisions;


    public override void Start()
    {
        base.Start();
        collisions.directionX = 1;
        collisions.directionY = -1;
    }

    public Vector2 Move(Vector2 moveAmount, bool tiempoCoyoteON, LayerMask layermask)
    {
        UpdateRaycastOrigins();
        collisions.Reset();

        
                
        HorizontalCollisions(ref moveAmount, layermask);

        VerticalCollisions(ref moveAmount, layermask);
        

        if (tiempoCoyoteON && moveAmount.y < 0)
        {
            moveAmount.y = 0;
        }

        transform.Translate(moveAmount);

        return moveAmount;
    }



    void HorizontalCollisions(ref Vector2 velocity, LayerMask layermask)
    {

        if (velocity.x != 0)
        {
            collisions.directionX = (int)Mathf.Sign(velocity.x);
        }

        float directionX = collisions.directionX;
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        if (Mathf.Abs(velocity.x) < skinWidth)
        {
            rayLength = 3 * skinWidth;
        }


        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            Vector2 rayOriginReverso = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;

            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            rayOriginReverso += Vector2.up * (horizontalRaySpacing * i);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, layermask);
            RaycastHit2D hitReverso = Physics2D.Raycast(rayOriginReverso, Vector2.right * -directionX, 3 * skinWidth, layermask);

            Debug.DrawRay(rayOrigin, new Vector3(rayLength * directionX,0,0), Color.red);
            Debug.DrawRay(rayOriginReverso, new Vector3(3 * skinWidth * -directionX,0,0), Color.blue);



            if (hit)
            {
                //PARA ATRAVESAR PALATAFORMAS
                if (hit.distance == 0 && hit.transform.tag == "Plataforma" || hit.transform.tag == "Plataforma")
                {
                    continue;
                }

                //DETECTAR COLISIONES EN EL BORDE
                if (i <= horizontalRayCount/10)
                {
                    collisions.edge = true;
                }
                else
                {
                    collisions.edge = false;
                }


                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;                

                collisions.hayGolpe = true;
                collisions.objetoGolpeadoHorizontal = hit.transform.gameObject;

                collisions.right = directionX == 1;
                collisions.left = directionX == -1;
            }

            if (hitReverso)
            {
                //DETECTAR COLISIONES EN EL BORDE
                if (i <= horizontalRayCount / 10)
                {
                    collisions.edge = true;
                }
                else
                {
                    collisions.edge = false;
                }


                collisions.hayGolpe = true;
                collisions.objetoGolpeadoHorizontal = hitReverso.transform.gameObject;

                collisions.right = directionX == -1;
                collisions.left = directionX == 1;
            }


        }
    }

    void VerticalCollisions(ref Vector2 velocity, LayerMask layermask)
    {
        if (velocity.y != 0)
        {
            collisions.directionY = (int)Mathf.Sign(velocity.y);
        }


        float directionY = collisions.directionY;
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        if (Mathf.Abs(velocity.y) < skinWidth)
        {
            rayLength = 3 * skinWidth;
        }


        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            Vector2 rayOriginReverso = (directionY == -1) ? raycastOrigins.topLeft : raycastOrigins.bottomLeft;

            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            rayOriginReverso += Vector2.right * (verticalRaySpacing * i);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, layermask);
            RaycastHit2D hitReverso = Physics2D.Raycast(rayOriginReverso, Vector2.up * -directionY, 3 * skinWidth, layermask);

            Debug.DrawRay(rayOrigin, new Vector3(0,rayLength * directionY,0), Color.red);
            Debug.DrawRay(rayOriginReverso, new Vector3(0,3 * skinWidth * -directionY,0), Color.blue);

            if (hit.distance == 0 || hit.transform.tag == "Plataforma" && velocity.y > 0)
            {
                continue;
            }


            if (hit)
            {

                if ((hit.distance == 0 && hit.transform.tag == "Plataforma") || (hit.transform.tag == "Plataforma" && velocity.y > 0))
                {
                    continue;
                }

                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;
             
                collisions.hayGolpe = true;
                collisions.objetoGolpeadoVertical = hit.transform.gameObject;

                collisions.above = directionY == 1;
                collisions.below = directionY == -1;
            }

            if (hitReverso)
            {
                collisions.hayGolpe = true;
                collisions.objetoGolpeadoVertical = hitReverso.transform.gameObject;

                collisions.above = directionY == -1;
                collisions.below = directionY == 1;
            }

        }

    }


    public struct CollisionInfo
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
