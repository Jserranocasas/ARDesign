using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InternationalText : MonoBehaviour
{
    public string Spanish, English, French;

    public bool IsButton;

    // Start is called before the first frame update
    void Start()
    {
        NotificationCenter.DefaultCenter().AddObserver(this, "UpdateLanguage");
        UpdateLanguage();
    }

    public void UpdateLanguage(){
        if(GlobalLanguage.CurrentLanguage == "Spanish"){
            if(IsButton)
            {
                GetComponentInChildren<Text>().text = Spanish;
            }
            else
            {
                GetComponent<Text> ().text = Spanish;
            }
        }

        if (GlobalLanguage.CurrentLanguage == "English")
        {
            if (IsButton)
            {
                GetComponentInChildren<Text>().text = English;
            }
            else
            {
                GetComponent<Text>().text = English;
            }
        }

        if (GlobalLanguage.CurrentLanguage == "French")
        {
            if (IsButton)
            {
                GetComponentInChildren<Text>().text = French;
            }
            else
            {
                GetComponent<Text>().text = French;
            }
        }
    }
}
