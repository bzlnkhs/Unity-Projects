using UnityEngine;
using HI5.VRInteraction;
using UnityEngine.SceneManagement;
namespace HI5.VRCalibration
{
    public class MenuStateButton : VRButton
    {
        public MenuState EnterState;
        [SerializeField] protected MenuStateMachine m_MenuSM;
        new void OnEnable()
        {
            base.OnEnable();
            if(m_SelectionRadial != null)
                m_SelectionRadial.OnSelectionComplete += HandleSelectionComplete;
        }

        new void OnDisable()
        {
            base.OnDisable();
            //ruige red
            // m_SelectionRadial.Hide();
            if (m_SelectionRadial != null)
                m_SelectionRadial.OnSelectionComplete -= HandleSelectionComplete;
        }

        private void HandleSelectionComplete()
        {
            if (m_GazeOver)
            {
                //if(EnterState == MenuState.Game)
                //{
                //   // SceneManager.LoadScene("TableScene_2");
                //}
                //else
                    m_MenuSM.State = EnterState;
            }
        }
        internal protected override void ClickButton()
        {
            HandleSelectionComplete();
            base.ClickButton();
        }
    }
}
