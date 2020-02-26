using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ReadyUpTimer : MonoBehaviour
{
    //https://answers.unity.com/questions/64498/time-counter-up.html

    public Text timerText;
    private float secondsCount = 0;
    private int minuteCount = 0;

    private bool isTiming = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isTiming)
        {
            secondsCount += Time.deltaTime;
        }
        
        timerText.text = minuteCount + ":" + secondsCount.ToString("F2");
        if (secondsCount >= 60)
        {
            minuteCount++;
            secondsCount = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        isTiming = false;
    }
}
