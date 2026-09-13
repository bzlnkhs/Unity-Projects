using UnityEngine;
using System;

namespace HI5.VRCalibration
{
    public enum CalibrationState
    {
        Exit = -1,
        //Warning = 0,
        //APose,
        //BPose,
        //PPose,
        //VPose,
        //Finish,

        Warning = 0,
        VPose,
        BPose,
        PPose,
        Finish,
    }

    public class CalibrationStateMachine : MonoBehaviour
    {
        [SerializeField] private MenuStateMachine m_MenuSM;
        [SerializeField] private GameObject[] m_StateItems;

        // Use this action on your own project to call the specific function after enter a state.
        public Action<CalibrationState> OnStateEnter;

        public CalibrationState State
        {
            get { return currentState; }
            set
            {
                StateExit(currentState);
                currentState = value;
                StateEnter(currentState);
            }
        }
        public HI5GloveState m_HI5GloveState;
        private CalibrationState currentState;

        private void OnEnable()
        {
            m_MenuSM.OnStateEnter += HandleMenuStateEnter;

            OnStateEnter += HandleCalibrationStateEnter;

            if (!IsEnvironmentGood())
                currentState = CalibrationState.Warning;
            else
                currentState = CalibrationState.VPose;

            this.State = currentState;

            //HI5_Calibration.ResetCalibration();
        }

        private void OnDisable()
        {
            m_MenuSM.OnStateEnter -= HandleMenuStateEnter;
            OnStateEnter -= HandleCalibrationStateEnter;
        }

        private void StateExit(CalibrationState stateExit)
        {
        }

        private void StateEnter(CalibrationState stateEnter)
        {
            if (OnStateEnter != null)
                OnStateEnter(stateEnter);
            ActiveItem((int)stateEnter);
        }

        private void ActiveItem(int index)
        {
            for (int i = 0; i < m_StateItems.Length; i++)
            {
                if (m_StateItems[i] != null)
                    m_StateItems[i].SetActive(false);
            }

            if (index > m_StateItems.Length && index < 0)
                return;

            if (index > -1 && index < m_StateItems.Length)
                m_StateItems[index].SetActive(true);
        }

        private void HandleMenuStateEnter(MenuState state)
        {
            if (state == MenuState.Calibration)
                currentState = CalibrationState.VPose;

            if (state != MenuState.Calibration)
                currentState = CalibrationState.Exit;
        }

        private void HandleCalibrationStateEnter(CalibrationState state)
        {
//             // Restart and Load the Calibration data 
//             if (state == CalibrationState.VPose)
//             {
//                 HI5_Calibration.ResetCalibration();
//             }
// 
//             if (state == CalibrationState.Exit)
//             {
//             }
        }

        private bool IsEnvironmentGood()
        {
            if (HI5_Manager_Thread.Instance() != null)
            {
                if(m_HI5GloveState != null)
                {
                    return !m_HI5GloveState.IsOpenWarning();
                }
             
            }
            return false;
        }
    }


}
