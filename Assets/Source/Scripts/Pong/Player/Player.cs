//namespace Pong.Player;
using Pong.Player;

using System;
using UnityEngine;

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
        controller.Initialize(controls);
    }

    // constructors will create new data, unless loaded from an old save
    public Player(GameObject body, PlayerControls controls) : this(DateTime.Now.ToString("MM/dd/yyyy H:mm"), body, controls) {
      // no name specified, so current date + time as name
    }

    public Player(string name, GameObject body, PlayerControls controls) : this(new PlayerData(name), body, controls) {}

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