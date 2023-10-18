// * NAMESPACE HEADER FILE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pong.GamePlayer {
    //* Players
    public partial class Player {}
    /*public partial class HumanPlayer : Player {}
    public abstract partial class AIPlayer : Player {}
    public partial class ComputerPlayer : AIPlayer {}
    public partial class RLPlayer : AIPlayer {}*/

    public partial class PlayerData : ScriptableObject {}
    //public partial class AIPlayerData : PlayerData {}

    public partial class PlayerController : MonoBehaviour {}

    public class PlayerControls {
        public readonly KeyCode Up, Down;
        public PlayerControls(KeyCode up, KeyCode down) {
            Up = up;
            Down = down;
        }
    }
}