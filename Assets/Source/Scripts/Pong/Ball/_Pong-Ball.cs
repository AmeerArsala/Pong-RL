// * NAMESPACE HEADER FILE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pong;

namespace Pong.Ball {
    /*
     * Player lastTouchedBy
     * Destroys the ball and increments points
    */
    public partial class PongBall {}

    /*
     * Handle collisions -> adjust trajectory
       speedX = GameCache.BALL_SPEED_VP
       velocityY = ForceAdjustment => Equation (due to possible derivatives) 
    */
    public partial class PongBallController : MonoBehaviour {}

    public static class BallStatus {
        public const int GOAL_LEFT = -1;
        public const int NO_GOAL = 0;
        public const int GOAL_RIGHT = 1;

        public static int INVERT(int ballStatus) {
            return ballStatus * -1;
        }
    }
}