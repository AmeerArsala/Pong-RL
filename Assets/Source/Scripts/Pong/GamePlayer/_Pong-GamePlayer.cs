// * NAMESPACE HEADER FILE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pong.GamePlayer {
    public partial class Player {}

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