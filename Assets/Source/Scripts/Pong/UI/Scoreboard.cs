//namespace Pong.UI;
using Pong.UI;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using System;

namespace Pong.UI {
    public partial class Scoreboard {
        private uint score = 0;
        private readonly TMP_Text scoreText;

        public Scoreboard(TMP_Text scoreText) {
            this.scoreText = scoreText;
        }

        public uint GetScore() { return score; }

        public void ScorePoint() {
            // increment score
            ++score;

            // update corresponding text
            scoreText.text = "" + score;

            //TODO: fix audio
            //PlayScoreSound();
        }

        private void PlayScoreSound() {
            AudioSource audioSource = scoreText.GetComponent<AudioSource>();

            audioSource.Play();
        }
    }
}