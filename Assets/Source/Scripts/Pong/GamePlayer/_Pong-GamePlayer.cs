// * NAMESPACE HEADER FILE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pong.GamePlayer {
    //* Players
    public partial class Player {}

    // Human Player
    public partial class HumanPlayer : Player {}

    // Cheating Computer Player
    public partial class CheatingPlayer : Player {}

    // AI Players
    /*public abstract partial class MLPlayer : Player {}
    public partial class RLPlayer : MLPlayer {}*/

    //* PlayerData
    public partial class PlayerData : ScriptableObject {}
    //public partial class AIPlayerData : PlayerData {}

    //* Interfacing Player Movement
    public partial class PlayerController : MonoBehaviour { }

    //* Sense commands
    public class PlayerCommandSensors {
        public bool Move_Up = false;
        public bool Move_Down = false;
        public bool Do_Nothing = false;

        public PlayerCommandSensors() {}

        public void Set(bool moveUp, bool moveDown, bool doNothing) {
            Move_Up = moveUp;
            Move_Down = moveDown;
            Do_Nothing = doNothing;
        }

        public void Sense(bool moveUp, bool moveDown) {
            Move_Up = moveUp;
            Move_Down = moveDown;
            Do_Nothing = (moveUp == moveDown); // if both are true, they cancel each other out. if both are false, no need to do anything
        }

        public void Reset() {
            Set(false, false, false);
        }
    }

    public class PlayerControls {
        public readonly KeyCode Up, Down;
        public PlayerControls(KeyCode up, KeyCode down) {
            Up = up;
            Down = down;
        }
    }
}