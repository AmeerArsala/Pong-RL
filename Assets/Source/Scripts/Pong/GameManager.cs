//namespace Pong;
using Pong;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pong.Player;

public partial class GameManager : MonoBehaviour
{
    private Player player1, player2;

    // CONTEXT: public => reference in the Unity Editor
    public string player1Name = PlayerData.NO_NAME, player2Name = PlayerData.NO_NAME;
    public GameObject playerPrefab; // will be a sprite prefab
    public GameObject backgroundSprite; // reference a GameObject in the Scene
    
    void Start()
    {
        // Hello World message
        Debug.Log("Hello World!");

        // Cache Desired Global Variables
        GameCache.BG_TRANSFORM = backgroundSprite.transform;

        //Debug.Log(GameConstants.RIGHT_PADDLE_START_POSITION);
        //Debug.Log(GameConstants.LEFT_PADDLE_START_POSITION);

        // Initialize Players/Pong Paddles
        player1 = Player.CreateNew(player1Name, playerPrefab, GameConstants.RIGHT_PADDLE_START_POSITION, GameConstants.RIGHT_PADDLE_CONTROLS);
        player2 = Player.CreateNew(player2Name, playerPrefab, GameConstants.LEFT_PADDLE_START_POSITION, GameConstants.LEFT_PADDLE_CONTROLS);
        
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
        return player1.GetScore() + "-" + player2.GetScore();
    }
}
