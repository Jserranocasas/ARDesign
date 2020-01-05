//-----------------------------------------------------------------------
// <copyright file="AndyPlacementManipulator.cs" company="Google">
//
// Copyright 2018 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.Examples.ObjectManipulation
{
    using GoogleARCore;
    using UnityEngine;
    using System.Collections;

    /// <summary>
    /// Controls the placement of Andy objects via a tap gesture.
    /// </summary>
    public class AndyPlacementManipulator : Manipulator
    {
        /// <summary>
        /// The first-person camera being used to render the passthrough camera image (i.e. AR
        /// background).
        /// </summary>
        public Camera FirstPersonCamera;

        /// <summary>
        /// A model to place when a raycast from a user touch hits a plane.
        /// </summary>
        private GameObject AndyPrefab = null;

        /// <summary>
        /// Manipulator prefab to attach placed objects to.
        /// </summary>
        public GameObject ManipulatorPrefab;

        /// <summary>
        /// The rotation in degrees need to apply to model when the Andy model is placed.
        /// </summary>
        private const float k_ModelRotation = 180.0f;

        /// <summary>
        /// Returns true if the manipulation can be started for the given gesture.
        /// </summary>
        /// <param name="gesture">The current gesture.</param>
        /// <returns>True if the manipulation can be started.</returns>
        protected override bool CanStartManipulationForGesture(TapGesture gesture)
        {
            if (gesture.TargetObject == null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Modify the andy prefab by another object of furniture type
        /// </summary>
        /// <param name="newPrefab">The new prefab of furniture type</param>
        public void SetAndyPrefab(GameObject newPrefab)
        {
            //Define prefab to be inserted
            AndyPrefab = newPrefab;

            //Change to insertion mode
            ModeAction.Instance.setMode(ModeStatus.Insertion);
        }

        

        /// <summary>
        /// Function called when the manipulation is ended.
        /// </summary>
        /// <param name="gesture">The current gesture.</param>
        protected override void OnEndManipulation(TapGesture gesture)
        {
            if (gesture.WasCancelled)
            {
                return;
            }

            // If gesture is targeting an existing object we are done.
            if (gesture.TargetObject != null)
            {
                return;
            }

            // Raycast against the location the player touched to search for planes.
            TrackableHit hit;
            // TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinInfinity;

            if(ModeAction.Instance.getMode() == ModeStatus.Insertion){
                if (Frame.Raycast(
                    gesture.StartPosition.x, gesture.StartPosition.y, raycastFilter, out hit))
                {
                    // Use hit pose and camera pose to check if hittest is from the
                    // back of the plane, if it is, no need to create the anchor.
                    if ((hit.Trackable is DetectedPlane) &&
                        Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                            hit.Pose.rotation * Vector3.up) < 0)
                    {
                        Debug.Log("Hit at back of the current DetectedPlane");
                    }
                    else
                    {
                        // Instantiate Andy model at the hit pose.
                        var andyObject = Instantiate(AndyPrefab, hit.Pose.position, hit.Pose.rotation);

                        // Instantiate manipulator.
                        var manipulator =
                            Instantiate(ManipulatorPrefab, hit.Pose.position, hit.Pose.rotation);

                        // Make Andy model a child of the manipulator.
                        andyObject.transform.parent = manipulator.transform;

                        // Compensate for the hitPose rotation facing away from the raycast (i.e.
                        // camera).
                        andyObject.transform.Rotate(0, k_ModelRotation, 0, Space.Self);
                        
                        // Create an anchor to allow ARCore to track the hitpoint as understanding of
                        // the physical world evolves.
                        var anchor = hit.Trackable.CreateAnchor(hit.Pose);

                        // Make manipulator a child of the anchor.
                        manipulator.transform.parent = anchor.transform;

                        // Select the placed object.
                        Session.AddInsertedObject(manipulator.GetHashCode().ToString(), andyObject);
                        
                        // manipulator.GetComponent<Manipulator>().Select(andyObject);
                        manipulator.GetComponent<Manipulator>().Select();
                    }
                }
            }
            else if (ModeAction.Instance.getMode() == ModeStatus.Edition)
            {
                StartCoroutine(WaitToInsert(0.5f));
            }
        }

        /// <summary>
        /// Firstly wait a few seconds and then change mode to insertion
        /// </summary>
        /// <param name="seconds">Seconds to wait</param>
        IEnumerator WaitToInsert(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            //Change to insertion mode
            ModeAction.Instance.setMode(ModeStatus.Insertion);
        }
    }
}
