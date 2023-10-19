//namespace Pong.GamePlayer;
using Pong.GamePlayer;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;

using Pong.RL;
using Pong.Physics;

namespace Pong.GamePlayer {
    [System.Serializable]
    public class PlayerDataBundle { //* Will serialize to JSON
        private static uint UID = 0;
        private const char DELIMITER = '_';

        public string player_name;
        public readonly List<DataUnit> data_history;
        public string date_time;

        public PlayerDataBundle(string player_name, List<DataUnit> data_history) {
            this.player_name = player_name;
            this.data_history = data_history;

            date_time = DateTime.Now.ToString("MM/dd/yyyy H:mm" + DELIMITER + UID++);
        }

        public string FileName() {
            // assume player_name allows all legal characters (will design for that later)
            return player_name + DELIMITER + date_time.Replace('/', DELIMITER).Replace(':', DELIMITER);
        }
    }

    /*  For Pong RL, we will only care about the statistics of:
        *   Y position of Paddle
        *   Ball's relative distance: vec2[X distance away (absolute value) from goal, Y distance away from Paddle]
        *   Ball's velocity: vec2[X speed towards goal (absolute value), Y]
        ?   Observed Y position of the other Paddle
        ??  Observed Y velocity of the other Paddle
        ??? Potential add-on: Y-acceleration and further derivatives of Y. Not X tho, that would be very stupid and not even make physical sense
    */
    public partial class PlayerData {
        public const string PLAYER_DATA_PATH = "Game/Data/Player/";
        public const string NO_NAME = "";
        private const bool TRACK_HISTORY_DEFAULT = false;
        
        // serialize only these fields 
        private string playerName;
        private bool trackHistory = TRACK_HISTORY_DEFAULT;
        private readonly List<DataUnit> history = new List<DataUnit>();

        private DataUnit current = null;

        // Remember, we eventually want a database that will autogenerate stuff like agent/model id: string, etc.
        // ? Maybe use burst compilation for tracking history?

        public PlayerData(string playerName) {
            this.playerName = playerName;
        }

        public PlayerData(string playerName, bool trackHistory) {
            this.playerName = playerName;
            this.trackHistory = trackHistory;
        }

        public PlayerData(PlayerDataBundle bundle) {
            playerName = bundle.player_name;
            history = bundle.data_history;
            trackHistory = true;
        }

        public string GetPlayerName() {
            return playerName;
        }

        public bool TrackHistory {
            get { return trackHistory; }
            set { trackHistory = value; }
        }

        public List<DataUnit> GetHistory() {
            return history;
        }

        public DataUnit GetMostRecent() {
            return current;
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
                current = null;
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

            current = dataStep;
        }

        //* Saves a JSON file
        public void Save() {
            // Bundle up the data the object has
            PlayerDataBundle bundle = new PlayerDataBundle(playerName, history);

            // Turn it into JSON
            string json = JsonUtility.ToJson(bundle);

            // Write it 
            string filename = bundle.FileName() + ".json";
            File.WriteAllText(PLAYER_DATA_PATH + filename, json);
        }

        public static PlayerData Load(string filename) {
            string filepath = PLAYER_DATA_PATH + filename;
            if (File.Exists(filepath)) {
                string json = File.ReadAllText(filepath);

                PlayerDataBundle bundle = JsonUtility.FromJson<PlayerDataBundle>(json);
                return new PlayerData(bundle);
            }

            return null;
        }
    } 
}