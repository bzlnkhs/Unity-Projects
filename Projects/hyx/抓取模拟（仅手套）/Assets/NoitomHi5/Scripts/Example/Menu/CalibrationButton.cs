using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using HI5.VRInteraction;
using UnityEngine.Serialization;
namespace HI5.VRCalibration
{
    public class CalibrationButton : VRButton
    {
        [SerializeField] private CalibrationState m_EnterState;
        [SerializeField] private CalibrationStateMachine m_CalibrationSM;
        [Serializable]
        public class ButtonClickedEvent : UnityEvent { }

        // Event delegates triggered on click.
        [FormerlySerializedAs("CalibrationButton")]
        [SerializeField]
        private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();
        public ButtonClickedEvent onClick
        {
            get { return m_OnClick; }
            set { m_OnClick = value; }
        }
        new void OnEnable()
        {
            base.OnEnable();
            if (m_SelectionRadial != null)
                m_SelectionRadial.OnSelectionComplete += HandleSelectionComplete;

            EnableButton(true);
        }

        new void OnDisable()
        {
            //if (GetComponent<VRInteractiveItem>() != null)
            //    GetComponent<VRInteractiveItem>().Out();
            base.OnDisable();
            if (m_SelectionRadial != null)
                m_SelectionRadial.OnSelectionComplete -= HandleSelectionComplete;
        }

        private void HandleSelectionComplete()
        {
            if (m_GazeOver)
            {
                m_CalibrationSM.State = m_EnterState;
            }
            UISystemProfilerApi.AddMarker("CalibrationButton.onClick", this);
            m_OnClick.Invoke();
        }

        private void EnableButton(bool value)
        {
            GetComponent<Renderer>().enabled = value;
            GetComponent<Collider>().enabled = value;
        }

        internal protected override void ClickButton()
        {
            HandleSelectionComplete();
            base.ClickButton();
            
        }
    }

}
