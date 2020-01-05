
namespace GoogleARCore.Examples.ObjectManipulation
{
    using UnityEngine.SceneManagement;
    using System.Collections.Generic;
    using UnityEngine.EventSystems;
    using GoogleARCoreInternal;
    using System.Collections;
    using GoogleARCore.Examples.Common;
    using UnityEngine.UI;
    using UnityEngine;

    /// <summary>
    /// 
    /// </summary>
    public class ScreenManager : MonoBehaviour
    {

        /// <summary>
        /// Screen to open automatically at the start of the Scene
        /// </summary>
        public Animator initiallyOpen;

        /// <summary>
        /// Screen to open automatically options menu
        /// </summary>
        public Animator OptionsMenu;

        /// <summary>
        /// Animator State and Transition names we need to check against.
        /// </summary>
        const string    k_SelectedTransitionName = "Selected", 
                        k_OpenTransitionName = "Open", 
                        k_InitTransitionName = "Init",
                        k_HiddenTransitionName = "Hidden";
                        

        private void Start()
        {
            if (initiallyOpen.gameObject.activeSelf)
            {
                // Don't show the main panel firstly 
                initiallyOpen.SetBool(k_SelectedTransitionName, false);
                initiallyOpen.SetBool(k_OpenTransitionName, false);
                initiallyOpen.SetBool(k_InitTransitionName, true);
            }

            if (OptionsMenu.gameObject.activeSelf)
            {
                OptionsMenu.SetBool(k_HiddenTransitionName, true);
            }
        }

        /// <summary>
        /// Opens the main panel with a animation
        /// </summary>
        /// <param name="anim">Animation to show</param>
        public void OpenPanelInitial()
        {
            if (initiallyOpen == null)
            {
                return;
            }

            initiallyOpen.enabled = true;

            if(initiallyOpen.GetBool(k_SelectedTransitionName))
            {
                initiallyOpen.SetBool(k_SelectedTransitionName, false);
            }

            // Start the open animation
            initiallyOpen.SetBool(k_OpenTransitionName, true);

            // Change to selection mode
            ModeAction.Instance.setMode(ModeStatus.Selection);  
        }

        /// <summary>
        /// Closes the main Screen with a animation
        /// </summary>
        /// <param name="anim">Animation to show</param>
        public void ClosePanelInitial()
        {
            if (initiallyOpen == null)
            {
                return;
            }

            // Start the close animation.
            initiallyOpen.SetBool(k_OpenTransitionName, false);

            // Change to insertion mode
            ModeAction.Instance.setMode(ModeStatus.Insertion);
        }

        /// <summary>
        /// Opens the provided one panel
        /// </summary>
        /// <param name="obj">GameObject to be open</param>
        public void OpenPanel(GameObject obj)
        {
            if (obj == null)
            {
                return;
            }

            // Activate the new Screen hierarchy so we can animate it.
            StartCoroutine(WaitAndTurnOnOff(obj, 0.01f, true));

            // initiallyOpen.GetComponent<Animator>().enabled = false;
            initiallyOpen.SetBool(k_InitTransitionName, false);
        }

        /// <summary>
        /// Closes the open Screen which is passed by parameter
        /// </summary>
        /// <param name="obj">GameObject to be closed</param>
        public void ClosePanel(GameObject obj)
        {
            if (obj == null)
            {
                return;
            }

            initiallyOpen.SetBool(k_InitTransitionName, true);

            // Start the close animation.
            StartCoroutine(WaitAndTurnOnOff(obj, 0.01f, false));
        }

        /// <summary>
        /// Closes the menu as selected object
        /// </summary>
        /// <param name="obj">GameObject to be closed</param>
        public void ClosePanelCompletely(GameObject obj)
        {
            if (obj == null)
            {
                return;
            }

            initiallyOpen.enabled = false;
            initiallyOpen.SetBool(k_SelectedTransitionName, true);
            initiallyOpen.SetBool(k_OpenTransitionName, false);
            initiallyOpen.SetBool(k_InitTransitionName, true);

            obj.gameObject.SetActive(false);
        }

        /// <summary>
        /// Firstly wait a few seconds to turn off or turn on animation
        /// </summary>
        /// <param name="obj">Object to enable or disable</param>
        /// <param name="seconds">Seconds to wait</param>
        /// <param name="turnOn">Define if the object will be enable or disable</param>
        IEnumerator WaitAndTurnOnOff(GameObject obj, float seconds, bool turnOn)
        {
            yield return new WaitForSeconds(seconds);

            obj.gameObject.SetActive(turnOn);
        }

        public void AcceptInsertedObject()
        {
            ModeAction.Instance.setMode(ModeStatus.Insertion);

            var manipulator = ManipulationSystem.Instance.SelectedObject.GetComponent<Manipulator>();

            manipulator.Deselect();
        }

        public void GoInitMenu()
        {
            Session.Reset();
            ShowDetected.ShowPlanes = true;
            SceneManager.LoadScene("Menu");
        }

        public void GoTutorial()
        {
            SceneManager.LoadScene("Tutorial");
        }

        public void HiddenVisibleButton(){
            if(OptionsMenu.GetBool(k_HiddenTransitionName))
            {
                OptionsMenu.SetBool(k_HiddenTransitionName, false);
                ModeAction.Instance.setMode(ModeStatus.Option);
            }
            else
            {
                OptionsMenu.SetBool(k_HiddenTransitionName, true);
                ModeAction.Instance.setMode(ModeStatus.Insertion);
            }
        }
    }
}