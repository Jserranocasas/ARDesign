namespace GoogleARCore.Examples.Common
{
    using System.Collections;
    using System.Collections.Generic;
    using GoogleARCore;
    using UnityEngine.UI;
    using UnityEngine;

    public class ShowDetected : MonoBehaviour
    {
        private Toggle m_Toggle;

        public static bool ShowPlanes = true;

        void Start()
        {
            ShowPlanes = true;

            //Fetch the Toggle GameObject
            m_Toggle = GetComponent<Toggle>();
            //Add listener for when the state of the Toggle changes, to take action
            m_Toggle.onValueChanged.AddListener(delegate
            {
                ToggleValueChanged(m_Toggle);
            });
        }

        //Output the new state of the Toggle into Text
        void ToggleValueChanged(Toggle change)
        {
            if(ShowPlanes){
                List<GameObject> m_DetectedPlanes = Session.GetDetectedPlanes();
                foreach (GameObject planeVisualizer in m_DetectedPlanes)
                {
                    planeVisualizer.SetActive(false);
                }

                ShowPlanes = false;
            } 
            else
            {
                ShowPlanes = true;

                List<GameObject> m_DetectedPlanes = Session.GetDetectedPlanes();
                foreach (GameObject planeVisualizer in m_DetectedPlanes)
                {
                    planeVisualizer.SetActive(true);
                }
            }
        }
    }
}