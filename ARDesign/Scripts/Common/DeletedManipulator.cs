namespace GoogleARCore.Examples.ObjectManipulation
{
    using GoogleARCore.Examples.ObjectManipulationInternal;
    using System.Collections;
    using UnityEngine.UI;
    using UnityEngine;

    /// <summary>
    /// Controls the deleted of an object via a deleted button.
    /// </summary>
    public class DeletedManipulator : Manipulator
    {
        /// <summary>
        /// Function called when a game object is selected and wants to be erased.
        /// </summary>
        public void DeleteSelectedObject(){
            ModeAction.Instance.setMode(ModeStatus.Insertion);

            Destroy(ManipulationSystem.Instance.SelectedObject.gameObject);
        }
    }

}
