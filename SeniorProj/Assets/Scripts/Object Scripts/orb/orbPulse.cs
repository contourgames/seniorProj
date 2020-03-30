using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orbPulse : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(transform.position);
        transform.localScale = new Vector3(transform.localScale.x + 0.05f, transform.localScale.y + 0.05f, transform.localScale.z);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {

    }

}
