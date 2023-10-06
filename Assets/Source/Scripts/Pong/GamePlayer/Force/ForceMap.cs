//namespace Pong.GamePlayer.Force;
using Pong.GamePlayer.Force;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pong;
using Pong.Physics;
using static Pong.GameHelpers; 

namespace Pong.GamePlayer.Force {
    //* This is relative to the "frontPoint" on the paddle where y is in the center but x is all the way on the forward side
    public partial class ForceMap {
        public const float DEFAULT_NORMAL_START_ANGLE = 0f;
        public const float DEFAULT_NORMAL_END_ANGLE = Mathf.PI;
        
        private readonly Transform paddleTransform; // used to calculate the frontPoint

        //* Angle scale is centered around [0, PI] where 0 and PI represent the ends of the paddle and everything in between is the straight portion of the paddle
        private float normalStartAngle, normalEndAngle; // normal angle range => multiplied trajectory (direction is simply redirected)

        // for angles in the ABNORMAL range (not normal), they completely modify the trajectory of the ball when the force is applied
        private float paddleVelocity = 0f;     // in terms of viewport y % 
        private float paddleAcceleration = 0f; // in terms of viewport y %

        public ForceMap(Transform paddleTransform, float normalStartAngle, float normalEndAngle) {
            this.paddleTransform = paddleTransform;
            this.normalStartAngle = normalStartAngle;
            this.normalEndAngle = normalEndAngle;
        }

        public ForceMap(Transform paddleTransform) {
            this.paddleTransform = paddleTransform;
            normalStartAngle = DEFAULT_NORMAL_START_ANGLE;
            normalEndAngle = DEFAULT_NORMAL_END_ANGLE;
        }

        public Vector2 FindFrontPoint() {
            Vector2 frontPoint = ToVector2(paddleTransform.localPosition);

            int facingDirection = paddleTransform.localPosition.x > 0 ? -1 : 1;
            float halfPaddleThickness = paddleTransform.localScale.x / 2f;
            
            frontPoint.x += facingDirection * halfPaddleThickness;

            return frontPoint;
        }

        public float PaddleVelocity {
            get { return paddleVelocity; }
            set { paddleVelocity = value; }
        }

        public float PaddleAcceleration {
            get { return paddleAcceleration; }
            set { paddleAcceleration = value; }
        }

        public (float, float) NormalAngleRange {
            get { return (normalStartAngle, normalEndAngle); }
            set {
                (float startAngle, float endAngle) = value;
                normalStartAngle = startAngle;
                normalEndAngle = endAngle;
            }
        }

        public bool IsNormalAngle(float angle, bool angleIsRelativized) {
            float relativizedAngle = angleIsRelativized ? angle : RelativizeAngle(angle);

            return relativizedAngle >= normalStartAngle && relativizedAngle <= normalEndAngle;
        }
        
        // returns whether it was abnormal
        public bool ApplyAt(Vector2 pointOfContact, Motion2D ballMotion) {
            Vector2 frontPoint = FindFrontPoint();
            Vector2 frontPointToPointOfContact = pointOfContact - frontPoint;

            float relativeAngle = CalculateRelativeAngle(frontPointToPointOfContact);

            if (IsNormalAngle(relativeAngle, angleIsRelativized: true)) {
                //* Normal; Just bounce
                ballMotion.velocity.x *= -1;

                return false;
            } else {
                //* Abnormal; Modify whole trajectory
                //? Maybe change so that X is also completely modified
                ballMotion.velocity.x *= -1;

                // y
                float paddleAbsoluteForce = Mathf.Abs(GameConstants.PADDLE_MASS * paddleAcceleration);
                float paddleSpeed = Mathf.Abs(paddleVelocity);
                ballMotion.velocity.y = Mathf.Sign(paddleVelocity) * (Mathf.Max(Mathf.Abs(ballMotion.velocity.y), paddleSpeed) + paddleAbsoluteForce);
                ballMotion.Y_Acceleration = paddleAcceleration;

                return true;
            }
        }

        public static float CalculateRelativeAngle(Vector2 frontPointToPointOfContact) {
            Vector2 relativeVec = new Vector2(frontPointToPointOfContact.x, frontPointToPointOfContact.y);
            
            // put everything within [-PI/2, PI/2] for now
            relativeVec.x = Mathf.Abs(relativeVec.x);

            // I COULD just use Mathf.Atan2(relativeVec.y, relativeVec.x), but this is more readable atm
            float tan_relativeAngle = (relativeVec.y / relativeVec.x);
            float relativeAngle = Mathf.Atan(tan_relativeAngle);

            // put on the same scale of [0, PI]
            relativeAngle += Mathf.PI / 2f;

            return relativeAngle;
        }

        public static float RelativizeAngle(float angle) {
            return CalculateRelativeAngle(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));
        }
    }
}