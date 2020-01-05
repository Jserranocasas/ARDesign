using GoogleARCore.Examples.ObjectManipulation;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class UpdateToggle : MonoBehaviour
{
    public string idiom;

    private Toggle m_Toggle;

    // Start is called before the first frame update
    void Start()
    {
        //Fetch the Toggle GameObject
        m_Toggle = GetComponent<Toggle>();

        if(idiom == ManipulationSystem.Instance.CurrentIdiom)
        {
            m_Toggle.isOn = true;
        } 
        else
        {
            m_Toggle.isOn = false;
        }
    }

}
