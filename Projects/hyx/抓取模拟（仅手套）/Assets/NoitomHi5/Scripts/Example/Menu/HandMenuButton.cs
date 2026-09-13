using UnityEngine;
using HI5.VRInteraction;

namespace HI5.VRCalibration
{
    public class HandMenuButton : VRButton
    {
        [SerializeField]
        private MenuStateMachine m_MenuSM;

        new void OnEnable()
        {
            base.OnEnable();
            if (m_SelectionRadial != null)
                m_SelectionRadial.OnSelectionComplete += HandleSelectionComplete;
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
                if (m_MenuSM.State == MenuState.Exit && HI5_Manager_Thread.Instance() != null)
                {
                    if (HI5_Manager_Thread.Instance().GetGloveStatus() == null )
                        m_MenuSM.State = MenuState.Main;
                    else
                    {
                        /*if (HI5_Manager.GetGloveStatus().IsLeftGloveAvailable)
                            Debug.Log("IsLeftGloveAvailable");
                        if (HI5_Manager.GetGloveStatus().IsRightGloveAvailable)
                            Debug.Log("IsRightGloveAvailable");
                        if (HI5_BindInfoManager.IsRightGloveBinded)
                            Debug.Log("IsRightGloveBinded");
                        if (HI5_BindInfoManager.IsLeftGloveBinded)
                            Debug.Log("IsLeftGloveBinded");*/
                        if (HI5_Manager_Thread.Instance().GetGloveStatus().IsLeftGloveAvailable 
                            && HI5_Manager_Thread.Instance().GetGloveStatus().IsRightGloveAvailable
                            && HI5_BindInfoManager.IsLeftGloveBinded && HI5_BindInfoManager.IsRightGloveBinded)
                        {
                            m_MenuSM.State = MenuState.Main;
                        }
                    }
                }
                else
                    m_MenuSM.State = MenuState.Exit;                
            }
        }
        internal protected override void ClickButton()
        {
           
            HandleSelectionComplete();
            base.ClickButton();
        }
    }

}
