// * NAMESPACE HEADER FILE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pong.Player;

namespace Pong {
    public static class GameConstants {
        // constants for if origin of a viewport is in the middle of the screen
        public const float CENTER_ORIGIN_X = 0.5f;
        public const float CENTER_ORIGIN_Y = 0.5f;

        // viewport x [CENTER ORIGIN]
        public const float RIGHT_PADDLE_X_POSITION = 0.95f - CENTER_ORIGIN_X;
        public const float LEFT_PADDLE_X_POSITION = -RIGHT_PADDLE_X_POSITION;  //= (1.0f - (RIGHT_PADDLE_X_POSITION + CENTER_ORIGIN_X)) - CENTER_ORIGIN_X;

        // viewport vector [CENTER ORIGIN]: position 
        public static readonly Vector2 RIGHT_PADDLE_START_POSITION = new Vector2(RIGHT_PADDLE_X_POSITION, (0.5f - CENTER_ORIGIN_Y)); // midpoint on y-axis
        public static readonly Vector2 LEFT_PADDLE_START_POSITION = new Vector2(LEFT_PADDLE_X_POSITION, (0.5f - CENTER_ORIGIN_Y));   // midpoint on y-axis

        // controls for each human player
        public static readonly PlayerControls RIGHT_PADDLE_CONTROLS = new PlayerControls(KeyCode.UpArrow, KeyCode.DownArrow);
        public static readonly PlayerControls LEFT_PADDLE_CONTROLS = new PlayerControls(KeyCode.W, KeyCode.S);

        // viewport vector: (thickness, scale)
        //public static readonly Vector2 DEFAULT_PADDLE_DIMENSIONS = new Vector2(0.025f, 0.175f);

        public static readonly char[] WINDOWS_BANNED_CHARS = {'\\', '/', ':', '*', '?', '\"', '<', '>', '|'};
    }

    public static class GameCache {
        public static Transform BG_TRANSFORM;
    }

    public partial class GameManager {}
}