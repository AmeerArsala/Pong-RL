//namespace Pong.Player;
using Pong.Player;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using Pong;
using Pong.UI;
using TMPro;

using static Pong.GameHelpers;

/**
 ** PlayerController controller
 ** PlayerData playerData 
*/
public partial class Player {
    private PlayerData playerData;
    public readonly GameObject sprite;
    
    private Scoreboard scoreboard;
    private Player opponent;

    // load from data
    public Player(PlayerData playerData, GameObject sprite, PlayerControls controls, Scoreboard scoreboard) {
        this.playerData = playerData;
        this.sprite = sprite;
        this.scoreboard = scoreboard;

        // add + initialize controller
        PlayerController controller = sprite.AddComponent<PlayerController>();
        controller.InitializeControls(controls);
    }

    private static GameObject InstantiatePaddle(GameObject prefab, Vector2 viewportPos) {
        // calculate actual position
        Vector3 pos = ToLocal(viewportPos);
        /*Vector3 bgScale = GameCache.BG_TRANSFORM.localScale;
        Vector2 pos2f = viewportPos * new Vector2(bgScale.x, bgScale.y);
        Vector3 pos = new Vector3(pos2f.x, pos2f.y, 0f);*/

        // create paddle
        GameObject paddle = GameObject.Instantiate(prefab, pos, Quaternion.identity);

        return paddle;
    }

    public static Player CreateNew(string name, GameObject prefab, Vector2 viewportPos, PlayerControls controls, TMP_Text scoreText) {
        // create paddle
        GameObject paddle = InstantiatePaddle(prefab, viewportPos);

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

    public Player Opponent {
        get { return opponent; }
        set { opponent = value; }
    }

    public void Update() {
        //TODO

        //TODO: playerData.feed(...);
    }

    public void ScorePoint() {
        // Game: score point
        scoreboard.ScorePoint();

        // onScore
        //TODO ...
        //? update RL agent?
    }

    public void SetLocalPaddleDimensions(float vpThickness, float vpLength) {
        Vector3 bgScale = GameCache.BG_TRANSFORM.localScale;

        sprite.transform.localScale = new Vector3(
            vpThickness * bgScale.x,
            vpLength * bgScale.y,
            sprite.transform.localScale.z
        );
    }

    // @param Vector2 vpDimensions - viewport dimensions Vector2f[thickness, length]
    public void SetLocalPaddleDimensions(Vector2 vpDimensions) {
        SetLocalPaddleDimensions(vpDimensions.x, vpDimensions.y);
    }
}