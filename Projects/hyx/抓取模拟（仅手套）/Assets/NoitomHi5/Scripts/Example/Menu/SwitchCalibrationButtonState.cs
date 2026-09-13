using UnityEngine;
using HI5.VRInteraction;

namespace HI5.VRCalibration
{
    public class SwitchCalibrationButtonState : MonoBehaviour
    {
        [SerializeField] private VRButton m_Button;
        [SerializeField] private CountDownUI m_CountDown;
        [SerializeField] private Sprite[] m_Styles;
        [SerializeField] private bool IsEnableUse = true;
        private void OnEnable()
        {
            m_CountDown.OnCountDwonStart += HandleCountDownStart;
            if(IsEnableUse)
                EnableButton(true);
            else
                EnableButton(false);
        }

        private void OnDisable()
        {
            m_CountDown.OnCountDwonStart -= HandleCountDownStart;
        }

        private void HandleCountDownStart()
        {
            EnableButton(false);
        }

        public void EnableButton(bool value)
        {
            //ruige red
            //ActiveSelectionRadial(false);
            ActiveSelectionRadial(value);

            GetComponent<Collider>().enabled = value;

            int index = value == false ? 1 : 0;
            SwitchStyle(index);
        }

        private void SwitchStyle(int index)
        {
            if (index > m_Styles.Length - 1)
                return;

            if (m_Styles[index] != null)
                GetComponent<SpriteRenderer>().sprite = m_Styles[index];
        }

        private void ActiveSelectionRadial(bool value)
        {
            if(m_Button.SelectionRadial != null)
            {
                if (value)
                    m_Button.SelectionRadial.Show();
                else
                    m_Button.SelectionRadial.Hide();
            }
            
        }
    }

}
