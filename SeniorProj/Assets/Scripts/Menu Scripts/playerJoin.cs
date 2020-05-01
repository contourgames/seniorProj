using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.ControllerExtensions;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class playerJoin : MonoBehaviour
{
    public Player player1;
    public Player player2;
    public Player player3;
    public Player player4;

    public List<GameObject> playerObj = new List<GameObject>();
    public List<Player> playerList = new List<Player>();
    void Awake()
    {

        ReInput.ControllerConnectedEvent += OnControllerConnected;
        foreach (Joystick j in ReInput.controllers.Joysticks)
        {
            if (ReInput.controllers.IsJoystickAssigned(j)) continue; // Joystick is already assigned

            // Assign Joystick to first Player that doesn't have any assigned
            AssignJoystickToNextOpenPlayer(j);
        }
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;

    }
    void AssignJoystickToNextOpenPlayer(Joystick j)
    {
        foreach (Player p in ReInput.players.Players)
        {
            if (p.controllers.joystickCount > 0) continue; // player already has a joystick
            p.controllers.AddController(j, true); // assign joystick to player
            print(p.controllers.joystickCount);
            return;
        }
    }
    void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        print("Controller connected");
    }
    void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
    {
        print("Controller disconnected");
    }
    public void LoadDefaultMaps(ControllerType controllerType)
    {

    }
    public void Start()
    {
        player1 = ReInput.players.GetPlayer(0);
        player2 = ReInput.players.GetPlayer(1);
        player3 = ReInput.players.GetPlayer(2);
        player4 = ReInput.players.GetPlayer(3);
        playerList.Add(player1);
        playerList.Add(player2);
        playerList.Add(player3);
        playerList.Add(player4);
        PlayerPrefs.SetInt("P1Spawn", 0);
        PlayerPrefs.SetInt("P2Spawn", 0);
        PlayerPrefs.SetInt("P3Spawn", 0);
        PlayerPrefs.SetInt("P4Spawn", 0);

    }
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < playerList.Count; i++) {
            if (playerList[i].GetButtonDown("A/X")) {
                playerObj[i].SetActive(true);
            }
        }

        if (playerObj[0].activeSelf == true) {
            PlayerPrefs.SetInt("P1Spawn", 1);
        }
        if (playerObj[1].activeSelf == true)
        {
            PlayerPrefs.SetInt("P2Spawn", 1);

        }
        if (playerObj[2].activeSelf == true)
        {
            PlayerPrefs.SetInt("P3Spawn", 1);

        }
        if (playerObj[3].activeSelf == true)
        {
            PlayerPrefs.SetInt("P4Spawn", 1);

        }


    }
}
