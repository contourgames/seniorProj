using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class gameManagerJuggernaut : MonoBehaviour
{
    public Player player1;
    public Player player2;
    public Player player3;
    public Player player4;
    [Header("Game Scores")]
    public float maxScore = 25f;
    public float P1Score = 0f;
    public float P2Score = 0f;
    public float P3Score = 0f;
    public float P4Score = 0f;
    [Space]
    [Header ("Lists")]
    List<float> _scoreList = new List<float>();
    public List<GameObject> playerList = new List<GameObject>();
    public List<Player> playerControls = new List<Player>();
    public GameObject orbPrefab;
    [Space]
    [Header("Bools")]
    public bool gameOver;

    GameObject orb;
    // Start is called before the first frame update
    void Start()
    {
        _scoreList.Add(P1Score);
        _scoreList.Add(P2Score);
        _scoreList.Add(P3Score);
        _scoreList.Add(P4Score);
        gameOver = false;
        orb = Instantiate(orbPrefab);
        orb.transform.position = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < playerList.Count; i++) { // If a player gets hit by a bomb start respawn and Iframe coroutines
            GameObject _player = playerList[i];
            playerCollision _pc = playerList[i].GetComponent<playerCollision>();
            if (_pc.gotHit) {
                StartCoroutine(Respawn(_player));
                _pc.gotHit = false;
            }
        }
        for (int i = 0; i < _scoreList.Count; i++) {
            if (_scoreList[i] >= maxScore) {

                GameOver();    
                
            }
        }
        
        #region Player Menu Controls
        #endregion
    }
    public void IncreasePlayerScore(GameObject player) {

        if (!gameOver) {
            if (player.name == "Player")
            {
                P1Score += 1f * Time.deltaTime;
            }
            else if (player.name == "Player 2")
            {
                P2Score += 1f * Time.deltaTime;
            }
            else if (player.name == "Player 3")
            {
                P3Score += 1f * Time.deltaTime;
            }
            else if (player.name == "Player 4")
            {
                P4Score += 1f * Time.deltaTime;
            }
        }

    }
    public void GameOver() {
        gameOver = true;
    }

    //Displaying rounded score numbers
    /*
        int temp = Mathf.RoundToInt(score);
        Debug.Log(gameObject.transform.name + " Score: " + temp);
    */

    public IEnumerator Respawn(GameObject playerObj) {
        GameObject _playerObj = playerObj;
        PlayerMovement _movementSc = _playerObj.GetComponent<PlayerMovement>();
        _playerObj.SetActive(false);
        Debug.Log("A");
        yield return new WaitForSeconds(2f);
        Debug.Log("B");

        _playerObj.SetActive(true);
        StartCoroutine(Iframes(_movementSc));
    }
    public IEnumerator Iframes(PlayerMovement _playerController) {

        _playerController.enableHurt = false;
        yield return new WaitForSeconds(3f);
        _playerController.enableHurt = true;

    }
}
