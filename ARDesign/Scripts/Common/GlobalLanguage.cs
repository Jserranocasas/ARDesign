using GoogleARCore.Examples.ObjectManipulation;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GlobalLanguage : MonoBehaviour
{
    public static string CurrentLanguage;

    void Start() {
        CurrentLanguage = ManipulationSystem.Instance.CurrentIdiom;
        ChangeLanguage(CurrentLanguage);
    }

    public void ChangeLanguage(string idiom){
        CurrentLanguage = idiom;
        ManipulationSystem.Instance.CurrentIdiom = idiom;
        NotificationCenter.DefaultCenter().PostNotification(this, "UpdateLanguage");
    }

}