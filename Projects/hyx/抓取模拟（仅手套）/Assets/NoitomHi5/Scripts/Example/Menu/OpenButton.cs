using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HI5.VRInteraction;

namespace HI5.VRCalibration
{
    public class OpenButton : MenuStateButton
    {
        private void Update()
        {
            if (m_MenuSM.State != MenuState.Exit)
                gameObject.SetActive(false);
        }
    }
}