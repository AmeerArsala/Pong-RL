namespace Pong.Player;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  For Pong RL, we will only care about the statistics of:
     *   Y position of Paddle
     *   Ball's relative distance:  vec2[X distance away (non-negative) from goal, Y distance away from Paddle]
     *   Ball's velocity: vec2[X speed towards goal, Y]
     ?   Observed Y position of the other Paddle
     ??  Observed Y velocity of the other Paddle; maybe delegate the observation of this to another model?
*/
public class PlayerData : ScriptableObject {
    public PlayerData() {}
}