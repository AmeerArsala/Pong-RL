//namespace Pong.Ball;
using Pong.Ball;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pong;
using static Pong.GameCache;
using static Pong.GameHelpers;

public partial class PongBallController : MonoBehaviour
{
    // parameters of trajectory
    public Vector2 viewportVelocity = new Vector2(0f, 0f); // base viewport velocity
    private readonly float[] yAccelerationAndBeyond = new float[GameConstants.BALL_Y_MAX_DERIVATIVE - 1]; // in terms of viewport percentage y

    // delta
    private Vector2 totalViewportVelocity = new Vector2(0f, 0f);

    // trajectory
    private float elapsedTrajectoryTime = 0.0f;
    private bool hasTrajectory = false;

    private int ballStatus = BallStatus.NO_GOAL;

    // collision detection helper
    private readonly BodyPoints body = new BodyPoints();

    private class BodyPoints {
        public float halfBallLength;

        public float leftEdgeX, rightEdgeX;
        public float topEdgeY, bottomEdgeY;

        public BodyPoints() {}

        public void Update(Transform transform) {
            // origin is in the center
            halfBallLength = transform.localScale.y / 2f;

            leftEdgeX = transform.localPosition.x - halfBallLength;
            rightEdgeX = transform.localPosition.x + halfBallLength;
            topEdgeY = transform.localPosition.y + halfBallLength;
            bottomEdgeY = transform.localPosition.y - halfBallLength;
        }
    }

    void Awake()
    {
        ZeroMotion();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Update body points
        body.Update(transform);

        if (hasTrajectory) {
            // calculate current velocity
            totalViewportVelocity = CalculateViewportVelocity(elapsedTrajectoryTime);

            // motion
            MoveLocal(ToLocal(totalViewportVelocity));

            elapsedTrajectoryTime += Time.deltaTime;
        }
    }

    public Vector2 CalculateViewportVelocity(float time) {
        Vector2 vpVelocity = viewportVelocity;

        for (int i = 0; i < yAccelerationAndBeyond.Length; ++i) {
            float coefficient = yAccelerationAndBeyond[i];
            float termIndex = i + 1; // yes, this is a float and is equal to the nth derivative - 1

            // only y changes; x changing like that would be very stupid
            vpVelocity.y += coefficient * (Mathf.Pow(time, termIndex) / termIndex);
        }

        return vpVelocity;
    }

    public void MoveLocal(Vector3 localDelta) {
        // origin is in the center
        (float MAX_X_POS, float MAX_Y_POS) = (BG_TRANSFORM.localScale.x / 2f, BG_TRANSFORM.localScale.y / 2f);
        (float MIN_X_POS, float MIN_Y_POS) = (-MAX_X_POS, -MAX_Y_POS);

        Vector3 actualLocalDelta = new Vector3(localDelta.x, localDelta.y, localDelta.z);
        
        // left
        if (transform.localPosition.x + localDelta.x < MIN_X_POS) {
            actualLocalDelta.x = MIN_X_POS - transform.localPosition.x;

            //* Score Point
            ballStatus = BallStatus.GOAL_LEFT;

            // Integrate Changes
            transform.localPosition += actualLocalDelta;
            return;
        }

        // right
        if (transform.localPosition.x + localDelta.x > MAX_X_POS) {
            actualLocalDelta.x = MAX_X_POS - transform.localPosition.x;
            
            //* Score Point
            ballStatus = BallStatus.GOAL_RIGHT;

            // Integrate Changes
            transform.localPosition += actualLocalDelta;
            return;
        }

        // top
        if (transform.localPosition.y + localDelta.y > MAX_Y_POS) {
            actualLocalDelta.y = MAX_Y_POS - transform.localPosition.y;

            //* Collide with the top wall
            actualLocalDelta.y *= -1; // reverse y trajectory
            //TODO: play wall sound
        }

        // bottom
        if (transform.localPosition.y + localDelta.y < MIN_Y_POS) {
            actualLocalDelta.y = MIN_Y_POS - transform.localPosition.y;
            
            //* Collide with the bottom wall
            actualLocalDelta.y *= -1; // reverse y trajectory
            //TODO: play wall sound
        }
        
        //* Integrate Changes
        transform.localPosition += actualLocalDelta;
    }

    public void BeginTrajectory() {
        hasTrajectory = true;
        elapsedTrajectoryTime = 0.0f;
    }

    public void HaltTrajectory() {
        hasTrajectory = false;
        elapsedTrajectoryTime = 0.0f;

        ZeroMotion();
    }

    private void ZeroMotion() {
        ballStatus = BallStatus.NO_GOAL;

        viewportVelocity.x = 0f;
        viewportVelocity.y = 0f;

        totalViewportVelocity.x = 0f;
        totalViewportVelocity.y = 0f;

        // initialize with zeroes
        for (int i = 0; i < yAccelerationAndBeyond.Length; ++i) {
            yAccelerationAndBeyond[i] = 0;
        }
    }

    // [(x, y), (x', y'), (x'', y''), ...]
    // in terms of viewport %
    public Vector2[] RetrieveBallMotion() {
        Vector2[] ballMotion = new Vector2[2 + yAccelerationAndBeyond.Length]; // position and velocity are included too!

        ballMotion[0] = ToViewport(transform.localPosition);
        ballMotion[1] = totalViewportVelocity;
        for (int i = 0; i < yAccelerationAndBeyond.Length; ++i) {
            int i_ballMotion = i + 2;
            ballMotion[i_ballMotion] = new Vector2(0.0f, yAccelerationAndBeyond[i]); // there is no x acceleration and beyonod, that would be very stupid
        }

        return ballMotion;
    }

    public int GetBallStatus() { return ballStatus; }
}
