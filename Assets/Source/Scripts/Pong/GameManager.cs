//namespace Pong;
using Pong;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pong.Player;
using TMPro;
using System;

public partial class GameManager : MonoBehaviour
{
    private Player player1, player2;

    // CONTEXT: public => reference in the Unity Editor
    public string player1Name = PlayerData.NO_NAME, player2Name = PlayerData.NO_NAME;
    public float playerSpeedVP = 1.00f; // per second; travel 100% vertical screen size in one second
    public float ballSpeedVP = 0.45f;   // per second; travel 45% horizontal screen size in one second
    public GameObject playerPrefab; // will be a sprite prefab
    public GameObject ballPrefab;   // will be a sprite prefab
    public GameObject backgroundSprite; // reference a GameObject in the Scene
    public TMP_Text player1scoreText, player2scoreText; // reference in Scene
    
    void Start()
    {
        // Hello World message
        Debug.Log("Hello World!");

        // Cache Desired Global Variables
        GameCache.BG_TRANSFORM = backgroundSprite.transform;
        GameCache.PLAYER_SPEED_VP = playerSpeedVP;
        GameCache.BALL_SPEED_VP = ballSpeedVP;
        //TODO: use import audio library
        //Audio.Cache.SFX = Audio.SfxPack.FromRegisteredMappings();

        //Debug.Log(GameConstants.RIGHT_PADDLE_START_POSITION);
        //Debug.Log(GameConstants.LEFT_PADDLE_START_POSITION);

        // Initialize Players/Pong Paddles
        player1 = Player.CreateNew(player1Name, playerPrefab, GameConstants.RIGHT_PADDLE_START_POSITION, GameConstants.RIGHT_PADDLE_CONTROLS, player1scoreText);
        player2 = Player.CreateNew(player2Name, playerPrefab, GameConstants.LEFT_PADDLE_START_POSITION, GameConstants.LEFT_PADDLE_CONTROLS, player2scoreText);

        // TODO: Create ball, then make it go at a random direction (left or right)
        
        // Make them enemies!!! >:)
        player1.Opponent = player2;
        player2.Opponent = player1;
    }

    // Update is called once per frame
    void Update()
    {
        //ball.Update()
        player1.Update();
        player2.Update();
    }

    public string GetCurrentScore() {
        return player1.GetScoreboard().GetScore() + "-" + player2.GetScoreboard().GetScore();
    }
}
