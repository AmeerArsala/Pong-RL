//* NAMESPACE HEADER FILE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pong.Physics {
    public partial class Motion2D {}

    public partial class RectangularBodyFrame : MonoBehaviour {}

    public static class BoundState {
        public const int UNBOUNDED = 0;

        // Positive: in between another
        public const int TOP_BOUNDED = 2;    // Horizontal = even
        public const int BOTTOM_BOUNDED = 4; // Horizontal = even
        public const int LEFT_BOUNDED = 1;   // Vertical = odd
        public const int RIGHT_BOUNDED = 3;  // Vertical = odd

        // Negative: another is in between it
        public const int TOP_BOUNDING_ANOTHER = -TOP_BOUNDED;
        public const int BOTTOM_BOUNDING_ANOTHER = -BOTTOM_BOUNDED;
        public const int LEFT_BOUNDING_ANOTHER = -LEFT_BOUNDED;
        public const int RIGHT_BOUNDING_ANOTHER = -RIGHT_BOUNDED;

        public static bool CollisionIsHorizontal(int boundState) {
            return Mathf.Abs(boundState) % 2 == 0;
        }
    }
}