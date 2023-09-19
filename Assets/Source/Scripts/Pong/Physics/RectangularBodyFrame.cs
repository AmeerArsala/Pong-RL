//namespace Pong.Physics;
using Pong.Physics;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pong;
using static Pong.GameHelpers;

//* This class assumes that the center of mass is in the center of the GameObject
public class RectangularBodyFrame : MonoBehaviour
{
    // the literal local dimensions, not viewport
    public readonly Vector2 halfBodyDimensions = new Vector2(0f, 0f); // because origin is in the center of the screen in this game
    
    public float leftEdgeX, rightEdgeX;
    public float topEdgeY, bottomEdgeY;

    private int boundedState = BoundState.UNBOUNDED;

    // Start is called before the first frame update
    void Start()
    {
        halfBodyDimensions = ToVector2(transform.localScale) / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        halfBodyDimensions = ToVector2(transform.localScale) / 2f;

        // origin is in the center
        leftEdgeX = transform.localPosition.x - halfBodyDimensions.x;
        rightEdgeX = transform.localPosition.x + halfBodyDimensions.x;
        topEdgeY = transform.localPosition.y + halfBodyDimensions.y;
        bottomEdgeY = transform.localPosition.y - halfBodyDimensions.y;
    }

    private bool CollidesWithLargerFrame(RectangularBodyFrame other) {
        // Horizontal Collisions
        (float otherBottomEdgeY, float otherTopEdgeY) otherYBounds = (other.bottomEdgeY, other.topEdgeY);
        if (BoundedBetween(topEdgeY, otherYBounds, BoundState.TOP_BOUNDED) || BoundedBetween(bottomEdgeY, otherYBounds, BoundState.BOTTOM_BOUNDED)) {
            return leftEdgeX == other.rightEdgeX || rightEdgeX == other.leftEdgeX;
        }

        // Vertical Collisions
        (float otherLeftEdgeX, float otherRightEdgeX) otherXBounds = (other.leftEdgeX, other.rightEdgeX);
        if (BoundedBetween(leftEdgeX, otherXBounds, BoundState.LEFT_BOUNDED) || BoundedBetween(rightEdgeX, otherXBounds, BoundState.RIGHT_BOUNDED)) {
            return topEdgeY == other.bottomEdgeY || bottomEdgeY == other.topEdgeY;
        }

        boundedState = BoundState.UNBOUNDED;
        return false;
    }

    public bool CollidesWith(RectangularBodyFrame other) {
        // we don't know which is larger
        // this will short circuit too if the first is true
        bool collidesWith = CollidesWithLargerFrame(other) || other.CollidesWithLargerFrame(this);
        boundedState = collidesWith ? -other.boundedState : BoundState.UNBOUNDED;

        return collidesWith;
    }

    //* ONLY call this once CollidesWith verifies that there IS a collision point
    //* Therefore, this assumes that one exists
    public Vector2 CollisionPoint(RectangularBodyFrame other) {
        Vector2 collisionPoint = new Vector2(0f, 0f);

        if (BoundState.CollisionIsHorizontal(boundedState)) {
            // Horizontal Collisions
            collisionPoint.x = leftEdgeX == other.rightEdgeX ? leftEdgeX : rightEdgeX;

            // y = avg(min of the top, max of the bottom)
            collisionPoint.y = (Mathf.Min(topEdgeY, other.topEdgeY) + Mathf.Max(bottomEdgeY, other.bottomEdgeY)) / 2f;
        } else {
            // Vertical Collisions
            // x = avg(min of the rights, max of the lefts)
            collisionPoint.x = (Mathf.Min(rightEdgeX, other.rightEdgeX) + Mathf.Max(leftEdgeX, other.leftEdgeX)) / 2f;
            collisionPoint.y = topEdgeY == other.bottomEdgeY ? topEdgeY : bottomEdgeY;
        }

        return collisionPoint;
    }

    private bool BoundedBetween(float value, (float start, float end) bounds, int boundState) {
        boundedState = boundState;
        return value >= start && value <= end;
    }
}
