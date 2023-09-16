//namespace Pong.Player;
using Pong.Player;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using Pong;

/**
 ** PlayerController controller
 ** PlayerData playerData 
*/
public partial class Player {
    private PlayerData playerData;
    public readonly GameObject sprite;
    
    private int score = 0;
    private Player opponent;

    // load from data
    public Player(PlayerData playerData, GameObject sprite, PlayerControls controls) {
        this.playerData = playerData;
        this.sprite = sprite;

        // add + initialize controller
        PlayerController controller = sprite.AddComponent<PlayerController>();
        controller.InitializeControls(controls);
    }

    private static GameObject InstantiatePaddle(GameObject prefab, Vector2 viewportPos) {
        // calculate actual position
        Vector3 bgScale = GameCache.BG_TRANSFORM.localScale;
        Vector2 pos2f = viewportPos * new Vector2(bgScale.x, bgScale.y);
        Vector3 pos = new Vector3(pos2f.x, pos2f.y, 0f);

        // create paddle
        GameObject paddle = GameObject.Instantiate(prefab, pos, Quaternion.identity);

        //Debug.Log("Viewport Position (Center Origin): " + viewportPos);
        //Debug.Log("Final Position: " + pos);

        return paddle;
    }

    public static Player CreateNew(string name, GameObject prefab, Vector2 viewportPos, PlayerControls controls) {
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

        return new Player(playerData, paddle, controls);
    }

    //TODO:
    //public static Player LoadExisting(string )

    public int GetScore() { return score; }
    public PlayerData GetPlayerData() { return playerData; }

    public Player Opponent {
        get { return opponent; }
        set { opponent = value; }
    }

    public void ScorePoint() {
        score++;

        // TODO: Update corresponding UI element

        OnScore();
    }

    public void OnScore() {
        // ? update Agent?
    }

    public void Update() {
        //TODO
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