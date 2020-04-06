using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public Image P1Bar;
    public Image P2Bar;
    public Image P3Bar;
    public Image P4Bar;
    public gameManagerJuggernaut _scoreScript;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        P1Bar.GetComponent<Image>().fillAmount = (_scoreScript.P1Score / 20f) ;
        P2Bar.GetComponent<Image>().fillAmount = (_scoreScript.P2Score / 20f) ;
        P3Bar.GetComponent<Image>().fillAmount = (_scoreScript.P3Score / 20f) ;
        P4Bar.GetComponent<Image>().fillAmount = (_scoreScript.P4Score / 20f) ;
    }
}
