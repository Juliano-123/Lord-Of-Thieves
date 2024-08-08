using UnityEngine;
using System.Collections;
using JetBrains.Annotations;

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

    public Vector2 Move(Vector2 moveAmount, bool tiempoCoyoteON)
    {
        UpdateRaycastOrigins();
        collisions.Reset();

        
                
        HorizontalCollisions(ref moveAmount);

        VerticalCollisions(ref moveAmount);
        

        if (tiempoCoyoteON)
        {
            moveAmount.y = 0;
        }

        transform.Translate(moveAmount);

        return moveAmount;
    }



    void HorizontalCollisions(ref Vector2 velocity)
    {

        if (velocity.x != 0)
        {
            collisions.directionX = (int)Mathf.Sign(velocity.x);
        }

        float directionX = collisions.directionX;
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        if (Mathf.Abs(velocity.x) < skinWidth)
        {
            rayLength = 2 * skinWidth;
        }

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            Vector2 rayOriginReverso = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;

            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            rayOriginReverso += Vector2.up * (horizontalRaySpacing * i);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
            RaycastHit2D hitReverso = Physics2D.Raycast(rayOriginReverso, Vector2.right * -directionX, 2 * skinWidth, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);
            Debug.DrawRay(rayOriginReverso, Vector2.right * -directionX, Color.blue);


            if (hit)
            {
                if (hit.transform.tag == "Piso" && velocity.y > 0)
                {

                }
                else
                {

                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;
                }

                collisions.hayGolpe = true;
                collisions.objetoGolpeado = hit.transform.gameObject;

                collisions.right = directionX == 1;
                collisions.left = directionX == -1;
            }

            if (hitReverso)
            {
                collisions.hayGolpe = true;
                collisions.objetoGolpeado = hitReverso.transform.gameObject;

                collisions.right = directionX == -1;
                collisions.left = directionX == 1;
            }


        }
    }

    void VerticalCollisions(ref Vector2 velocity)
    {
        if (velocity.y != 0)
        {
            collisions.directionY = (int)Mathf.Sign(velocity.y);
        }


        float directionY = collisions.directionY;
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        if (Mathf.Abs(velocity.y) < skinWidth)
        {
            rayLength = 2 * skinWidth;
        }


        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            Vector2 rayOriginReverso = (directionY == -1) ? raycastOrigins.topLeft : raycastOrigins.bottomLeft;

            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            rayOriginReverso += Vector2.right * (verticalRaySpacing * i);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            RaycastHit2D hitReverso = Physics2D.Raycast(rayOriginReverso, Vector2.up * -directionY, 2 * skinWidth, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);
            Debug.DrawRay(rayOriginReverso, Vector2.up * -directionY, Color.blue);


            if (hit)
            {

                if (hit.transform.tag == "Piso" && velocity.y >0)
                {

                }
                else
                {
                    velocity.y = (hit.distance - skinWidth) * directionY;
                    rayLength = hit.distance;
                }

                collisions.hayGolpe = true;
                collisions.objetoGolpeado = hit.transform.gameObject;

                collisions.above = directionY == 1;
                collisions.below = directionY == -1;
            }

            if (hitReverso)
            {
                collisions.hayGolpe = true;
                collisions.objetoGolpeado = hitReverso.transform.gameObject;

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
        public GameObject objetoGolpeado;
        public int directionX;
        public int directionY;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            hayGolpe = false;
            objetoGolpeado = null;
        }
    }

}
