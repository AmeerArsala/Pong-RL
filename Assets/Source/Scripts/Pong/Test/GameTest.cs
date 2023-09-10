using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// * CURRENT TEST BEHAVIOR: LSHIFT => +1 for P2, RSHIFT => +1 for P1. GUI must update too.
public class GameTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() 
    {
        Debug.Log("Hello World! This is a test!");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightShift)) {
            // +1 for P1
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            // +1 for P2
        }
    }
}
