//namespace Pong.GamePlayer;
using Pong.GamePlayer;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityUtils;

using Pong;
using Pong.RL;
using Pong.GamePlayer.Force;
using Pong.Physics;
using Pong.UI;

using TMPro;

using static Pong.GameHelpers;
using static Pong.RL.Constants;

namespace Pong.GamePlayer {
    public abstract partial class MLPlayer : Player {
        public MLPlayer(PlayerData playerData, GameObject sprite, Scoreboard scoreboard) : base(playerData, sprite, scoreboard) {
            //TODO: constructor
        }

        public override void FeedData(Vector2 vpPos, Vector2 vpOpponentPos, Vector2[] ballMotion) {
            base.FeedData(vpPos, vpOpponentPos, ballMotion);

            //* ML agent makes an inference
            DataUnit dataUnit = playerData.GetMostRecent();
            float action = ActionInference(dataUnit.GetObservation());

            // Update the DataUnit
            dataUnit.Action = action;

            //* Take the action
            playerSprite.controller.commandSensors.Set(
                moveUp: (action == ACTION_MOVE_UP),
                moveDown: (action == ACTION_MOVE_DOWN),
                doNothing: (action == ACTION_NULL)
            );

            //? Debug.Log() the values?
        }

        public override void ScorePoint() {
            base.ScorePoint();

            //* Update ML Agent
            OnScore();
        }

        //* Abstract Methods
        public abstract float ActionInference(float[] observation);
        public abstract void OnScore();
    }

    public partial class RLPlayer : MLPlayer {
        public RLPlayer(PlayerData playerData, GameObject sprite, Scoreboard scoreboard) : base(playerData, sprite, scoreboard) {
            //TODO: constructor
        }

        public override float ActionInference(float[] observation) {
            //TODO
            return ACTION_NULL;
        }

        public override void OnScore() {
            //TODO
        }
    }
}