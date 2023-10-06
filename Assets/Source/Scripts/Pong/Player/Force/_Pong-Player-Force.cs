//* NAMESPACE HEADER FILE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pong.Physics;

namespace Pong.GamePlayer.Force {
    public partial class ForceMap {}

    public class Rebounder {
        public readonly ForceMap forceMap;
        public readonly RectangularBodyFrame bodyFrame;

        public Rebounder(ForceMap forceMap, RectangularBodyFrame bodyFrame) {
            this.forceMap = forceMap;
            this.bodyFrame = bodyFrame;
        }
    }
}