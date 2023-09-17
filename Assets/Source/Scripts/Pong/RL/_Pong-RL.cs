// * NAMESPACE HEADER FILE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pong.RL {
    public static class Constants {
        // Observation Constants: Observation Indices (not all of the possible ones, but the ones that include pairs)
        public const int PLAYER_POS_Y = 0, OPPONENT_POS_Y = 1;
        public const int BALL_RELATIVE_POS_X = 2, BALL_RELATIVE_POS_Y = 3;
        public const int BALL_RELATIVE_VELOCITY_X = 4, BALL_RELATIVE_VELOCITY_Y = 5;
        public const int BALL_RELATIVE_FURTHER_DERIVATIVES_START = 6;

        // Action Constants
        public const float ACTION_NULL = 0f;

        // Reward Constants
        public const float REWARD_NULL = 0.0f;
    }

    public partial class DataUnit {}
}