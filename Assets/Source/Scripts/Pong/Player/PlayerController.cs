namespace Pong.Player;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isInitialized = false;
    public PlayerControls controls;

    public void Initialize(PlayerControls controls) {
        this.controls = controls;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        control();
    }

    private void control() {
        if (!isInitialized) {
            return;
        }

        if (Input.GetKey(controls.Up)) {
            //TODO: move up
        }

        if (Input.GetKey(controls.Down)) {
            //TODO: move down
        }
    }
}

public record PlayerControls(KeyCode Up, KeyCode Down);
