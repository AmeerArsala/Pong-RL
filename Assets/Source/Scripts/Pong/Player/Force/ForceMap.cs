//namespace Pong.Player.Force;
using Pong.Player.Force;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ForceMap {
    //* Angle scale is centered around [0, PI] where 0 and PI represent the ends of the paddle and everything in between is the straight portion of the paddle
    private float normalStartAngle, normalEndAngle; // normal angle range => multiplied trajectory (direction is simply redirected)

    // for angles in the ABNORMAL range (not normal), they completely modify the trajectory of the ball when the force is applied
    
    public ForceMap(float normalStartAngle, float normalEndAngle) {
        this.normalStartAngle = normalStartAngle;
        this.normalEndAngle = normalEndAngle;
    }

    public ForceMap() {
        normalStartAngle = 0f;
        normalEndAngle = Mathf.PI;
    }

    public (float, float) NormalAngleRange {
        get { return (normalStartAngle, normalEndAngle); }
        set {
            (float startAngle, float endAngle) = value;
            normalStartAngle = startAngle;
            normalEndAngle = endAngle;
        }
    }

    public bool IsNormalAngle(float angle) {
        float relativizedAngle = RelativizeAngle(angle);

        return relativizedAngle >= normalStartAngle && relativizedAngle <= normalEndAngle;
    }

    public static float CalculateRelativeAngle(Vector2 centerToPointOfContact) {
        Vector2 relativeVec = new Vector2(centerToPointOfContact.x, centerToPointOfContact.y);
        
        // put everything within [-PI/2, PI/2] for now
        relativeVec.x = Mathf.Abs(relativeVec.x);

        // I COULD just use Mathf.Atan2(relativeVec.y, relativeVec.x), but this is more readable atm
        float tan_relativeAngle = (relativeVec.y / relativeVec.x);
        float relativeAngle = Mathf.Atan(tan_relativeAngle);

        // put on the same scale of [0, PI]
        relativeAngle += Mathf.PI / 2f;

        return relativeAngle;
    }

    public static float RelativizeAngle(float angle) {
        return CalculateRelativeAngle(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));
    }
}