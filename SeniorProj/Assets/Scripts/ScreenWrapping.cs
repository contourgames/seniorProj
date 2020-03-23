using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrapping : MonoBehaviour
{
    public PlayerMovement playerScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.layer == 9)
        {
            if (transform.position.y < -5.8)
            {
                playerScript.screenWrapping = true;
                transform.GetComponent<TrailRenderer>().enabled = false;
                transform.position = new Vector2(transform.position.x, 5.7f);
                StartCoroutine(trailTimer());
            }
            else if (transform.position.y > 5.8)
            {
                playerScript.screenWrapping = true;
                transform.GetComponent<TrailRenderer>().enabled = false;
                transform.position = new Vector2(transform.position.x, -5.7f);
                StartCoroutine(trailTimer());
            }

            if (transform.position.x < -9.5)
            {
                playerScript.screenWrapping = true;
                transform.GetComponent<TrailRenderer>().enabled = false;
                transform.position = new Vector2(9.45f, transform.position.y);
                StartCoroutine(trailTimer());

            }
            else if (transform.position.x > 9.5)
            {
                playerScript.screenWrapping = true;
                transform.GetComponent<TrailRenderer>().enabled = false;
                transform.position = new Vector2(-9.45f, transform.position.y);
                StartCoroutine(trailTimer());
            }
        }

        if(gameObject.layer == 13)
        {
            if (transform.position.y < -5.8)
            {
                
                transform.position = new Vector2(transform.position.x, 5.7f);

            }
            else if (transform.position.y > 5.8)
            {

                transform.position = new Vector2(transform.position.x, -5.7f);
            }

            if (transform.position.x < -9.5)
            {

                transform.position = new Vector2(9.45f, transform.position.y);


            }
            else if (transform.position.x > 9.5)
            {
               
                transform.position = new Vector2(-9.45f, transform.position.y);
            }
        }
    }

    public IEnumerator trailTimer()
    {
        yield return new WaitForSeconds(0.2f);
        playerScript.screenWrapping = false;

    }
}
