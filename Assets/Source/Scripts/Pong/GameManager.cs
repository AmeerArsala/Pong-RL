//namespace Pong;
using Pong;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pong.Player;

public partial class GameManager : MonoBehaviour
{
    private Player player1, player2;

    // public => reference in the Unity Editor
    public string player1Name = PlayerData.NO_NAME, player2Name = PlayerData.NO_NAME;
    public GameObject playerPrefab; // will be a sprite prefab
    public GameObject backgroundSprite; // reference a GameObject in the Scene
    
    void Start()
    {
        // Hello World
        Debug.Log("Hello World!");

        // Cache Desired Global Variables
        GameCache.BG_TRANSFORM = backgroundSprite.transform;

        //Debug.Log(GameConstants.RIGHT_PADDLE_START_POSITION);
        //Debug.Log(GameConstants.LEFT_PADDLE_START_POSITION);

        // Initialize Players/Pong Paddles
        player1 = Player.CreateNew(player1Name, playerPrefab, GameConstants.RIGHT_PADDLE_START_POSITION, GameConstants.RIGHT_PADDLE_CONTROLS);
        player2 = Player.CreateNew(player2Name, playerPrefab, GameConstants.LEFT_PADDLE_START_POSITION, GameConstants.LEFT_PADDLE_CONTROLS);
        
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: put stuff here
    }

    public string GetCurrentScore() {
        return player1.GetScore() + "-" + player2.GetScore();
    }
}
