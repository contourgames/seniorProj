using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnPoint : MonoBehaviour
{
    CircleCollider2D _collider;
    public bool isEmpty;
    // Start is called before the first frame update
    void Start()
    {
        isEmpty = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected");

        if(collision.tag == "Bomb" || collision.tag == "Knife"){
            isEmpty = false;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Bomb" || collision.tag == "Knife")
        {
            isEmpty = true;
        }
    }
}
