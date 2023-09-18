//namespace Pong.Player;
using Pong.Player;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Pong.GameCache;

public partial class PlayerController : MonoBehaviour
{
    //? incorporate RigidBody2D + ball physics

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
        RespondToInput();
    }

    protected void RespondToInput() {
        if (!isInitialized) {
            return;
        }

        float dy = 0f;

        if (Input.GetKey(controls.Up)) {
            dy += DeltaY();
        }

        if (Input.GetKey(controls.Down)) {
            dy += -DeltaY();
        }

        // now that all the movement updates have been collected, time to apply them
        MoveY(dy);
    }

    protected float DeltaY() {
        float dy_dt = PLAYER_SPEED_VP * BG_TRANSFORM.localScale.y;
        
        return dy_dt * Time.deltaTime;
    }

    public void MoveY(float deltaY) {
        // origin is in the center
        float MAX_Y_POS = BG_TRANSFORM.localScale.y / 2f;
        float halfPaddleLength = transform.localScale.y / 2f;
        
        float topEdgeY = transform.localPosition.y + halfPaddleLength;
        float bottomEdgeY = transform.localPosition.y - halfPaddleLength;

        if (topEdgeY + deltaY > MAX_Y_POS || bottomEdgeY + deltaY < -MAX_Y_POS) { // not allowed to move out of bounds
            return;
        }

        // moving will not take it out of bounds
        transform.localPosition += new Vector3(0f, deltaY, 0f);
    }

    public bool IsInitialized() { return isInitialized; }
}
