using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrapping : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -6)
        {
            transform.position = new Vector2(transform.position.x, -transform.position.y);
        } else if(transform.position.y > 6)
        {
            transform.position = new Vector2(transform.position.x, -transform.position.y);
        }

        if(transform.position.x < -9.5)
        {
            transform.position = new Vector2(-transform.position.x, transform.position.y);
        }
        else if(transform.position.x > 9.5)
        {
            transform.position = new Vector2(-transform.position.x, transform.position.y);
        }
    }
}
