using UnityEngine;
using HI5.VRInteraction;

namespace HI5.VRCalibration
{
    public class StartButton : VRButton
    {
        [SerializeField] private CountDownUI m_CountDownUI;
        [SerializeField] private CalibrationStateMachine m_CalibrationSM;

        public SwitchCalibrationButtonState _HomeButton = null;
        new void OnEnable()
        {
            base.OnEnable();
            if (m_SelectionRadial != null)
                m_SelectionRadial.OnSelectionComplete += HandleSelectionComplete;

            EnableButton(true);

            if (m_CalibrationInstance!=null&&!m_CalibrationInstance.GetIsVPose())//不是V Pose
            {
                AutoCali();
            }
        }

        new void OnDisable()
        {
            base.OnDisable();
            if (m_SelectionRadial != null)
                m_SelectionRadial.OnSelectionComplete -= HandleSelectionComplete;
        }

        private void HandleSelectionComplete()
        {
            if (m_GazeOver)
            {
                if (m_CountDownUI != null)
                    m_CountDownUI.StartCD();
               // m_CalibrationInstance.
                //HI5_Calibration.ResetCalibration();
                EnableButton(false);
            }
        }

        private void EnableButton(bool value)
        {
            GetComponent<Collider>().enabled = value;
        }
        internal protected override void ClickButton()
        {
            
            if (GetComponent<Collider>().enabled)
            {
                HandleSelectionComplete();
            }
            base.ClickButton();
        }

        #region Auto Cali
        private CalibrationInstance m_CalibrationInstance;
        private void Awake()
        {
            m_CalibrationInstance = GetComponent<CalibrationInstance>();
        }
        private void AutoCali()
        {
            if (m_CountDownUI != null)
                m_CountDownUI.StartCD();

            //HI5_Calibration.ResetCalibration();
            EnableButton(false);
        }
        //private IEnumerator ResetHome()
        //{
        //    yield return new WaitForSeconds(0.5f);
        //}
        #endregion
    }
}