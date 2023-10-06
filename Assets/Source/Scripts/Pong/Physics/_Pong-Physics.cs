//* NAMESPACE HEADER FILE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pong.Physics {
    public partial class Motion2D {}

    public partial class RectangularBodyFrame : MonoBehaviour {}

    public static class CollisionState {
        public const int NONE = 0;
        public const int HORIZONTAL = 1;
        public const int VERTICAL = 2;
    }
}