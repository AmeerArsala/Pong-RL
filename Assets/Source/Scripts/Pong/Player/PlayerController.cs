//namespace Pong.Player;
using Pong.Player;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    // per second
    public const float SPEED_VIEWPORT_PERCENTAGE = 1.00f;  // travel 100% vertical screen size in one second

    private bool isInitialized = false;
    public PlayerControls controls;

    public void InitializeControls(PlayerControls controls) {
        this.controls = controls;
        isInitialized = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        respondToInput();
    }

    protected void respondToInput() {
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
