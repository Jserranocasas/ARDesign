namespace AppDesign{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine.UI;
    using UnityEngine;

    public class DragAndDrop : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        public float FixedSpeed = 10, SpeedIncrease = 100;

        /// <summary>
        /// 
        /// </summary>
        private const float epsilon = 0.0000001f;

        /// <summary>
        /// 
        /// </summary>
        private float PreviusY;

        /// <summary>
        /// 
        /// </summary>
        private float Displacement;

        /// <summary>
        /// 
        /// </summary>
        public float HighLimit, LowLimit;

        /// <summary>
        /// 
        /// </summary>    
        private bool IsSelectedPanel;

        /// <summary>
        /// 
        /// </summary>
        private bool ExistDisplacement;

        /// <summary>
        /// The elapsed time that panel is being pressed.
        /// </summary>
        private float PressedPanelElapsed;
        
        void Awake() {
            ExistDisplacement = false;
            PressedPanelElapsed = 0.0f;
            Displacement = 0f;
        }

        // Update is called once per frame
        public void Update()
        {
            if (ModeAction.Instance.getMode() == ModeStatus.Selection)
            {
                float MovementY = 0f;

                if (IsSelectedPanel == true) {

                    PressedPanelElapsed += Time.deltaTime;

                    if(PressedPanelElapsed > 0.1){
                        MovementY = PreviusY - Input.mousePosition.y;

                        if (Mathf.Abs(MovementY) > epsilon) {
                            float range = (Mathf.Abs(MovementY) > 90) ? 90 : Mathf.Abs(MovementY);

                            // Get inverse number in range [0, 90] to cosine
                            // So you get a speed with smooth curve
                            range = Mathf.Abs((range / 100.0f) - 1) * 90;
                            float MovementSpeed = FixedSpeed + Mathf.Abs(
                                Mathf.Pow(Mathf.Cos(range), 3)) * SpeedIncrease;

                            if (MovementY > 0 && transform.position.y > LowLimit)
                            {
                                PerformMovement(MovementSpeed, false);
                                Displacement -= MovementSpeed;
                                
                                // Menu has been moved
                                ExistDisplacement = true;
                            }
                            else if (MovementY < 0 && transform.position.y < HighLimit)
                            {
                                PerformMovement(MovementSpeed, true);
                                Displacement +=  MovementSpeed;

                                // Menu has been moved
                                ExistDisplacement = true;
                            }

                        }
                    }
                }

                if (Input.GetMouseButtonUp(0))
                {
                    PressedPanelElapsed = 0f;
                    IsSelectedPanel = false;
                }

            }
            
            PreviusY = Input.mousePosition.y;
        }

        /// <summary>
        /// Move the panel vertically
        /// <param name="MovementSpeed">Distance to move</param>
        /// <param name="MovementPositive">To know if to move up or down</param>
        /// </summary>
        public void PerformMovement(float MovementSpeed, bool MovementPositive)
        {
            int i=0;

            while(MovementSpeed > i)
            {
                if(MovementPositive)
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y + 1);
                }
                else
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y - 1);
                }

                i++;
            }
        }

        /// <summary>
        /// Return the panel to original position
        /// </summary>
        public void ComeBackInitialPosition()
        {
            if (ExistDisplacement)
            {
                transform.position = new Vector2(transform.position.x,
                                transform.position.y - Displacement);

                ExistDisplacement = false;
                Displacement = 0f;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnGUI()
        {
            if (Input.GetMouseButtonDown(0))
            {
                IsSelectedPanel = true;
            }
        }
    }
}