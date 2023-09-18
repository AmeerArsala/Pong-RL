//namespace Pong.Ball;
using Pong.Ball;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pong;
using static Pong.GameHelpers;
using static Pong.GameCache;

//* After scoring, it goes: Destroy() -> [tiny delay] -> Reset() -> [small delay] -> Serve()
public partial class PongBall {
    public readonly GameObject sprite; // it won't actually be destroyed; it will just vanish and look like it was destroyed
    private readonly Stack<float> serveAngles = new Stack<float>(); // in radians

    // Player on the offensive
    private Player attacker = null; // "lastTouchedBy"; the initial trajectory will also set this as the player opposite to where it is traveling

    public PongBall(GameObject ballSprite) {
        sprite = ballSprite;

        // add + initialize controller
        PongBallController ballController = sprite.AddComponent<PongBallController>();

        // initialize serveAngles
        uint maxRounds = (WIN_SCORE) + (WIN_SCORE - 1);
        for (uint i = 0; i < maxRounds; ++i) {
            float angle = Random.Range(-BALL_SERVE_MAX_ANGLE, BALL_SERVE_MAX_ANGLE);

            if (i % 2 == 1) { // if odd, add PI so that it goes on the left side 
                angle += Mathf.PI;
            }

            serveAngles.Push(angle);
        }
    }

    public static PongBall FromPrefab(GameObject prefab) {
        GameObject ballSprite = GameManager.Instantiate(prefab, GetStartLocalPosition(), Quaternion.identity);

        PongBall pongBall = new PongBall(ballSprite);
        pongBall.SetLocalScale(GameConstants.BALL_SCALE_Y);

        return pongBall;
    }

    // serve the ball
    public void Serve() {
        float angle = serveAngles.Pop();
        float speed = BALL_SPEED_VP; // in terms of viewport x percentage

        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)); // unit vector
        Vector2 viewportVelocity = speed * direction;
        //Vector3 localVelocity = ToLocal(viewportVelocity);

        PongBallController ballController = sprite.GetComponent<PongBallController>();
        ballController.viewportVelocity = viewportVelocity; // set velocity
        ballController.BeginTrajectory(); // start the timer for y'(t)
    }

    public void Update() {
        //TODO
    }

    public void Destroy() {
        //? maybe remove this below
        PongBallController ballController = sprite.GetComponent<PongBallController>();
        ballController.HaltTrajectory();

        sprite.SetActive(false);
    }

    public void Reset() {
        // activate
        sprite.SetActive(true);

        // set position to start position
        sprite.transform.localPosition = GetStartLocalPosition();
    }

    public void SetLocalScale(float vpY) {
        Vector3 bgScale = BG_TRANSFORM.localScale;

        sprite.transform.localScale = new Vector3(
            vpY * bgScale.y, // square
            vpY * bgScale.y, // square
            sprite.transform.localScale.z
        );
    }

    public static Vector3 GetStartLocalPosition() {
        return ToLocal(GameConstants.BALL_START_POSITION);
    }
}