//namespace Pong;
using Pong;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pong.GamePlayer;
using Pong.Ball;

using TMPro;
using System;

namespace Pong {
    public partial class GameManager : MonoBehaviour
    {
        private Player player1, player2;
        private PongBall ball;

        // CONTEXT: public => reference in the Unity Editor
        public string player1Name = PlayerData.NO_NAME, player2Name = PlayerData.NO_NAME;
        public float playerSpeedVP = 1.00f; // per second; travel 100% vertical screen size in one second
        public float ballSpeedVP = 0.65f;   // per second; travel 45% horizontal screen size in one second
        public float ballServeMaxAngle = (3f / 7f) * Mathf.PI;
        public float ballBounceMaxAngle = (3f / 7f) * Mathf.PI;
        public uint scoreToWin = GameConstants.DEFAULT_WIN_SCORE;
        public GameObject playerPrefab; // will be a sprite prefab
        public GameObject ballPrefab;   // will be a sprite prefab
        public GameObject backgroundSprite; // reference a GameObject in the Scene
        public TMP_Text player1scoreText, player2scoreText; // reference in Scene

        void Awake()
        {
            // Hello World message
            Debug.Log("Hello World!");

            //Debug.Log(GameConstants.RIGHT_PADDLE_START_POSITION);
            //Debug.Log(GameConstants.LEFT_PADDLE_START_POSITION);
        }
        
        void Start()
        {
            // Cache Desired Global Variables
            GameCache.BG_TRANSFORM = backgroundSprite.transform;
            GameCache.PLAYER_SPEED_VP = playerSpeedVP;
            GameCache.BALL_SPEED_VP = ballSpeedVP;
            GameCache.BALL_SERVE_MAX_ANGLE = ballServeMaxAngle;
            GameCache.BALL_BOUNCE_MAX_ANGLE = ballBounceMaxAngle;
            GameCache.WIN_SCORE = scoreToWin;
            //TODO: use import audio library
            //Audio.Cache.SFX = Audio.SfxPack.FromRegisteredMappings();

            // Initialize Players/Pong Paddles
            player1 = Player.CreateNew(player1Name, playerPrefab, GameConstants.RIGHT_PADDLE_START_POSITION, GameConstants.RIGHT_PADDLE_CONTROLS, player1scoreText);
            player2 = Player.CreateNew(player2Name, playerPrefab, GameConstants.LEFT_PADDLE_START_POSITION, GameConstants.LEFT_PADDLE_CONTROLS, player2scoreText);
            
            // Make them enemies!!! >:)
            player1.Opponent = player2;
            player2.Opponent = player1;

            //* Create ball, then make it go at a random direction (left or right)
            ball = PongBall.FromPrefab(ballPrefab);
            ball.Initialize(server: RandomPlayer());
            ball.Serve();
        }

        // Update is called once per frame
        void Update()
        {
            // Player Updates
            player1.Update();
            player2.Update();

            // PongBall Update
            ball.Update(); // didn't call this before the player updates for a better user experience
        }

        // Time-dependent
        void FixedUpdate() {
            // Player Fixed Updates
            player1.FixedUpdate();
            player2.FixedUpdate();

            // PongBall Fixed Update
            ball.FixedUpdate();
        }

        public string GetCurrentScore() {
            return player1.GetScoreboard().GetScore() + "-" + player2.GetScoreboard().GetScore();
        }

        // pick a random Player. Either player1 or player2
        public Player RandomPlayer() {
            bool isPlayer1 = UnityEngine.Random.Range(0, 2) == 0; // random boolean

            if (isPlayer1) {
                return player1;
            } else {
                return player2;
            }
        }
    }
}