//namespace Pong;
using Pong;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pong.Player;

public partial class GameManager : MonoBehaviour
{
    private Player player1, player2;
    public GameObject playerPrefab; // reference in the Unity Editor; will be a sprite prefab
    
    void Start()
    {
        Debug.Log("Hello World!");

        // Initialize Players/Pong Paddles
        GameObject rightPaddle = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        
    }

    // Update is called once per frame
    void Update()
    {
        //i++;
        //Debug.Log("Hello World! x" + i);
    }
}
