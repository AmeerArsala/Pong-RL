//namespace Pong.GamePlayer;
using Pong.GamePlayer;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pong.RL;
using Pong.Physics;

namespace Pong.GamePlayer {
    /*  For Pong RL, we will only care about the statistics of:
        *   Y position of Paddle
        *   Ball's relative distance: vec2[X distance away (absolute value) from goal, Y distance away from Paddle]
        *   Ball's velocity: vec2[X speed towards goal (absolute value), Y]
        ?   Observed Y position of the other Paddle
        ??  Observed Y velocity of the other Paddle
        ??? Potential add-on: Y-acceleration and further derivatives of Y. Not x tho, that would be very stupid and not even make physical sense
    */
    public partial class PlayerData : ScriptableObject {
        public const string NO_NAME = "";
        private const bool TRACK_HISTORY_DEFAULT = false;
        
        [SerializeField] private string playerName;
        [SerializeField] private bool trackHistory = TRACK_HISTORY_DEFAULT;
        [SerializeField] private readonly List<DataUnit> history = new List<DataUnit>();

        // Remember, we eventually want a database that will autogenerate stuff like agent/model id: string, etc.
        // ? Maybe use burst compilation for tracking history?

        public void Initialize(string playerName) {
            this.playerName = playerName;
        }

        public void Initialize(string playerName, bool trackHistory) {
            this.playerName = playerName;
            this.trackHistory = trackHistory;
        }

        public string GetPlayerName() {
            return name;
        }

        public bool TrackHistory {
            get { return trackHistory; }
            set { trackHistory = value; }
        }

        public List<DataUnit> GetHistory() {
            return history;
        }

        /**
        * @param Vector2 playerPos - (x, y)
        * @param Vector2 opponentPos - (x, y)
        * @param Vector2[] relativeBallMotion - [(x, y), (x', y'), (x'', y''), ...]; Note that x'' and beyond will be 0; x' is constant, so it doesn't really matter
        ** (x, y) in [0.0, 1.0]. for x: 0.0 being at the player goal and 1.0 being at the opponent goal
        ** x' is constant and uses the same scale. After preprocessing, it can be either positive (heading away from Player) or negative (heading towards the player) 
        ** x'' and beyond = 0
        ** y uses a universal vertical axis (not specific per Paddle)
        */
        public void Feed(Vector2 playerPos, Vector2 opponentPos, Vector2[] ballMotion) {
            if (!trackHistory) {
                return;
            }
            
            // Relativize to the player
            Vector2[] relativeBallMotion = Motion2D.RelativizeTrajectoryByX(ballMotion, playerPos.x);

            float[] observation = new float[2 + relativeBallMotion.Length];
            
            // player Y pos and opponent Y pos
            observation[0] = playerPos.y;
            observation[1] = opponentPos.y;

            // relative ball positions
            Vector2 relativeBallPos = relativeBallMotion[0];
            observation[2] = relativeBallPos.x;  // [0.0, 1.0]; 0.0 being at the player goal and 1.0 being at the opponent goal
            observation[3] = relativeBallPos.y;  // [0.0, 1.0]; 0.0 at the bottom of the screen/boundary and 1.0 being at the top of the screen/boundary

            // relative ball velocities
            Vector2 relativeBallVelocity = relativeBallMotion[1];
            observation[4] = relativeBallVelocity.x;  // either positive (heading away from Player) or negative (heading towards the player)
            observation[5] = relativeBallVelocity.y;

            // ball motion
            int i_start = 2;
            int FURTHER_DERIVATIVES_START = 6;
            for (int i = i_start; i < relativeBallMotion.Length; i++) {
                int observationIndex = FURTHER_DERIVATIVES_START + (2 * (i - i_start)); // "i_obs0 + 2n"

                Vector2 relativeBallMotionDerivative = relativeBallMotion[i];
                
                observation[observationIndex] = relativeBallMotionDerivative.x;  // should be 0
                observation[observationIndex + 1] = relativeBallMotionDerivative.y;
            }

            // add to history
            DataUnit dataStep = new DataUnit(observation);
            history.Add(dataStep);
        }
    }
}