//namespace Pong.Ball;
using Pong.Ball;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pong;
using Pong.Physics;
using static Pong.GameCache;
using static Pong.GameHelpers;

public partial class PongBallController : MonoBehaviour
{
    // parameters of trajectory
    // contains base viewport velocity (Vector2) and float[] yAccelerationAndBeyond in terms of viewport percentage y
    private readonly Motion2D viewportMotion = new Motion2D();

    // trajectory facilitation
    private float elapsedTrajectoryTime = 0.0f;
    private bool hasTrajectory = false;

    // goal detection state
    private int ballStatus = BallStatus.NO_GOAL;

    // collision detection helper
    private RectangularBodyFrame bodyFrame;
    //private RectangularBodyFrame rebounderBodyFrame;

    void Awake()
    {
        ZeroMotion();
    }

    // Start is called before the first frame update
    void Start()
    {
        bodyFrame = GetComponent<RectangularBodyFrame>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasTrajectory) {
            // calculate current velocity
            Vector2 currentTotalViewportVelocity = viewportMotion.CalculateTotalVelocity(elapsedTrajectoryTime);

            // motion
            MoveLocal(ToLocal(currentTotalViewportVelocity));

            elapsedTrajectoryTime += Time.deltaTime;
        }
    }

    public void MoveLocal(Vector3 localDelta) {
        // origin is in the center
        (float MAX_X_POS, float MAX_Y_POS) = (BG_TRANSFORM.localScale.x / 2f, BG_TRANSFORM.localScale.y / 2f);
        (float MIN_X_POS, float MIN_Y_POS) = (-MAX_X_POS, -MAX_Y_POS);

        Vector3 actualLocalDelta = new Vector3(localDelta.x, localDelta.y, localDelta.z);
        
        //* Left Boundary
        if (bodyFrame.leftEdgeX + localDelta.x <= MIN_X_POS) {
            actualLocalDelta.x = MIN_X_POS - bodyFrame.leftEdgeX;

            //* Score Point
            ballStatus = BallStatus.GOAL_LEFT;

            // Integrate Changes
            transform.localPosition += actualLocalDelta;
            return;
        }

        //* Right Boundary
        if (bodyFrame.rightEdgeX + localDelta.x >= MAX_X_POS) {
            actualLocalDelta.x = MAX_X_POS - bodyFrame.rightEdgeX;
            
            //* Score Point
            ballStatus = BallStatus.GOAL_RIGHT;

            // Integrate Changes
            transform.localPosition += actualLocalDelta;
            return;
        }

        //* Top Wall
        if (bodyFrame.topEdgeY + localDelta.y >= MAX_Y_POS) {
            actualLocalDelta.y = MAX_Y_POS - bodyFrame.topEdgeY;

            //* Collide with the top wall => reverse y trajectory
            viewportMotion.velocity.y *= -1;
            //TODO: play wall sound
        }

        //* Bottom Wall
        if (bodyFrame.bottomEdgeY + localDelta.y <= MIN_Y_POS) {
            actualLocalDelta.y = MIN_Y_POS - bodyFrame.bottomEdgeY;
            
            //* Collide with the bottom wall => reverse y trajectory
            viewportMotion.velocity.y *= -1;
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
        // reset ball status
        ballStatus = BallStatus.NO_GOAL;

        // cancel motion: no more velocity and further derivatives! all 0
        viewportMotion.ZeroOut();
    }

    // [(x, y), (x', y'), (x'', y''), ...]
    // in terms of viewport %
    public Vector2[] RetrieveBallTrajectory() {
        Vector2 vpLocalPos = ToViewport(transform.localPosition);

        return viewportMotion.RetrieveTrajectory(vpLocalPos);
    }

    public int GetBallStatus() { return ballStatus; }

    public bool HasTrajectory() { return hasTrajectory; }
    public float GetElapsedTrajectoryTime() { return elapsedTrajectoryTime; }

    public Vector2 ViewportVelocity {
        get { 
            return viewportMotion.velocity; 
        }
        set {
            viewportMotion.velocity.Set(value.x, value.y);
        }
    }
}
