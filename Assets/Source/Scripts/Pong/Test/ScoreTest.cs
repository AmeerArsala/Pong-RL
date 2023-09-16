using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using System;

// * TEST BEHAVIOR: LSHIFT => +1 for P2, RSHIFT => +1 for P1. GUI must update too.
namespace Pong.Test {
    public class ScoreTest : MonoBehaviour 
    {
        public TMP_Text p1Text; // right
        public TMP_Text p2Text; // left

        private float p1score = 0;
        private float p2score = 0;

        // Start is called before the first frame update
        void Start() 
        {
            Debug.Log("Hello World! This is a test for the scoreboards!");
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightShift)) {
                // +1 for P1
                ++p1score;
                
                // update the text
                p1Text.text = "" + p1score;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift)) {
                // +1 for P2
                ++p2score;

                // update the text
                p2Text.text = "" + p2score;
            }
        }
    }
}
