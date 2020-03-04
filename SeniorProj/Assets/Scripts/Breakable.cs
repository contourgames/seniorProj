using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{

    public int HitsNeeded;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(HitsNeeded <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D (Collision2D coll)
    {
        if(coll.gameObject.tag == "Throwable")
        {
            HitsNeeded--;
        }
    }
}
