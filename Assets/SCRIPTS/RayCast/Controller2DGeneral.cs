using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2DGeneral : RayCastController
{
    [SerializeField]
    public CollisionInfo collisions;


    public override void Start()
    {
        base.Start();
        collisions.facedir = 1;
    }

    public Vector2 Move(Vector2 moveAmount, bool tiempoCoyoteON)
    {
        UpdateRaycastOrigins();
        collisions.Reset();

        if (moveAmount.x != 0)
        {
            collisions.facedir = (int)Mathf.Sign(moveAmount.x);
        }

        HorizontalCollisions(ref moveAmount);

        if (moveAmount.y != 0)
        {
            VerticalCollisions(ref moveAmount);
        }

        if (tiempoCoyoteON)
        {
            moveAmount.y = 0;
        }

        transform.Translate(moveAmount);

        return moveAmount;
    }



    void HorizontalCollisions(ref Vector2 velocity)
    {
        float directionX = collisions.facedir;
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
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

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
        public GameObject objetoGolpeadoHorizontal;
        public GameObject objetoGolpeadoVertical;
        public int facedir;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            objetoGolpeadoHorizontal = null;
            objetoGolpeadoVertical = null;
        }
    }

}