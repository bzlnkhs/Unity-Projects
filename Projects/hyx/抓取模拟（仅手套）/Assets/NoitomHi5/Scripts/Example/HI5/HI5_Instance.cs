//======= Copyright (c) Beijing Noitom Technology Ltd., All rights reserved. ===============
//
// Purpose: Connect and disconnect the Hi5 devices.
//
//==========================================================================================

using UnityEngine;

namespace HI5
{
    public class HI5_Instance : MonoBehaviour
    {
        public Hand HandType;

        [Header("Bone Transform Settings")]

        [SerializeField]
        protected Transform m_Root = null;

        [SerializeField]
        protected string m_Prefix = "Human_";

        public Transform[] HandBones = new Transform[(int)Bones.NumOfHI5Bones];

        protected HI5_Source m_BindSource = null;

        protected void Start()
        {
            Connect();
        }

        protected void OnEnable()
        {
            Connect();
        }

        protected void OnDisable()
        {
        }

        protected void OnApplicationQuit()
        {
            Disconnect();
        }

        protected void Update()
        {
            if(HI5_Manager_Thread.Instance() != null)
            {
                SetBindSource(HI5_Manager_Thread.Instance().m_HI5Source);
            }
        }
        protected void Connect()
        {
            //if (HI5_Manager_Thread.Instance() != null)
            //{
            //    if (!HI5_Manager_Thread.Instance().IsConected)
            //    {
            //        //AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            //        //AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            //        //AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");
            //        //context.GetRawObject();
            //        //HI5_Manager.Connect();
            //        Debug.Log("Connect");
            //        HI5_Manager_Thread.Instance().RequestConnect();
            //        /// 
            //        //RegisterGloveStateChangedCallback();
            //        //HI5_Device.StartHI5Dongle();
            //        //HI5_Manager_Thread.Instance().IsConected = true;
            //    }
            //    m_BindSource = HI5_Manager_Thread.Instance().GetHI5Source();
            //}
           
        }

        protected void Disconnect()
        {
            //if (HI5_Manager_Thread.Instance() != null)
            //{
            //    if (HI5_Manager_Thread.Instance().IsConected)
            //    {

            //        HI5_Manager_Thread.Instance().RequestCloseConnect();
            //    }
            //    else { return; }
            //}
            
        }

        // Used by HI5_InstanceEditor.
        public void AutoBindBones(Hand type)
        {
            HI5_Manager_Thread.BindBones(m_Root, HandBones, m_Prefix, type);
        }

        // Returns the saved transform references in the HandBones list.
        public Transform GetBoneTransform(Bones bone)
        {
            return HandBones[(int)bone];
        }

        public void SetBindSource(HI5_Source source)
        {
            if(m_BindSource == null)
                m_BindSource = source;
        }
    }
}