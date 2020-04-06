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
    [Header("Game Scores & floats")]
    public float startDelay = 4f;
    public float maxScore;
    public float P1Score = 0f;
    public float P2Score = 0f;
    public float P3Score = 0f;
    public float P4Score = 0f;
    [Space]
    [Header ("Lists")]
    public List<float> _scoreList = new List<float>();
    public List<GameObject> playerList = new List<GameObject>();
    public List<Player> playerControls = new List<Player>();
    public GameObject orbPrefab;
    [Space]
    [Header("Bools")]
    public bool gameStart;
    public bool gameOver;
    public bool playersCanMove;
    public bool orbRespawn;

    public GameObject countDown;
    //public Text P1ScoreText;
    //public Text P2ScoreText;
    //public Text P3ScoreText;
    //public Text P4ScoreText;

    GameObject orb;
    // Start is called before the first frame update
    void Start()
    {
        _scoreList.Add(P1Score);
        _scoreList.Add(P2Score);
        _scoreList.Add(P3Score);
        _scoreList.Add(P4Score);
        gameStart = false;
        gameOver = false;
        playersCanMove = false;
        orb = Instantiate(orbPrefab);
        orb.transform.position = new Vector2(0, 0);

        orb.SetActive(false);

        orbRespawn = false;
    }

    public void FixedUpdate()
    {
        //3 second countdown
        startDelay -= Time.deltaTime;
        countDown.GetComponent<Text>().text = "" +(int)startDelay;
        if (startDelay <= 0) {
            startDelay = 0;
        }

        if (startDelay < 1)
        {
            gameStart = true;
            countDown.SetActive(false);
            orb.SetActive(true);
            playersCanMove = true;
        }
        else {
            gameStart = false;
        }

        //Debug.Log((int)startDelay);
    }

    // Update is called once per frame
    void Update()
    {
        #region Score Text UI
        //if(SceneManager.GetActiveScene().name == "SampleScene - Copy")
        //{

        //P1ScoreText.text = "Player 1: " + P1Score.ToString("F0");
        //P2ScoreText.text = "Player 2: " + P2Score.ToString("F0");
        //P3ScoreText.text = "Player 3: " + P3Score.ToString("F0");
        //P4ScoreText.text = "Player 4: " + P4Score.ToString("F0");
        //}
        #endregion

        _scoreList[0] = P1Score;
        _scoreList[1] = P2Score;
        _scoreList[2] = P3Score;
        _scoreList[3] = P4Score;

        for (int i = 0; i < playerList.Count; i++) { // If a player gets hit by a bomb start respawn and Iframe coroutines
            GameObject _player = playerList[i];
            playerCollision _pc = playerList[i].GetComponent<playerCollision>();
            if (_pc.gotHit) {
               
                StartCoroutine(Respawn(_player, _pc.spawnPosition));
                _pc.gotHit = false;
             
            }
        }
        for (int i = 0; i < _scoreList.Count; i++) {
            if (_scoreList[i] >= maxScore) {
                _scoreList[i] = maxScore; //cap the player score;
                //Debug.Log("A");
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
    public void DecreasePlayerScore(GameObject player) {
        if (!gameOver)
        {
            if (player.name == "Player")
            {
                P1Score -= 1f * Time.deltaTime;
            }
            else if (player.name == "Player 2")
            {
                P2Score -= 1f * Time.deltaTime;
            }
            else if (player.name == "Player 3")
            {
                P3Score -= 1f * Time.deltaTime;
            }
            else if (player.name == "Player 4")
            {
                P4Score -= 1f * Time.deltaTime;
            }
        }
    }
    public void GameOver() {
        gameOver = true;
        Debug.Log("Game Over");

        for (int i = 0; i < _scoreList.Count; i++) {

            if (_scoreList[0] == maxScore) {
                Debug.Log("Player 1 Wins");
            }
            else if (_scoreList[1] == maxScore)
            {
                Debug.Log("Player 2 Wins");
            }
            else if(_scoreList[2] == maxScore)
            {
                Debug.Log("Player 3 Wins");
            }
            else if(_scoreList[3] == maxScore)
            {
                Debug.Log("Player 4 Wins");
            }
        }
    }

    //Displaying rounded score numbers
    /*
        int temp = Mathf.RoundToInt(score);
        Debug.Log(gameObject.transform.name + " Score: " + temp);
    */

    public void RespawnOrb()
    {
        Destroy(orb.gameObject);
        GameObject neworb = Instantiate(orbPrefab);
        neworb.transform.position = new Vector2(0, 0);
    }
    public IEnumerator Respawn(GameObject playerObj, Vector2 spawnPoint) {

        GameObject _playerObj = playerObj;
        PlayerMovement _movementSc = _playerObj.GetComponent<PlayerMovement>();

        _playerObj.SetActive(false);
        _playerObj.transform.position = spawnPoint;

        yield return new WaitForSeconds(2f);

        _playerObj.SetActive(true);
        StartCoroutine(Iframes(_movementSc));
    }
    public IEnumerator Iframes(PlayerMovement _playerController) {

        _playerController.enableHurt = false;
        yield return new WaitForSeconds(3f);
        _playerController.enableHurt = true;
    }

}
