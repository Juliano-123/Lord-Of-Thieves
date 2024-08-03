using UnityEngine;
using System.Collections;

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
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

            if (hit)
            {
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
                collisions.objetoGolpeadoHorizontal = hit.transform.gameObject;
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
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
                collisions.objetoGolpeadoVertical = hit.transform.gameObject;
            }
        }

    }


    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;
        public GameObject objetoGolpeadoDerecha;
        public GameObject objetoGolpeadoIzquierda;
        public GameObject objetoGolpeadoArriba;
        public GameObject objetoGolpeadoAbajo;
        public int directionX;
        public int directionY;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            objetoGolpeadoHorizontal = null;
            objetoGolpeadoVertical = null;
        }
    }

}
