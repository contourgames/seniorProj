using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayQuit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(gameObject.name == "PlayCollider")
        {
            SceneManager.LoadScene("SampleScene");
         Debug.Log("PlayGame");
		}

        if(gameObject.name == "QuitCollider")
        {
        Application.Quit();
         Debug.Log("QuitGame"); 

		}
	}
}
