//namespace Pong.Player;
using Pong.Player;

using System;
using UnityEngine;

using Pong;

/**
 ** PlayerController controller
 ** PlayerData playerData 
*/
public partial class Player {
    private PlayerData playerData;
    public readonly GameObject sprite;
    
    private int score = 0;

    // load from data
    public Player(PlayerData playerData, GameObject sprite, PlayerControls controls) {
        this.playerData = playerData;
        this.sprite = sprite;

        // add + initialize controller
        PlayerController controller = sprite.AddComponent<PlayerController>();
        controller.InitializeControls(controls);
    }

    public Player(string name, GameObject body, PlayerControls controls) : this(new PlayerData(name), body, controls) {}

    public static Player CreateNew(string name, GameObject prefab, Vector2 viewportPos, PlayerControls controls) {
        // calculate actual position
        Vector2 pos = viewportPos * GameCache.BG_TRANSFORM.position;

        // create paddle
        GameObject paddle = GameObject.Instantiate(prefab, pos, Quaternion.identity);

        // default value
        string playerName = name;

        // decide name if not named
        if (playerName.Equals(PlayerData.NO_NAME)) { // empty => no name => current date time name
            playerName = DateTime.Now.ToString("MM/dd/yyyy H:mm");
        }

        return new Player(playerName, paddle, controls);
    }

    //TODO:
    //public static Player LoadExisting(string )

    public int GetScore() { return score; }
    public PlayerData GetPlayerData() { return playerData; }

    public void ScorePoint() {
        score++;

        // TODO: Update UI element

        OnScore();
    }

    public void OnScore() {
        // ? update Agent?
    }
}