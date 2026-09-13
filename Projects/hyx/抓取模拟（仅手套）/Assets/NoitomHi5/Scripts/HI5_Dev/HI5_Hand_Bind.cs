using UnityEngine;

namespace HI5
{
    public class HI5_Hand_Bind : MonoBehaviour
    {
        bool isInitSendBind = false;
        bool isSetThickness = false;
        public float Thickness = 0.02f;

        public static HI5_Hand_Bind GetInstance()
        {
            return _instance;
        }
        static HI5_Hand_Bind _instance = null;

        private void Awake()
        {
            _instance = this;
            // Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
        void Update()
        {
            if (!isInitSendBind && HI5_Manager_Thread.Instance() != null && HI5_Manager_Thread.Instance().IsConected)
            {
                HI5_Manager_Thread.Instance().SetBindTrackedObjectInfo(false);
                HI5_Manager_Thread.Instance().LoadCalibrationData();
                isInitSendBind = true;

            }

            if (HI5_Manager_Thread.Instance() != null && HI5_Manager_Thread.Instance().IsConected && HI5_Manager_Thread.Instance().IsStartDongle)
            {
                if (isSetThickness)
                {
                    HI5_Manager_Thread.Instance().SetHandThickness(Thickness);
                    isSetThickness = false;
                }
            }
        }

        internal void BindTrackedObjectInfo()
        {
            if (HI5_Manager_Thread.Instance() != null)
            {
                HI5_Manager_Thread.Instance().SetBindTrackedObjectInfo(false);
            }
        }

        void OnApplicationFocus(bool isFocus)
        {
            if (isFocus)
            {

            }
            else
            {
                // Debug.Log("离开游戏 激活推送");  //  返回游戏的时候触发     执行顺序 1
            }
        }


        void OnApplicationPause(bool isPause)
        {
            //if (isPause)
            //{
            //    Debug.Log("Game stop");  // 缩到桌面的时候触发
            //    if (HI5_Manager_Thread.Instance() != null)
            //    {
            //        HI5_Manager_Thread.Instance().GamePause(isPause);
            //    }
            //}
            //else
            //{
            //    if (HI5_Manager_Thread.Instance() != null)
            //    {
            //        HI5_Manager_Thread.Instance().GamePause(isPause);
            //    }
            //    Debug.Log("Game start");  //回到游戏的时候触发 最晚
            //}
        }
    }
}
