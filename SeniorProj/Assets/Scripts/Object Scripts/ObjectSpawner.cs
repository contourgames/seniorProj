using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{

    public List<GameObject> bombSpawner = new List<GameObject>();

    public List<Vector3> spawnPos = new List<Vector3>();
    public List<GameObject> spawnPoints = new List<GameObject>();

    public GameObject bombPrefab;

    public bool hasSpawned;
    // Start is called before the first frame update
    void Start()
    {

        for(int i = 0; i <bombSpawner.Count; i++)
        {
           spawnPos.Add(bombSpawner[i].transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < bombSpawner.Count; i++)
        {
            if (bombSpawner[i].gameObject.GetComponent<Bomb>().hasDetonated)
            {
                GameObject newBomb = Instantiate(bombPrefab);
                bombSpawner[i].GetComponent<Bomb>().hasSpawned = true;
                bombSpawner.Insert(bombSpawner.IndexOf(bombSpawner[i]), newBomb);
                bombSpawner.RemoveAt(bombSpawner.IndexOf(bombSpawner[i]) + 1);
                GetNewSpawnPoint(newBomb);
                //newBomb.transform.position = new Vector3(spawnPos[i].x, spawnPos[i].y, spawnPos[i].z);
                //spawnPos[Random.Range(0, spawnPos.Count)];
                //new Vector3(spawnPos[i].x, spawnPos[i].y, spawnPos[i].z);
                
            }
        }
    }

    void GetNewSpawnPoint(GameObject newBomb) {


        for (int i = Random.Range(0,spawnPoints.Count - 1); i < spawnPoints.Count;) {
            if (spawnPoints[i].GetComponent<spawnPoint>().isEmpty) {
                 newBomb.transform.position = spawnPoints[i].transform.position;
                Debug.Log("Empty spawn point found.");
                return;
            }
            else{
                    Debug.Log("Respawn point was not null, checking for a new point...");

                i++;
            }
        }

    }
}
