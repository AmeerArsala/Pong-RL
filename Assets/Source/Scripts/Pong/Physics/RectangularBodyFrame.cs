//namespace Pong.Physics;
using Pong.Physics;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pong;
using static Pong.GameHelpers;

namespace Pong.Physics {
    //* This class assumes that the center of mass is in the center of the GameObject
    public partial class RectangularBodyFrame : MonoBehaviour
    {
        // the literal local dimensions, not viewport
        public Vector2 halfBodyDimensions = new Vector2(0f, 0f); // because origin is in the center of the screen in this game
        
        public float leftEdgeX = 0f, rightEdgeX = 0f;
        public float topEdgeY = 0f, bottomEdgeY = 0f;

        private int collisionState = CollisionState.NONE;

        private Vector2 HalfBodyDimensions {
            get { return halfBodyDimensions; }
            set { halfBodyDimensions.Set(value.x, value.y); }
        }

        // Start is called before the first frame update
        void Start()
        {
            HalfBodyDimensions = ToVector2(transform.localScale) / 2f;
        }

        // Update is called once per frame
        void Update()
        {
            // Since it is just setting dimensions, this is frame-dependent

            HalfBodyDimensions = ToVector2(transform.localScale) / 2f;

            // origin is in the center
            leftEdgeX = transform.localPosition.x - halfBodyDimensions.x;
            rightEdgeX = transform.localPosition.x + halfBodyDimensions.x;
            topEdgeY = transform.localPosition.y + halfBodyDimensions.y;
            bottomEdgeY = transform.localPosition.y - halfBodyDimensions.y;
        }

        public void ResetState() {
            collisionState = CollisionState.NONE;

            leftEdgeX = 0f;
            rightEdgeX = 0f;
            topEdgeY = 0f;
            bottomEdgeY = 0f;
        }

        //* This will only be correct if there IS a collision to begin with. Otherwise, it is just a guess that isn't CollisionState.NONE
        public int MostLikelyCollisionState(RectangularBodyFrame other) {
            // Horizontal Losses
            float leftEdge = Mathf.Abs(leftEdgeX - other.rightEdgeX);
            float rightEdge = Mathf.Abs(other.leftEdgeX - rightEdgeX);

            // Vertical Losses
            float topEdge = Mathf.Abs(other.bottomEdgeY - topEdgeY);
            float bottomEdge = Mathf.Abs(bottomEdgeY - other.topEdgeY);

            // Candidate Selection
            float bestHorizontal = Mathf.Min(leftEdge, rightEdge);
            float bestVertical = Mathf.Min(topEdge, bottomEdge);

            if (bestHorizontal <= bestVertical) {
                return CollisionState.HORIZONTAL;
            } else { // bestVertical < bestHorizontal
                return CollisionState.VERTICAL;
            }
        }

        public bool CollidesWith(RectangularBodyFrame other) {
            // Eliminate Horizontal Possibilities
            if (rightEdgeX < other.leftEdgeX || leftEdgeX > other.rightEdgeX) {
                SetCollisionStates(CollisionState.NONE, other);
                return false;
            }

            // Eliminate Vertical Possibilities
            if (bottomEdgeY > other.topEdgeY || topEdgeY < other.bottomEdgeY) {
                SetCollisionStates(CollisionState.NONE, other);
                return false;
            }

            // X and Y overlap
            SetCollisionStates(MostLikelyCollisionState(other), other);
            return true;
        }

        //* ONLY call this once CollidesWith() verifies that there IS a collision point
        //* Therefore, this assumes that one exists
        public Vector2 CollisionPoint(RectangularBodyFrame other) {
            Vector2 collisionPoint = new Vector2(0f, 0f);

            switch (collisionState) {
                case CollisionState.HORIZONTAL:
                    collisionPoint.x = leftEdgeX <= other.rightEdgeX ? leftEdgeX : rightEdgeX;
                    
                    // y = avg(min of the top, max of the bottom)
                    collisionPoint.y = (Mathf.Min(topEdgeY, other.topEdgeY) + Mathf.Max(bottomEdgeY, other.bottomEdgeY)) / 2f;
                    break;
                case CollisionState.VERTICAL:
                    // x = avg(min of the rights, max of the lefts)
                    collisionPoint.x = (Mathf.Min(rightEdgeX, other.rightEdgeX) + Mathf.Max(leftEdgeX, other.leftEdgeX)) / 2f;

                    collisionPoint.y = topEdgeY >= other.bottomEdgeY ? topEdgeY : bottomEdgeY;
                    break;
                default:
                    // no collision occurred
                    break;
            }

            return collisionPoint;
        }

        private void SetCollisionStates(int state, RectangularBodyFrame other) {
            collisionState = state;
            other.collisionState = state;
        }
    }
}