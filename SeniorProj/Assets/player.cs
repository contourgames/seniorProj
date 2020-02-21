using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
public class player : MonoBehaviour
{
    // Start is called before the first frame update
    public Player playerObj;
    float x;
    float y;

    void Start()
    {
        playerObj = ReInput.players.GetPlayer(0);
    }

    // Update is called once per frame
    void Update()
    {
        x = playerObj.GetAxis("Horizontal");
        y = playerObj.GetAxis("Vertical");

    }
}
