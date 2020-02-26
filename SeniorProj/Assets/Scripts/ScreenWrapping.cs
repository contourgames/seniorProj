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
        if(transform.position.y < -5.8)
        {
            transform.position = new Vector2(transform.position.x, 5.7f);
        } else if(transform.position.y > 5.8)
        {
            transform.position = new Vector2(transform.position.x, -5.7f);
        }

        if(transform.position.x < -9.5)
        {
            transform.position = new Vector2(9.45f, transform.position.y);
        }
        else if(transform.position.x > 9.5)
        {
            transform.position = new Vector2(-9.45f, transform.position.y);
        }
    }
}
