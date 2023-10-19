//namespace Pong.GamePlayer;
using Pong.GamePlayer;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityUtils;

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
        private static uint UID = 0;

        // For RL and save/loading
        protected PlayerData playerData;

        // Tangible GameObjects
        public readonly ControlledGameObject<PlayerController> playerSprite;
        private readonly Scoreboard scoreboard;

        // For physics
        private readonly ForceMap forceMap;

        private Player opponent;

        public Player(PlayerData playerData, GameObject sprite, Scoreboard scoreboard) {
            this.playerData = playerData;
            this.scoreboard = scoreboard;

            // add + initialize controller
            PlayerController controller = sprite.AddComponent<PlayerController>();

            // collision detection
            RectangularBodyFrame bodyFrame = sprite.AddComponent<RectangularBodyFrame>();

            // collision forces
            forceMap = new ForceMap(sprite.transform);

            // wrap it up
            playerSprite = new ControlledGameObject<PlayerController>(sprite, controller);
        }

        public static string CreateName(string name) {
            // default value
            string playerName = name;

            // decide name if not named
            if (playerName.Equals(PlayerData.NO_NAME)) { // empty => no name => current date time name
                playerName = DateTime.Now.ToString("MM/dd/yyyy H:mm" + "_" + UID++);
                Debug.Log("Player Name: " + playerName);
            }

            return playerName;
        }

        public PlayerData GetPlayerData() { return playerData; }
        public Scoreboard GetScoreboard() { return scoreboard; }
        public ForceMap GetForceMap() { return forceMap; }

        public Player Opponent {
            get { return opponent; }
            set { opponent = value; }
        }

        // Frame-dependent 
        public virtual void Update() {
            //? put any frame-dependent updates here
        }

        // Time-dependent
        public virtual void FixedUpdate() {
            forceMap.PaddleVelocity = ToLocal(playerSprite.controller.GetViewportMotionTracker().velocity).y;
            forceMap.PaddleAcceleration = ToLocal(new Vector2(0f, playerSprite.controller.GetViewportMotionTracker().Y_Acceleration)).y;
        }

        public virtual void FeedData(Vector2 vpPos, Vector2 vpOpponentPos, Vector2[] ballMotion) {
            playerData.Feed(vpPos, vpOpponentPos, ballMotion);
            //? RL agent makes an inference
        }

        public virtual void ScorePoint() {
            // Game: score point
            scoreboard.ScorePoint();

            // onScore
            //TODO ...
            //? update RL agent?
        }
        
        public Rebounder AsRebounder() {
            return new Rebounder(forceMap, playerSprite.gameObj.GetComponent<RectangularBodyFrame>());
        }

        public void SetLocalPaddleDimensionsFromVP(float vpXThickness, float vpYLength) {
            Vector3 bgScale = GameCache.BG_TRANSFORM.localScale;

            playerSprite.transform.localScale = new Vector3(
                ToLocalX(vpXThickness),
                ToLocalY(vpYLength),
                playerSprite.transform.localScale.z
            );
        }

        // @param Vector2 vpDimensions - viewport dimensions Vector2f[thickness, length]
        public void SetLocalPaddleDimensionsFromVP(Vector2 vpDimensions) {
            SetLocalPaddleDimensionsFromVP(vpDimensions.x, vpDimensions.y);
        }

        public virtual void SaveData() {
            playerData.Save();
            //? In an inherited class, save the model here too
        }

        public virtual void LoadData(string filename) {
            playerData = PlayerData.Load(filename);
            //? In an inherited class, load the model here too
        }

        public void OnExit() {
            if (playerData.TrackHistory) {
                // write to memory
                SaveData();
            }
        }
    }

    //* An actual Human Player with controls and all
    public partial class HumanPlayer : Player {
        private readonly PlayerControls controls;

        public HumanPlayer(PlayerData playerData, GameObject sprite, PlayerControls controls, Scoreboard scoreboard) : base(playerData, sprite, scoreboard) {
            this.controls = controls;
        }

        public static HumanPlayer CreateNew(string name, GameObject prefab, Vector2 viewportPos, PlayerControls controls, TMP_Text scoreText, bool recordHistory=false) {
            // create paddle
            GameObject paddle = GameObject.Instantiate(prefab, ToLocal(viewportPos), Quaternion.identity);

            // validate name or have a default value
            string playerName = CreateName(name);

            // initialize data
            PlayerData playerData = new PlayerData(playerName, recordHistory);

            return new HumanPlayer(playerData, paddle, controls, new Scoreboard(scoreText));
        }

        public override void Update()
        {
            //base.Update();
            //* Listen for User Input
            playerSprite.controller.commandSensors.Sense(
                moveUp: Input.GetKey(controls.Up),
                moveDown: Input.GetKey(controls.Down));
        }

        public PlayerControls GetPlayerControls() { return controls; }
    }

    //* A cheating computer player
    public partial class CheatingPlayer : Player {
        public CheatingPlayer(PlayerData playerData, GameObject sprite, Scoreboard scoreboard) : base(playerData, sprite, scoreboard) {
            // constructor
        }

        public static CheatingPlayer CreateNew(string name, GameObject prefab, Vector2 viewportPos, TMP_Text scoreText, bool recordHistory=false) {
            // create paddle
            GameObject paddle = GameObject.Instantiate(prefab, ToLocal(viewportPos), Quaternion.identity);

            // validate name or have a default value
            string playerName = CreateName(name);

            // initialize data
            PlayerData playerData = new PlayerData(playerName, recordHistory);

            return new CheatingPlayer(playerData, paddle, new Scoreboard(scoreText));
        }

        public override void FeedData(Vector2 vpPos, Vector2 vpOpponentPos, Vector2[] ballMotion)
        {
            base.FeedData(vpPos, vpOpponentPos, ballMotion);

            Vector2 ballPosition = ballMotion[0];
            Vector2 localPosition = ToViewport(playerSprite.transform.localPosition);

            //* Cheat by tracing the ball's position
            playerSprite.controller.commandSensors.Sense(
                moveUp: (ballPosition.y > localPosition.y),
                moveDown: (ballPosition.y < localPosition.y));
        }
    }
}