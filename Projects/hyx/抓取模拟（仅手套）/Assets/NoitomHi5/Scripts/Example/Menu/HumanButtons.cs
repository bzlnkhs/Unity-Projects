using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HI5;
using HI5.VRCalibration;
    public class HumanButtons : MonoBehaviour
    {
        public SpriteRenderer renderHelp;
        public Sprite RumblerMenuSprite1;
        public Sprite RumblerMenuSprite2;
    //public SpriteRenderer renderMagnetic;
    //public SpriteRenderer renderBattery;
       public GameObject colliderHelp;
        private bool m_isVisible = false;
    
        HI5.VRCalibration.MenuState mState;

        private void setVisible(bool isVisible)
        {
            m_isVisible = isVisible;
        }

        public void IsVisible(bool isVisible)
        {
            setVisible(isVisible);
            if (isVisible)
            {
                renderHelp.sprite = RumblerMenuSprite2;
            }
            else
            {
                renderHelp.sprite = RumblerMenuSprite1;
            }
            //if (renderHelp != null)
            //{
            //    renderHelp.enabled = isVisible;
            //    BoxCollider colliderBox = renderHelp.GetComponent<BoxCollider>();
            //    if (colliderBox != null)
            //    {
            //        colliderBox.enabled = isVisible;
            //    }
            //}

            //if (colliderHelp != null)
            //{
            //    PhysicalHandButton button = colliderHelp.GetComponent<PhysicalHandButton>();
            //    if (button != null)
            //    {
            //        button.enabled = isVisible;
            //    }
            //    BoxCollider colliderBox = colliderHelp.GetComponent<BoxCollider>();
            //    if (colliderBox != null)
            //    {
            //        colliderBox.enabled = isVisible;
            //    }
            //}
        }

        public void Update()
        {
            if (MenuStateMachine.GetInstanceMenuStateMachine() != null)
            {
                //if(MenuStateMachine.GetInstanceMenuStateMachine().currentState == MenuState.Exit)
                //{
                //        IsVisible(true);
                //}
                
            }
            else
            {
                //IsVisible(m_isVisible);
            }
        }
    private IEnumerator C_DelayVisible()
    {
        yield return new WaitForSeconds(5f);
        if (MenuStateMachine.GetInstanceMenuStateMachine().currentState == MenuState.Exit)
            IsVisible(true);
        else
            IsVisible(false);
       
    }
    private void OnEnable()
        {
           // Hi5_Message.GetInstance().RegisterMessage (ReceiveMessageFun, Hi5_Message.Hi5_MessageMessageKey.messageStateChange);
        }

        private void OnDisable()
        {
           // Hi5_Message.GetInstance().UnRegisterMessage(ReceiveMessageFun, Hi5_Message.Hi5_MessageMessageKey.messageStateChange);
        }

        //void ReceiveMessageFun(string messageKey, object param1, object param2)
        //{
        //    if (messageKey.CompareTo(Hi5_Message.Hi5_MessageMessageKey.messageStateChange) == 0)
        //    {
        //        mState = (HI5.VRCalibration.MenuState)(param1);
        //    }
        //}
}

