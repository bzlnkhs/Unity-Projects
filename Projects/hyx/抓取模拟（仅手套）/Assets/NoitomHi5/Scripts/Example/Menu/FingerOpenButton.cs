using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HI5
{
    public class FingerOpenButton : MonoBehaviour
    {
        public Hi5_Thread_MonoBehaviour fingerSet = null;
        public GameObject On = null;
        public GameObject Off = null;
        // Start is called before the first frame update
        void Start()
        {

        }
        private void OnEnable()
        {
            FreshVisible();
        }
        public void ClickButton()
        {
            if (fingerSet != null)
            {
                fingerSet.ChangeFingerFixState();
            }
            FreshVisible();
        }
        private void FreshVisible()
        {
            if (fingerSet != null)
            {
                if (!fingerSet.isEnableFingerFixed)
                {
                    On.SetActive(true);
                    Off.SetActive(false);
                }
                else
                {
                    On.SetActive(false);
                    Off.SetActive(true);
                }
            }
        }
        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
