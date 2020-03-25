using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{

    public List<GameObject> bombSpawner = new List<GameObject>();

    public List<Vector3> spawnPos = new List<Vector3>();

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
                newBomb.transform.position = new Vector3(spawnPos[i].x, spawnPos[i].y, spawnPos[i].z);
              
            }
        }
    }
}
