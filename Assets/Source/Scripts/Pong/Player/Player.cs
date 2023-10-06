//namespace Pong.GamePlayer;
using Pong.GamePlayer;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using Pong;
using Pong.GamePlayer.Force;
using Pong.Physics;
using Pong.UI;
using TMPro;

using static Pong.GameHelpers;

namespace Pong.GamePlayer {
    /**
    ** PlayerController controller
    ** PlayerData playerData 
    */
    public partial class Player {
        private PlayerData playerData;

        public readonly GameObject sprite;
        private readonly PlayerController controller;
        private readonly Scoreboard scoreboard;

        private readonly ForceMap forceMap;

        private Player opponent;

        // load from data
        public Player(PlayerData playerData, GameObject sprite, PlayerControls controls, Scoreboard scoreboard) {
            this.playerData = playerData;
            this.sprite = sprite;
            this.scoreboard = scoreboard;

            // add + initialize controller
            controller = sprite.AddComponent<PlayerController>();
            controller.InitializeControls(controls);

            // collision detection
            RectangularBodyFrame bodyFrame = sprite.AddComponent<RectangularBodyFrame>();

            // collision forces
            forceMap = new ForceMap(sprite.transform);
        }

        public static Player CreateNew(string name, GameObject prefab, Vector2 viewportPos, PlayerControls controls, TMP_Text scoreText) {
            // create paddle
            GameObject paddle = GameObject.Instantiate(prefab, ToLocal(viewportPos), Quaternion.identity);

            // default value
            string playerName = name;

            // decide name if not named
            if (playerName.Equals(PlayerData.NO_NAME)) { // empty => no name => current date time name
                playerName = DateTime.Now.ToString("MM/dd/yyyy H:mm");
            }

            // initialize and set name
            PlayerData playerData = ScriptableObject.CreateInstance<PlayerData>();
            playerData.Initialize(playerName);

            return new Player(playerData, paddle, controls, new Scoreboard(scoreText));
        }

        //TODO:
        //public static Player LoadExisting(string )

        public PlayerData GetPlayerData() { return playerData; }
        public Scoreboard GetScoreboard() { return scoreboard; }
        public ForceMap GetForceMap() { return forceMap; }

        public Player Opponent {
            get { return opponent; }
            set { opponent = value; }
        }

        public void Update() {
            forceMap.PaddleVelocity = ToLocal(controller.GetViewportMotionTracker().velocity).y;
            forceMap.PaddleAcceleration = ToLocal(new Vector2(0f, controller.GetViewportMotionTracker().Y_Acceleration)).y;

            //TODO: playerData.feed(...);
        }

        public void ScorePoint() {
            // Game: score point
            scoreboard.ScorePoint();

            // onScore
            //TODO ...
            //? update RL agent?
        }

        //TODO:
        //public void SendBallData(Vector) {}

        public Rebounder AsRebounder() {
            return new Rebounder(forceMap, sprite.GetComponent<RectangularBodyFrame>());
        }

        public void SetLocalPaddleDimensionsFromVP(float vpXThickness, float vpYLength) {
            Vector3 bgScale = GameCache.BG_TRANSFORM.localScale;

            sprite.transform.localScale = new Vector3(
                vpXThickness * bgScale.x,
                vpYLength * bgScale.y,
                sprite.transform.localScale.z
            );
        }

        // @param Vector2 vpDimensions - viewport dimensions Vector2f[thickness, length]
        public void SetLocalPaddleDimensionsFromVP(Vector2 vpDimensions) {
            SetLocalPaddleDimensionsFromVP(vpDimensions.x, vpDimensions.y);
        }
    }
}