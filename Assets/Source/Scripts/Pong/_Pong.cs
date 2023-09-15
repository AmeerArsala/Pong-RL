// * NAMESPACE HEADER FILE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pong.Player;

namespace Pong {
    public static class GameConstants {
        // viewport x
        public const float RIGHT_PADDLE_X_POSITION = 0.9f;
        public const float LEFT_PADDLE_X_POSITION = 1.0f - RIGHT_PADDLE_X_POSITION;

        // viewport position
        public static readonly Vector2 RIGHT_PADDLE_START_POSITION = new Vector2(RIGHT_PADDLE_X_POSITION, 0.5f); // midpoint on y-axis
        public static readonly Vector2 LEFT_PADDLE_START_POSITION = new Vector2(LEFT_PADDLE_X_POSITION, 0.5f);   // midpoint on y-axis

        public static readonly PlayerControls RIGHT_PADDLE_CONTROLS = new PlayerControls(KeyCode.UpArrow, KeyCode.DownArrow);
        public static readonly PlayerControls LEFT_PADDLE_CONTROLS = new PlayerControls(KeyCode.W, KeyCode.S);

        public static readonly char[] WINDOWS_BANNED_CHARS = {'\\', '/', ':', '*', '?', '\"', '<', '>', '|'};
    }

    public static class GameCache {
        public static Transform BG_TRANSFORM;
    }

    public partial class GameManager {}
}