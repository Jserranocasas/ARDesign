namespace GoogleARCore.Examples.Common
{
    using System.Collections.Generic;
    using UnityEngine.SceneManagement;
    using GoogleARCoreInternal;
    using UnityEngine.UI;
    using GoogleARCore;
    using UnityEngine;
    
    /// <summary>
    /// Manages the visualization of detected planes in the scene.
    /// </summary>
    public class RoomPlaneGenerator : MonoBehaviour
    {
        /// <summary>
        /// Constant that will give the wall height.
        /// </summary>
        private const float WallHeight = 2.70f;

        /// <summary>
        /// A prefab for tracking and visualizing detected planes.
        /// </summary>
        public GameObject DetectedPlanePrefab;

        /// <summary>
        /// A prefab for determining detected wall.
        /// </summary>
        public GameObject DetectedVerticalPlanePrefab;

        /// <summary>
        /// A prefab for determining detected soil.
        /// </summary>
        public GameObject DetectedHorizontalPlanePrefab;

        /// <summary>
        /// A prefab for determining roof.
        /// </summary>
        public GameObject DetectedRoofPlanePrefab;

        /// <summary>
        /// GameObject List to activate when room is generated
        /// </summary>
        public GameObject[] ActiveButton;

        /// <summary>
        /// GameObject List to desactivate when room is generated
        /// </summary>
        public GameObject[] DesactiveButton;

        /// <summary>
        /// Sound to play when a plane is detected
        /// </summary>
        public AudioSource SoundDetedtedPlane;

        /// <summary>
        /// A list to hold new planes ARCore began tracking in the current frame. This object is
        /// used across the application to avoid per-frame allocations.
        /// </summary>
        private List<DetectedPlane> m_NewPlanes = new List<DetectedPlane>();

        /// <summary>
        /// A list to hold valid planes ARCore began tracking in the current frame. This object is
        /// used across the application to avoid per-frame allocations.
        /// </summary>
        private List<DetectedPlane> m_DetectedPlanes = new List<DetectedPlane>();

        /// <summary>
        /// A list to hold geometry planes.
        /// </summary>
        private List<Plane3D> m_RoomPlanes = new List<Plane3D>();

        /// <summary>
        /// A list to hold geometry points.
        /// </summary>
        private List<Vector3> m_IntersectionPoints = new List<Vector3>();

        /// <summary>
        /// A list to hold geometry points.
        /// </summary>
        private List<GameObject> PlanesObjects = new List<GameObject>();

        /// <summary>
        /// Store if the floor is detected.
        /// </summary>
        private static bool HasFloor = false;

        /// <summary>
        /// Store if the floor is detected.
        /// </summary>
        private bool IsCreate = false;

        /// <summary>
        /// Text to show.
        /// </summary>
        public Text IndicationText;

        /// <summary>
        /// Last detected normal.
        /// </summary>
        private Vector3 CurrentNormal = new Vector3(0,0,0);

        /// <summary>
        /// Constant near zero to check equality
        /// </summary>
        private const float epsilon = 0.00001f;

        /// <summary>
        /// The Unity Update method.
        /// </summary>
        public void Update()
        {
            // Check that motion tracking is tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                return;
            }
            
            // Iterate over planes found in this frame and instantiate corresponding GameObjects to
            // visualize them.
            Session.GetTrackables<DetectedPlane>(m_NewPlanes, TrackableQueryFilter.New);
            for (int i = 0; i < m_NewPlanes.Count; i++)
            {

                Vector3 planeNormal = m_NewPlanes[i].CenterPose.rotation * Vector3.up;

                // Check that don't insert more planes of type floor
                if (HasFloor){
                    if(_IsNormalFloor(planeNormal))
                    {
                        return;
                    }
                }
                else { // Check that first plane is the floor
                    if(!_IsNormalFloor(planeNormal))
                    {
                        return; 
                    }
                }

                // Check that new detected plane isn't equal to previous plane
                if(_IsSamePreviousPlane(planeNormal))
                {
                    return;
                }

                if(IsCreate){
                    return;
                }

                GameObject planeObject = Instantiate(DetectedPlanePrefab,
                            Vector3.zero, Quaternion.identity, transform);
                
                planeObject.GetComponent<DetectedPlaneVisualizer>().Initialize(m_NewPlanes[i]);
                PlanesObjects.Add(planeObject);
                ProcessPlane(m_NewPlanes[i], planeNormal);
            }
        }

        /// <summary>
        /// Check plane and instantiate the room
        /// </summary>
        /// <param name="plane">Detected plane to process.</param>
        /// <param name="normal">Normal to process.</param>
        private void ProcessPlane(DetectedPlane plane, Vector3 normal)
        {
            // Mobile vibrates, so user knows that the plane is detected
            // Discarded, as it affects negatively to the oscilloscope
            // Handheld.Vibrate(); 
            // Better uses only a sound
            SoundDetedtedPlane.Play();

            m_RoomPlanes.Add(new Plane3D(plane.CenterPose.position, normal));
            m_DetectedPlanes.Add(plane);

            // Wheter exist more than two planes, calculate planes intersection
            if (m_RoomPlanes.Count > 2)
            {
                int planes = m_RoomPlanes.Count;

                Vector3 v = new Vector3();
                if (Plane3D.IntersectPlanes(m_RoomPlanes[0],
                    m_RoomPlanes[m_RoomPlanes.Count - 2],
                    m_RoomPlanes[m_RoomPlanes.Count - 1], ref v))
                {
                    m_IntersectionPoints.Add(v);
                    v.y += WallHeight;
                    m_IntersectionPoints.Add(v);
                }
            }
        }

        /// <summary>
        /// Check that plane normal  doesn't yet exist
        /// </summary>
        /// <param name="normal">Normal to check.</param>
        /// <returns><c>true</c>, if normal is soil type, <c>false</c> otherwise.</returns>
        private bool _IsNormalFloor(Vector3 normal)
        {
            if( Mathf.Abs(normal.normalized.x) < epsilon && 
                Mathf.Abs(normal.normalized.y - 1) < epsilon && 
                Mathf.Abs(normal.normalized.z) < epsilon)
            {
                HasFloor = true;
                if (GlobalLanguage.CurrentLanguage == "Spanish")
                {
                    IndicationText.text = "Detectando superficies pared...";
                }
                if (GlobalLanguage.CurrentLanguage == "English")
                {
                    IndicationText.text = "Detecting for wall surface...";
                }
                if (GlobalLanguage.CurrentLanguage == "French")
                {
                    IndicationText.text = "Détecter la surface du mur...";
                }                
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check that plane normal  doesn't yet exist
        /// </summary>
        /// <param name="normal">Normal to check.</param>
        /// <returns><c>true</c>, if normal is soil type, <c>false</c> otherwise.</returns>
        private bool _IsSamePreviousPlane(Vector3 normal)
        {
            if (Mathf.Abs(normal.normalized.x - CurrentNormal.normalized.x) < epsilon &&
                Mathf.Abs(normal.normalized.y - CurrentNormal.normalized.y) < epsilon &&
                Mathf.Abs(normal.normalized.z - CurrentNormal.normalized.z) < epsilon)
            {             
                return true;
            }
            else
            {
                CurrentNormal = normal;
                return false;
            }
        }

        /// <summary>
        /// Reset the scene to start again
        /// </summary>
        public void ResetDetectedPlane()
        {
            HasFloor = false;
            Session.Reset();
            m_NewPlanes.Clear();
            m_RoomPlanes.Clear();
            m_DetectedPlanes.Clear();
            m_IntersectionPoints.Clear();
            ShowDetected.ShowPlanes = true;
            SceneManager.LoadScene("VirtualDesign");
        }

        /// <summary>
        /// Initialize the process to finish creation
        /// </summary>
        public void FinishCreation()
        {
            Vector3 v = new Vector3();

            if(m_RoomPlanes.Count > 4){
                GameObject andyObject;

                // I close the cycle
                if (Plane3D.IntersectPlanes(m_RoomPlanes[0], m_RoomPlanes[1],
                    m_RoomPlanes[m_RoomPlanes.Count - 1], ref v))  
                {
                    m_IntersectionPoints.Add(v);
                    v.y += WallHeight;
                    m_IntersectionPoints.Add(v);
                }

                int WallsNumber = m_IntersectionPoints.Count / 2;

                // Instantiate the walls less last
                for(int i = 0; i < WallsNumber-1; i++)
                {
                    Vector3 MiddlePoint = new Vector3();

                    for (int j = 0; j < 4; j++) // Calculate middle point of the plane
                    {
                        MiddlePoint.x += m_IntersectionPoints[j + (i * 2)].x;
                        MiddlePoint.y += m_IntersectionPoints[j + (i * 2)].y;
                        MiddlePoint.z += m_IntersectionPoints[j + (i * 2)].z;
                    }

                    MiddlePoint /= 4.0f;

                    int IndexPlane = (2 + i) % (WallsNumber + 1);

                    andyObject = Instantiate(DetectedVerticalPlanePrefab, MiddlePoint,
                        m_DetectedPlanes[IndexPlane].CenterPose.rotation, transform);

                    float PlaneSize = Vector3.Distance(m_IntersectionPoints[i * 2], m_IntersectionPoints[(i + 1) * 2]); ;
                    andyObject.transform.localScale += new Vector3((WallHeight * 0.1f) + 0.5f, 0, (PlaneSize * 0.1f) + 0.5f);
                }

                // Instantiate the last wall
                Vector3 MiddleLastPoint = new Vector3();

                for (int i = 0; i < 2; i++) 
                {
                    MiddleLastPoint.x += m_IntersectionPoints[i + (WallsNumber - 1) * 2].x;
                    MiddleLastPoint.y += m_IntersectionPoints[i + (WallsNumber - 1) * 2].y;
                    MiddleLastPoint.z += m_IntersectionPoints[i + (WallsNumber - 1) * 2].z;
                }

                for (int i = 0; i < 2; i++)
                {
                    MiddleLastPoint.x += m_IntersectionPoints[i].x;
                    MiddleLastPoint.y += m_IntersectionPoints[i].y;
                    MiddleLastPoint.z += m_IntersectionPoints[i].z;
                }

                MiddleLastPoint /= 4.0f;

                andyObject = Instantiate(DetectedVerticalPlanePrefab, MiddleLastPoint,
                    m_DetectedPlanes[1].CenterPose.rotation, transform);

                float PlaneSize2 = Vector3.Distance(m_IntersectionPoints[(WallsNumber-1) * 2], m_IntersectionPoints[0]); ;
                andyObject.transform.localScale += new Vector3((WallHeight * 0.1f) + 0.5f, 0, (PlaneSize2 * 0.1f) + 0.5f);

                // Instantiate the roof and floor
                Vector3 RoofPosition = m_DetectedPlanes[0].CenterPose.position;
                RoofPosition.y = WallHeight;

                Instantiate(DetectedHorizontalPlanePrefab, m_DetectedPlanes[0].CenterPose.position,
                            m_DetectedPlanes[0].CenterPose.rotation, transform);
                            
                var RoofObject = Instantiate(DetectedRoofPlanePrefab,RoofPosition, m_DetectedPlanes[0].CenterPose.rotation, transform);
                RoofObject.transform.RotateAround(RoofPosition, new Vector3(1,0,0), 180);

                // Active and desactive buttons
                foreach(GameObject ab in ActiveButton){
                    ab.SetActive(true);
                }

                foreach (GameObject db in DesactiveButton)
                {
                    db.SetActive(false);
                }

                foreach (GameObject dp in PlanesObjects)
                {
                    Destroy(dp);
                }

                // Arcore stops searching planes
                ARCoreAndroidLifecycleManager.Instance.SessionComponent.SessionConfig.PlaneFindingMode =
                    DetectedPlaneFindingMode.Disabled;

                IsCreate = true;
            }
            else {
                if (GlobalLanguage.CurrentLanguage == "Spanish")
                {
                    IndicationText.text = "Aún no se han detectado toda las paredes";
                }
                if (GlobalLanguage.CurrentLanguage == "English")
                {
                    IndicationText.text = "All the walls haven't yet been detected";
                }
                if (GlobalLanguage.CurrentLanguage == "French")
                {
                    IndicationText.text = "Pas encore tous les murs ont été détectés";
                }
            }
        }

    }

}