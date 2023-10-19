//namespace Pong.GamePlayer;
using Pong.GamePlayer;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pong.Physics;

using static Pong.GameCache;
using static Pong.GameHelpers;

namespace Pong.GamePlayer {
    public partial class PlayerController : MonoBehaviour
    {
        // parameters of trajectory; x velocity and beyond will ALWAYS be 0. For the sake of simplicity, any derivative beyond acceleration will be 0
        // used to track motion rather than determine it
        // contains base viewport velocity (Vector2) and float[] yAccelerationAndBeyond in terms of viewport percentage y
        private readonly Motion2D viewportMotion = new Motion2D(); // this tracks motion, rather than controlling it

        public PlayerCommandSensors commandSensors = new PlayerCommandSensors();

        // Start is called before the first frame update
        void Start()
        {
            // Start
        }

        // Update is called once per frame
        void Update()
        {
            //? put any frame-dependent updates here
        }

        // Time-dependent updates (such as physics)
        void FixedUpdate() {
            // Don't respond to a command if it is just nothing!
            float deltaY = commandSensors.Do_Nothing ? 0f : RespondToCommand(Time.fixedDeltaTime);

            //* Track Motion
            // calculate velocity at this frame
            Vector3 deltaPos = new Vector3(0f, deltaY, 0f);
            Vector2 viewportVelocity = ToViewport(deltaPos) / Time.fixedDeltaTime;

            // calculate acceleration at this frame
            float deltaYVelocity = viewportVelocity.y - viewportMotion.velocity.y;
            float viewportYAcceleration = deltaYVelocity / Time.fixedDeltaTime;

            // set the new velocity and acceleration
            viewportMotion.velocity.Set(viewportVelocity.x, viewportVelocity.y);
            viewportMotion.Y_Acceleration = viewportYAcceleration;
        }

        protected float RespondToCommand(float dt) { // dt = delta_time
            float dy = 0f;

            if (commandSensors.Move_Up) {
                dy += DeltaY(dt);
            }

            if (commandSensors.Move_Down) {
                dy += -DeltaY(dt);
            }

            //* Now that all the movement updates have been collected, time to apply them
            MoveY(dy);

            // return the change in y
            return dy;
        }

        protected float DeltaY(float dt) {
            // ToLocal() but with a single value rather than a whole ass vector
            float dy_dt = PLAYER_SPEED_VP * BG_TRANSFORM.localScale.y;
            
            return dy_dt * dt;
        }

        public void MoveY(float deltaY) {
            // origin is in the center
            float MAX_Y_POS = BG_TRANSFORM.localScale.y / 2f;
            float halfPaddleLength = transform.localScale.y / 2f;
            
            float topEdgeY = transform.localPosition.y + halfPaddleLength;
            float bottomEdgeY = transform.localPosition.y - halfPaddleLength;

            if (topEdgeY + deltaY > MAX_Y_POS || bottomEdgeY + deltaY < -MAX_Y_POS) { // not allowed to move out of bounds
                return;
            }

            // moving will not take it out of bounds
            transform.localPosition += new Vector3(0f, deltaY, 0f);
        }

        public Motion2D GetViewportMotionTracker() { return viewportMotion; }
    }
}