//namespace Pong.Ball;
using Pong.Ball;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pong;
using Pong.GamePlayer;
using Pong.Physics;
using static Pong.GameHelpers;
using static Pong.GameCache;

namespace Pong.Ball {
    //* After scoring, it goes: DestroyBall() -> [tiny delay] -> Reset() -> [small delay] -> Serve()
    public partial class PongBall {
        public readonly GameObject sprite; // it won't actually be destroyed; it will just vanish and look like it was destroyed
        private readonly PongBallController controller;
        private readonly Stack<(float, int)> serveAngles = new Stack<(float, int)>(); // float is in radians, and the int is the attackerDesire

        // Player on the offensive
        private Player attacker; // "lastTouchedBy"; the initial trajectory will also set this as the player opposite to where it is traveling
        private int attackerDesire = BallStatus.NO_GOAL;

        public PongBall(GameObject ballSprite) {
            sprite = ballSprite;

            // add + initialize controller
            controller = sprite.AddComponent<PongBallController>();

            // collision detection
            RectangularBodyFrame bodyFrame = sprite.AddComponent<RectangularBodyFrame>();
        }

        public static PongBall FromPrefab(GameObject prefab) {
            GameObject ballSprite = GameManager.Instantiate(prefab, GetStartLocalPosition(), Quaternion.identity);

            PongBall pongBall = new PongBall(ballSprite);
            pongBall.SetLocalScaleFromVPY(GameConstants.BALL_SCALE_Y);

            return pongBall;
        }

        public void Initialize(Player server) {
            // First, reset the ball (just in case)
            Reset();

            // Handle which server this is
            bool serverIsToLeft = server.sprite.transform.localPosition.x < GetStartLocalPosition().x;

            // initialize status
            attackerDesire = serverIsToLeft ? BallStatus.GOAL_RIGHT : BallStatus.GOAL_LEFT; 

            // either 0 or 1, depending on whether it is even or odd respectively
            // if first server (even, index 0) is to the left, the remaining odd servers will be to the right and therefore have the ball traveling left on serve
            // if first server is to the right, all the rest of the even servers on the right will have the ball traveling left on serve. the remaining odd servers will be to the left, and the ball 
            uint playerFactor = (uint)(serverIsToLeft ? 1 : 0);

            // initialize serveAngles
            uint maxRounds = (WIN_SCORE) + (WIN_SCORE - 1);
            for (uint i = 0; i < maxRounds; ++i) {
                float angle = Random.Range(-BALL_SERVE_MAX_ANGLE, BALL_SERVE_MAX_ANGLE);
                int desire = BallStatus.GOAL_RIGHT; // assumes the server is on the left; doesn't matter if wrong bc it will get changed in the following if statement

                // (i % 2 == 1) => first server is to the left => the right server is on odd rounds to serve left
                // (i % 2 == 0) => first server is to the right => the right server is on even rounds to serve left
                if (i % 2 == playerFactor) { // if odd/even, add PI so that it goes on the left side 
                    angle += Mathf.PI;
                    desire = BallStatus.GOAL_LEFT;
                }

                Debug.Log(angle);
                serveAngles.Push((angle, desire));
                
                //TODO: debug
                if (i == 10) {
                    break;
                }
            }

            // the Player serving is the one on the offensive
            SetAttacker(server);
        }

        // serve the ball
        public void Serve() {
            (float angle, int serverDesire) = serveAngles.Pop();
            float speed = BALL_SPEED_VP; // in terms of viewport x percentage

            bool otherPlayerServingInstead = attackerDesire != serverDesire;
            if (otherPlayerServingInstead) {
                SwapAttacker();
            }

            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)); // unit vector
            Vector2 viewportVelocity = speed * direction;

            controller.ViewportVelocity = viewportVelocity; // set velocity
            controller.BeginTrajectory(); // start the timer for y'(t)
        }

        public void Update() {
            bool attackerScoredGoal = controller.GetBallStatus() != BallStatus.NO_GOAL && controller.GetBallStatus() == attackerDesire;
            if (attackerScoredGoal) {
                //* Attacker Scored Goal
                //Debug.Log(controller.GetBallStatus());

                DestroyBall();

                attacker.ScorePoint();

                //TODO: [tiny delay]
                Reset();
                //TODO: [small delay]
                Serve();
            } else if (controller.GetBallStatus() == BallStatus.REBOUNDED) {
                //* Ball was Rebounded by the defender
                controller.ResetBallStatus();
                controller.SetFreeze(false); // unfreeze
                SwapAttacker();
            }

            //TODO: feed to players?
        }

        public void DestroyBall() {
            controller.HaltTrajectory(); // stop the ball from going off the screen
            sprite.SetActive(false);
        }

        public void Reset() {
            // activate
            sprite.SetActive(true);

            // set position to start position
            sprite.transform.localPosition = GetStartLocalPosition();
        }

        private void SetAttacker(Player atkr) {
            attacker = atkr;

            // Now that the attacker has been set, the ball will be headed towards the "rebounder", or in other words, the other player
            controller.Rebounder = attacker.Opponent.AsRebounder();
        }

        private void SwapAttacker() {
            SetAttacker(attacker.Opponent);
            attackerDesire = BallStatus.INVERT(attackerDesire);
        }

        public void SetLocalScaleFromVPY(float viewportY) {
            Vector3 bgScale = BG_TRANSFORM.localScale;

            sprite.transform.localScale = new Vector3(
                viewportY * bgScale.y, // square
                viewportY * bgScale.y, // square
                sprite.transform.localScale.z
            );

            //Debug.Log("LocalScale: " + sprite.transform.localScale);
        }

        public static Vector3 GetStartLocalPosition() {
            return ToLocal(GameConstants.BALL_START_POSITION);
        }
    }
}