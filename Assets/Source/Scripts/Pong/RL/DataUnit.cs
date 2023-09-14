//namespace RL;
using RL;

using System.Collections;
using System.Collections.Generic;

using static RL.Constants;

//TODO: use something like GSON here; purely for data storage and not necessarily an object we want to really keep track of.
public partial class DataUnit {
    private readonly float[] observation;
    private float action;
    private float reward;

    public DataUnit(float[] observation, float action, float reward) {
        this.observation = observation;
        this.action = action;
        this.reward = reward;
    }

    public DataUnit(float[] observation) {
        this.observation = observation;
        action = ACTION_NULL;
        reward = REWARD_NULL;
    }

    public float[] GetObservation() { 
        return observation; 
    }

    public float Action {
        get { return action; }
        set { action = value; }
    }

    public float Reward {
        get { return reward; }
        set { reward = value; }
    }
}