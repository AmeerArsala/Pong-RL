// * NAMESPACE HEADER FILE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pong.GamePlayer;

namespace Pong {
    public static class GameConstants {
        // must be >= 1 because velocity is required
        public const uint BALL_Y_MAX_DERIVATIVE = 3; // velocity + (acceleration, acceleration')

        public const uint MAX_SCORE = 999; // any more than that and the UI kinda clips
        public const uint DEFAULT_WIN_SCORE = 11; // 11 points is a win in the original Pong game!

        // used for abnormal shots
        public const float PADDLE_MASS = 1.00f;

        // viewport y
        public const float BALL_SCALE_Y = 0.05f;

        // constants for if origin of a viewport is in the middle of the screen
        public const float CENTER_ORIGIN_X = 0.5f;
        public const float CENTER_ORIGIN_Y = 0.5f;

        // viewport x [CENTER ORIGIN]
        public const float RIGHT_PADDLE_X_POSITION = 0.95f - CENTER_ORIGIN_X; // 95% of the horizontal portion of the screen
        public const float LEFT_PADDLE_X_POSITION = -RIGHT_PADDLE_X_POSITION; //= (1.0f - (RIGHT_PADDLE_X_POSITION + CENTER_ORIGIN_X)) - CENTER_ORIGIN_X;

        // viewport vector [CENTER ORIGIN]: position
        public static readonly Vector2 BALL_START_POSITION = new Vector2((0.5f - CENTER_ORIGIN_X), (0.5f - CENTER_ORIGIN_Y)); // start in the middle of the screen
        public static readonly Vector2 RIGHT_PADDLE_START_POSITION = new Vector2(RIGHT_PADDLE_X_POSITION, (0.5f - CENTER_ORIGIN_Y)); // midpoint on y-axis
        public static readonly Vector2 LEFT_PADDLE_START_POSITION = new Vector2(LEFT_PADDLE_X_POSITION, (0.5f - CENTER_ORIGIN_Y));   // midpoint on y-axis

        // controls for each human player
        public static readonly PlayerControls RIGHT_PADDLE_CONTROLS = new PlayerControls(KeyCode.UpArrow, KeyCode.DownArrow);
        public static readonly PlayerControls LEFT_PADDLE_CONTROLS = new PlayerControls(KeyCode.W, KeyCode.S);

        public static readonly char[] WINDOWS_BANNED_CHARS = {'\\', '/', ':', '*', '?', '\"', '<', '>', '|'};
    }

    public static class GameCache {
        // cached at the beginning of GameManager
        public static Transform BG_TRANSFORM;

        // these are context passed into GameManager and set by it
        public static float PLAYER_SPEED_VP; // in terms of viewport percentage Y
        public static float BALL_SPEED_VP;   // in terms of viewport percentage X
        public static float BALL_SERVE_MAX_ANGLE;  // in radians
        public static float BALL_BOUNCE_MAX_ANGLE; // in radians
        public static uint WIN_SCORE;

        // A single hotkey (likely M) will mute the sounds
        public static bool MUTE_SOUNDS = false; // when turned on, audio won't be played
    }

    public static class GameHelpers {
        public static Vector2 ToVector2(Vector3 vector) {
            return new Vector2(vector.x, vector.y);
        }

        public static Vector3 ToLocal(Vector2 viewportVec) {
            Vector3 bgScale2f = ToVector2(GameCache.BG_TRANSFORM.localScale);

            Vector2 localVec2f = viewportVec * bgScale2f;
            Vector3 localVec = new Vector3(localVec2f.x, localVec2f.y, 0f);

            return localVec;
        }

        public static Vector2 ToViewport(Vector3 localVec) {
            Vector2 localVec2f = ToVector2(localVec);
            Vector2 bgScale2f = ToVector2(GameCache.BG_TRANSFORM.localScale);

            Vector2 viewportVec = localVec2f / bgScale2f;

            return viewportVec;
        }

        public static float ToLocalX(float vpX) {
            return GameCache.BG_TRANSFORM.localScale.x * vpX;
        }

        public static float ToLocalY(float vpY) {
            return GameCache.BG_TRANSFORM.localScale.y * vpY;
        }

        public static float ToViewportX(float localX) {
            return localX / GameCache.BG_TRANSFORM.localScale.x;
        }

        public static float ToViewportY(float localY) {
            return localY / GameCache.BG_TRANSFORM.localScale.y;
        }
    }

    public partial class GameManager : MonoBehaviour {}

    //public partial class GameContext {}
}