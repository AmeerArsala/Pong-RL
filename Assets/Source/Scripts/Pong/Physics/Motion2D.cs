//namespace Pong.Physics;
using Pong.Physics;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pong;

namespace Pong.Physics {
    public partial class Motion2D {
        public Vector2 velocity = new Vector2(0f, 0f);
        public readonly float[] yAccelerationAndBeyond = new float[GameConstants.BALL_Y_MAX_DERIVATIVE - 1];

        public Motion2D(Vector2 velocity2f) {
            velocity.Set(velocity2f.x, velocity2f.y);
            ResetYAccelerationAndBeyond();
        }

        public Motion2D() {
            ResetYAccelerationAndBeyond();
        }

        public float Y_Acceleration {
            get { return yAccelerationAndBeyond[0]; }
            set { yAccelerationAndBeyond[0] = value; }
        }

        private void ResetYAccelerationAndBeyond() {
            // initialize with zeros
            for (int i = 0; i < yAccelerationAndBeyond.Length; ++i) {
                yAccelerationAndBeyond[i] = 0;
            }
        }

        // stop the motion by zeroing it out
        public void ZeroOut() {
            velocity.Set(0f, 0f);
            ResetYAccelerationAndBeyond();
        }

        public Vector2 CalculateTotalVelocity(float t) {
            Vector2 totalVelocity = new Vector2(velocity.x, velocity.y);

            for (int i = 0; i < yAccelerationAndBeyond.Length; ++i) {
                float coefficient = yAccelerationAndBeyond[i];

                int derivativeNumber = i + 2;
                float termIndex = derivativeNumber - 1; // yes, this is a float and is equal to the nth derivative - 1

                // only y changes; x changing like that would be very stupid
                totalVelocity.y += coefficient * (Mathf.Pow(t, termIndex) / termIndex);
            }

            return totalVelocity;
        }

        // [(x, y), (x', y'), (x'', y''), ...]
        public Vector2[] RetrieveTrajectory(Vector2 position) {
            Vector2[] trajectory = new Vector2[2 + yAccelerationAndBeyond.Length]; // position and velocity are included too!

            trajectory[0] = position;
            trajectory[1] = velocity;

            for (int i = 0; i < yAccelerationAndBeyond.Length; ++i) {
                int i_trajectory = i + 2;
                trajectory[i_trajectory] = new Vector2(0.0f, yAccelerationAndBeyond[i]); // there is no x acceleration and beyonod, that would be very stupid
            }

            return trajectory;
        }
    }
}