// * NAMESPACE HEADER FILE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pong;

namespace Pong.RL {
    public static class Constants {
        // Spaces
        public static readonly uint OBSERVATION_SPACE = Calculate_STATE_SPACE();
        public const uint ACTION_SPACE = 3; // move up, move down, or do nothing

        // Observation Constants: Observation Indices (not all of the possible ones, but the ones that include pairs)
        public const uint PLAYER_POS_Y = 0, OPPONENT_POS_Y = 1;
        public const uint BALL_RELATIVE_POS_X = 2, BALL_RELATIVE_POS_Y = 3;
        public const uint BALL_RELATIVE_VELOCITY_X = 4, BALL_RELATIVE_VELOCITY_Y = 5;
        public const uint BALL_RELATIVE_FURTHER_DERIVATIVES_START = 6;

        // Action Constants
        public const float ACTION_NULL = 0f;
        public const float ACTION_MOVE_UP = 1f;
        public const float ACTION_MOVE_DOWN = 2f;

        // Reward Constants
        public const float REWARD_NULL = 0.0f;
        public const float REWARD_DEFEND = 1.0f;

        // Calculate Observation Space
        // In order
        public static uint Calculate_STATE_SPACE() {
            //* Players
            // Player.Y, Opponent.Y :: observation[0], observation[1]
            uint playerParams = 2;

            //* Ball
            // Relative Ball Position(X, Y) :: (observation[2], observation[3])
            uint relative_ballPosParams = 2;

            // Relative Ball Velocity(X, Y) :: (observation[4], observation[5])
            uint relative_ballVelocityParams = 2;

            // {Relative Ball Motion Beyond Velocity}(Y) :: {observation[6], ... , observation[observation.Length - 1]}
            uint yAccelerationAndBeyond_Length = GameConstants.BALL_Y_MAX_DERIVATIVE - 1;

            //* Sum
            // = 5 + BALL_Y_MAX_DERIVATIVE
            return (playerParams + relative_ballPosParams + relative_ballVelocityParams + yAccelerationAndBeyond_Length);
        }
    }

    public partial class DataUnit {}
}