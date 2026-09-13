using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace HI5
{
    public class Hi5_Thread_MonoBehaviour : MonoBehaviour
    {
        public bool isInertia = false;
        public bool isEnableFingerFixed = true;   
        public HI5_InertiaInstance mLeft = null;
        public HI5_InertiaInstance mRight = null;
        public GameObject mHandLeft = null;
        [NonSerialized]
        public bool isResetRoot = false;
        bool isCarlibration;
        protected bool IsVisibleHandMenu;
        public  bool VisibleHandMenu
        {
            get { return IsVisibleHandMenu; }
            set 
            { 
                IsVisibleHandMenu = value;
                if(mHandLeft != null)
                {
                    if (IsVisibleHandMenu && isCarlibration)
                        mHandLeft.SetActive(true);
                    else
                        mHandLeft.SetActive(false);
                }
            }
        }
        private void Awake()
        {
            DontDestroyOnLoad(this);
            isCarlibration = false;
            VisibleHandMenu = false;
            //HI5_Manager_Thread.ConnectEquipment();
            HI5_Manager_Thread.Instance().isInertia = isInertia;
            HI5_Manager_Thread.Instance().mHi5ThreadMonoBehaviour = this;
            StartHi5();
        }


        // Start is called before the first frame update
        void Start()
        {
            isResetRoot = false;
        }
        // Update is called once per frame
        void Update()
        {
            if (HI5_Manager_Thread.Instance() != null)
            {
                HI5_Manager_Thread.Instance().isInertia = isInertia;
                HI5_Manager_Thread.Instance().Update();
            }
            if(isResetRoot)
            {
                if (mLeft != null)
                {
                    if (HI5_Manager_Thread.Instance().isInertia)
                        mLeft.transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                }
                if (mRight != null)
                {
                    if (HI5_Manager_Thread.Instance().isInertia)
                        mRight.transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                }
                VisibleHandMenu = true;
                isResetRoot = false;
            }
        }

        protected void OnDisable()
        {
            
        }
        public void ChangeFingerFixState()
        {
            isEnableFingerFixed = !isEnableFingerFixed;
            HI5_Device.EnableFingerAdbFixed(isEnableFingerFixed);
        }
        void OnApplicationQuit()
        {
            //if (HI5_Manager_Thread.Instance() != null)
            {
                //Debug.Log("OnApplicationQuit1");
                StopHi5();
                 // HI5_Manager_Thread.CloseConnectEquipment();
            }
        }

        public void ResetHandRoot()
        {
            isResetRoot = true;
            isCarlibration = true;
           
        }

        public void StartHi5()
        {
            HI5_Manager_Thread.Instance().enableFingerAdbFixed = isEnableFingerFixed;
            HI5_Manager_Thread.Instance().RequestConnect(isInertia);
            
        }

        public void StopHi5()
        {
            HI5_Manager_Thread.Instance().RequestCloseConnect();
            HI5_Manager_Thread.Clean();
        }
    }
}
