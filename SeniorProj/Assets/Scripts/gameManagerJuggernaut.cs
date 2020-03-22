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

    public List<GameObject> playerList = new List<GameObject>();
    public List<Player> playerControls = new List<Player>();
    public GameObject orbPrefab;

    GameObject orb;
    // Start is called before the first frame update
    void Start()
    {
        orb = Instantiate(orbPrefab);
        orb.transform.position = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void IncreasePlayerScore(PlayerMovement player) {

        player.score += 1f * Time.deltaTime;
    }

    public IEnumerator Respawn(GameObject playerObj) {

        playerObj.SetActive(false);
        yield return new WaitForSeconds(2f);
        playerObj.SetActive(true);

    }
    public IEnumerator Iframes(GameObject playerObj, PlayerMovement _playerController) {

        _playerController.enableHurt = false;
        yield return new WaitForSeconds(3f);
        _playerController.enableHurt = true;

    }
}
