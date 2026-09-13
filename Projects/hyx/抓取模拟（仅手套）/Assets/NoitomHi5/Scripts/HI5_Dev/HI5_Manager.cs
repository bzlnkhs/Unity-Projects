/**
 * @file       HI5_Manager.cs
 * @brief      HI5 basic functions
 * @details 
 * @author     Qiuhai He
 * @date       30/08/2017
 * @version Hi5_Unity_0_9_8_508_1
 * @par Copyright (c):
 *  Beijing Noitom Technology Ltd.
 * @par History:       
 *  version: Qiuhai He, 18/06/2017, Alpha version\n
 */

using System;
using UnityEngine;
using System.Threading;
using System.Collections.Generic;
namespace HI5
{
    /// <summary>
    /// HI5 bones reference.
    /// </summary>
    public enum Bones
    {
        /// <summary>
        /// The fore arm joint.
        /// </summary>
        ForeArm = 0,
        /// <summary>
        /// The hand joint.
        /// </summary>
        Hand = 1,
        /// <summary>
        /// The metacarpal joint of thumb finger.
        /// </summary>
        HandThumb1,
        /// <summary>
        /// The proximal joint of thumb finger.
        /// </summary>
        HandThumb2,
        /// <summary>
        /// The distal joint of thumb finger.
        /// </summary>
        HandThumb3,
        /// <summary>
        /// The metacarpal joint of index finger.
        /// </summary>
        InHandIndex,
        /// <summary>
        /// The proximal joint of index finger.
        /// </summary>
        HandIndex1,
        /// <summary>
        /// The middle joint of index finger.
        /// </summary>
        HandIndex2,
        /// <summary>
        /// The distal joint of index finger.
        /// </summary>
        HandIndex3,
        /// <summary>
        /// The metacarpal joint of middle finger.
        /// </summary>
        InHandMiddle,
        /// <summary>
        /// The proximal joint of middle finger.
        /// </summary>
        HandMiddle1,
        /// <summary>
        /// The middle joint of middle finger.
        /// </summary>
        HandMiddle2,
        /// <summary>
        /// The distal joint of middle finger.
        /// </summary>
        HandMiddle3,
        /// <summary>
        /// The metacarpal joint of ring finger.
        /// </summary>
        InHandRing,
        /// <summary>
        /// The proximal joint of ring finger.
        /// </summary>
        HandRing1,
        /// <summary>
        /// The middle joint of ring finger.
        /// </summary>
        HandRing2,
        /// <summary>
        /// The distal joint of ring finger.
        /// </summary>
        HandRing3,
        /// <summary>
        /// The metacarpal joint of pinky finger.
        /// </summary>
        InHandPinky,
        /// <summary>
        /// The proximal joint of pinky finger.
        /// </summary>
        HandPinky1,
        /// <summary>
        /// The middle joint of pinky finger.
        /// </summary>
        HandPinky2,
        /// <summary>
        /// The distal joint of pinky finger.
        /// </summary>
        HandPinky3,
        /// <summary>
        /// The number of joints of Hi5 bones.
        /// </summary>
        NumOfHI5Bones,
    }

    /// <summary>
    /// The type of optical tracked device.
    /// </summary>
    public enum OPTDeviceType
    {
        /// <summary>
        /// Unknown type.
        /// </summary>
        Unknown = -1,
        /// <summary>
        /// Type of HTC VIVE tracker.
        /// </summary>
        HTC_VIVE_Tracker,
        /// <summary>
        /// Type of HTC VIVE controller.
        /// </summary>
        HTC_VIVE_Controller,

        Pico_Neo3_Controller,
    }

    /// <summary>
    /// The different level of the glove power.
    /// </summary>
    public enum PowerLevel
    {
        /// <summary>
        /// Unknown power level.
        /// </summary>
        Unknown = -1,
        /// <summary>
        /// Full power level.
        /// </summary>
        Full = 0,
        Good,
        /// <summary>
        /// Normal power level.
        /// </summary>
        Normal,
        /// <summary>
        /// Low power level.
        /// </summary>
        Low,
        None,
    }

    /// <summary>
    /// Magnetic field environment status.
    /// </summary>
    public enum MagneticStatus
    {
        /// <summary>
        /// Unknown status .
        /// </summary>
        Unknown = -1,
        /// <summary>
        /// Status is good.
        /// </summary>
        Good = 0,
        /// <summary>
        /// Status is fair.
        /// </summary>
        Fair,
        /// <summary>
        /// Status is bad.
        /// </summary>
        Bad,
        None,
    }

    /// <summary>
    /// The left or right type of hand.
    /// </summary>
    public enum Hand
    {
        /// <summary>
        /// Left hand.
        /// </summary>
        LEFT = 0,
        /// <summary>
        /// Right hand.
        /// </summary>
        RIGHT,

    }

    /// <summary>
    /// Manage the basic functions of HI5.
    /// </summary>
    public static class HI5_Manager
    {
        //ruige  2019.3.27
        public static bool modifyThreadSave = true;
        //ruige 2019.6.24
        //public static bool IsHasDongle = true;
        /// <summary>
        /// Get whether the Hi5 dongle is connected.
        /// </summary>
        public static bool IsConnected
        { get { return isConnected; } }
        private static bool isConnected = false;

        //public static Action OnDongleStateChanged;

        private static HI5_GloveStatus m_HI5Status = new HI5_GloveStatus();
        private static HI5_Source m_HI5Source = new HI5_Source();
        private static GloveBVHData m_GloveData;

        private static Thread recvThread = null;
        private static bool exitFlag = false;
        public static HI5_MagneticStatus magneticStatus = new HI5_MagneticStatus();
        /// <summary>
        /// Get the left glove position offset related to binded optical device.
        /// </summary>
        public static Vector3 LeftOffset
        {
            get
            {
                SetLeftWristPositionOffset();

                return leftOffset;
            }
        }

        /// <summary>
        /// Get the right glove position offset related to binded optical device.
        /// </summary>
        public static Vector3 RightOffset
        {
            get
            {
                SetRightWristPositionOffset();

                return rightOffset;
            }
        }

        private static Vector3 leftOffset = new Vector3(-0.025f, 0.063f, -0.04f);
        private static Vector3 rightOffset = new Vector3(0.04f, -0.052f, -0.04f);

        public static float[] leftTrackerInitOffset = new float[] { -0.025f, 0.063f, 0.04f };
        public static float[] rightTrackerInitOffset = new float[] { 0.04f, -0.052f, 0.04f };

        // On Glove StateChanged 
        private static bool m_GloveStateCallBackRegistered = false;

        //这个锁没必要 全是主线程调用
        private static object connectLock = new object();

        /// <summary>
        /// Connect the Hi5 device.
        /// </summary>
        public static void Connect()
        {
            /// 问题 调用由HI5_Instance每只手 调用
            lock (connectLock)
            {
                if (isConnected)
                    return;

                // 
                RegisterGloveStateChangedCallback();


                HI5_Device.StartHI5Dongle();


                isConnected = true;

                HI5_Device.SetReducedOutputDataFreq(1);


                //打开接收数据线程 接收数据
                StartDataStreaming();

                //HI5_GloveStatus update
                //打开监听状态线程 监听状态
                RegisterGloveStatusCallback();


                SetDefaultHandOffset();

                //RegisterDongleStateChangedCallback();
                //SerialNumber OpticalSensorType
                SetBindTrackedObjectInfo();


                //没用
                LoadCalibrationData();

                // m_HI5Status.IsCalibrationBposSuccess = true;
                //  m_HI5Status.IsBposComplete = true;
                //ruige 2019 6 24
                // IsHasDongle = true;
            }
        }

        /// <summary>
        /// Disconnect the Hi5 device.
        /// </summary>
        public static void DisConnect()
        {
            if (isConnected)
            {
                StopDataStreaming();

                ReleaseGloveStatusCallBack();

                HI5_Device.StopHI5Dongle();

                // Debug.Log("Hi5 Shutdown!");
                isConnected = false;

                //OnDongleStateChanged -= HandleDongleStateChanged;
            }
        }

        /// <summary>
        /// Get the instance of HI5_GloveStatus class.
        /// </summary>
        /// <returns>
        /// HI5_GloveStatus class instance
        /// </returns>
        public static HI5_GloveStatus GetGloveStatus()
        {
            return m_HI5Status;
        }

        /// <summary>
        /// Get the instance of HI5_Source class.
        /// </summary>
        /// <returns>
        /// HI5_Source class instance
        /// </returns>
        public static HI5_Source GetHI5Source()
        {
            return m_HI5Source;
        }

        /**
         * \cond INTERNAL_USE
         */

        /// <summary>
        /// Set the default hand offset.
        /// </summary>
        /// <param name="leftOffset"></param>
        /// <param name="rightOffset"></param>
        public static void SetDefaultHandOffset()
        {
            //ruige ?
            HI5_Device.SetWristPositionInTrackerFrame(leftTrackerInitOffset, rightTrackerInitOffset);
        }

        public static Vector3 GetLeftWristPositionOffset()
        {
            float[] leftOff = new float[3];
            float[] rightOff = new float[3];
            HI5_Device.GetWristPositionInTrackerFrame(ref leftOff, ref rightOff);
            Vector3 leftOffTrans = new Vector3(leftOff[0], leftOff[1], -leftOff[2]);
            //Vector3 leftOffTrans = new Vector3(leftOff[2], leftOff[0], leftOff[1]);
            //Vector3 leftOffTrans = new Vector3(-leftOff[2], leftOff[0], -leftOff[1]);
            //Vector3 leftOffTrans = new Vector3(-leftOff[2], leftOff[0], leftOff[1]);
            //Vector3 leftOffTrans = new Vector3(leftOff[1], leftOff[0], leftOff[2]);
            //Debug.Log("left " + " x= " + leftOffTrans.x + " y= " + leftOffTrans.y + " z= " + leftOffTrans.z);
            return leftOffTrans;
        }

        public static Vector3 GetRightWristPositionOffset()
        {
            float[] leftOff = new float[3];
            float[] rightOff = new float[3];
            HI5_Device.GetWristPositionInTrackerFrame(ref leftOff, ref rightOff);
            Vector3 rightOffTrans = new Vector3(rightOff[0], rightOff[1], -rightOff[2]);
            //Vector3 rightOffTrans = new Vector3(rightOff[2], rightOff[0], rightOff[1]);
            //Vector3 rightOffTrans = new Vector3(-rightOff[2], rightOff[0], -rightOff[1]);
            //Vector3 rightOffTrans = new Vector3(-rightOff[2], rightOff[0], rightOff[1]);
            //Vector3 rightOffTrans = new Vector3(rightOff[1], rightOff[0], rightOff[2]);
            //Debug.Log("right " + " x= " + rightOffTrans.x + " y= " + rightOffTrans.y + " z= " + rightOffTrans.z);
            return rightOffTrans;
        }

        public static void SetLeftWristPositionOffset()
        {
            Vector3 newOff = GetLeftWristPositionOffset();
            if (newOff != Vector3.zero)
                leftOffset = newOff;
        }

        public static void SetRightWristPositionOffset()
        {
            Vector3 newOff = GetRightWristPositionOffset();
            if (newOff != Vector3.zero)
                rightOffset = newOff;
        }

        /// <summary>
        /// Set the serial number of binded tracked object.
        /// </summary>
        /// <returns>
        /// True, success; False, fail.
        /// </returns>
        public static bool SetBindTrackedObjectInfo()
        {

            if (LoadBindTrackedObjectsInfo())
            {
                OpticalSensorType leftSensorType = DeviceTypeToSensorType(HI5_BindInfoManager.BindInfo.Left.DeviceType);
                OpticalSensorType rightSensorType = DeviceTypeToSensorType(HI5_BindInfoManager.BindInfo.Right.DeviceType);
                HI5_Device.SetOptiSensorBindState(GloveMod.GM_LeftGlove, HI5_BindInfoManager.BindInfo.Left.SerialNumber, leftSensorType);
                HI5_Device.SetOptiSensorBindState(GloveMod.GM_RightGlove, HI5_BindInfoManager.BindInfo.Right.SerialNumber, rightSensorType);
                return true;
            }
            else
            {
                HI5_Device.SetOptiSensorBindState(GloveMod.GM_LeftGlove, "Pico Neo3 Controller left",
                    OpticalSensorType.OST_HTC_VIVE_Tracker);
                HI5_Device.SetOptiSensorBindState(GloveMod.GM_RightGlove, "Pico Neo3 Controller right",
                    OpticalSensorType.OST_HTC_VIVE_Tracker);
                return true;
            }
        }

        /*
        /// <summary>
        /// Enable/Disable the finger rotation on YAW direction.
        /// </summary>
        /// <param name="value">
        /// </param>
        public static void EnableFingerAdbFixed(bool value)
        {
            HI5_Device.EnableFingerAdbFixed(value);
        }
        */

        /// <summary>
        /// Bind HI5 bones data to each transform of bones on hand.
        /// </summary>
        /// <param name="root">
        /// The root bone of the rigged hand model. Input by <see cref="UnityEngine.Transform"/>.
        /// </param>
        /// <param name="bones">
        /// All the bones of the rigged hand model. The sort of the array is referenced to <see cref="HI5.Bones"/>.
        /// </param>
        /// <param name="prefix">
        /// The prefix of each bones. Input by <see cref="System.String"/>.
        /// </param>
        /// <param name="handType">
        /// The type of <see cref="HI5.Hand"/>.
        /// </param>
        /// <returns>
        /// Number of successfully binded bones. 
        /// </returns>
        public static void BindBones(Transform root, Transform[] bones, string prefix, Hand handType)
        {
            //HI5_Helper.Bind(root, bones, prefix, handType);
            HI5_Bones.Bind(root, bones, prefix, handType);
        }

        /**
         * \endcond
         */

        /// <summary>
        /// Check whether the HI5 dongle is available.
        /// </summary>
        /// <returns>
        /// True, the HI5 dongle is available. 
        /// False, the HI5 dongle is inavailable.
        /// </returns>
        public static bool IsDongleAvailable()
        {
            return HI5_Device.IsDongleAvailable();
        }

        /// <summary>
        /// Control the vibration on left glove.
        /// </summary>
        /// <param name="time">
        /// Input the vibration time by milliseconds.
        /// </param>
        public static void EnableLeftVibration(int time)
        {
            HI5_Device.EnableLeftGloveVibration(time);
        }

        /// <summary>
        /// Control the vibration on right glove.
        /// </summary>
        /// <param name="time">
        /// Input the vibration time by milliseconds.
        /// </param>
        public static void EnableRightVibration(int time)
        {
            HI5_Device.EnableRightGloveVibration(time);
        }

        /// <summary>
        /// Control the vibration on both gloves.
        /// </summary>
        /// <param name="leftTime">
        /// Input the vibration time by milliseconds on left glove.
        /// </param>
        /// <param name="rightTime">
        /// Input the vibration time by milliseconds on right glove.
        /// </param>
        public static void EnableBothGlovesVibration(int leftTime, int rightTime)
        {
            HI5_Device.EnableGlovesVibration(leftTime, rightTime);
        }

        // Test for Openning 

        private static OpticalSensorType DeviceTypeToSensorType(OPTDeviceType type)
        {
            if (type == OPTDeviceType.HTC_VIVE_Controller)
                return OpticalSensorType.OST_HTC_VIVE_Controller;
            if (type == OPTDeviceType.HTC_VIVE_Tracker)
                return OpticalSensorType.OST_HTC_VIVE_Tracker;
            return OpticalSensorType.OST_Unknown;
        }

        private static void StartDataStreaming()
        {
            //Debug.Log("StartDataStreaming");
            if (recvThread != null)
            {
                recvThread.Abort();
                recvThread = null;
            }
            recvThread = new Thread(new ThreadStart(DoReceive));
            exitFlag = false;
            recvThread.Start();
        }

        private static void StopDataStreaming()
        {
            //Debug.Log("StopDataStreaming");
            exitFlag = true;
            if (recvThread != null)
            {
                recvThread.Abort();
                recvThread = null;
            }
        }
        public static void Update()
        {
            //ruige 2019 6 25
            magneticStatus.Update(Time.deltaTime);
            if (modifyThreadSave)
            {
                

            }
        }
        private static void DoReceive()
        {
            //Debug.Log("Start Receive HI5 BVH Thread");
            while (!exitFlag)
            {
                HI5_Device.ReadBVHData(ref m_GloveData);
                if (m_GloveData.bUpdate == 1)
                {
                    //if (modifyThreadSave)
                    //{
                    //    Hi5_BVHDataQuene.WriteBvhData(m_GloveData);
                    //}
                    //else
                    //{
                    //    m_HI5Source.UpdateTrackingData(m_GloveData);
                    //}
                }
                Thread.Sleep(1);
            }
        }

        private static void RegisterDongleStateChangedCallback()
        {
            if (!m_GloveStateCallBackRegistered)
            {
                HI5_Device.setDongleStateChangedHandle(IntPtr.Zero, OnDongleStateChanged);
                m_GloveStateCallBackRegistered = true;
            }
        }

        private static void RegisterGloveStatusCallback()
        {
            /// m_HI5Status.RegisterGloveStatusChangedCallBack();
        }

        private static void ReleaseGloveStatusCallBack()
        {
            m_HI5Status.ReleaseGloveStatusChangedCallBack();
        }

        private static void OnDongleStateChanged(IntPtr customObject, IntPtr hid, DongleEvent action)
        {
            //ruige 2019 6 24
            if (action == DongleEvent.DE_Insert)
            {
                //IsHasDongle = true;
            }
            else if (action == DongleEvent.DE_Removed)
            {
                // IsHasDongle = false;
                magneticStatus.RemoveDongle();
            }
        }

        private static void RegisterGloveStateChangedCallback()
        {
            HI5_Device.setGloveStateChangedHandle(IntPtr.Zero, OnGloveStateChanged);
        }

        //private static void RegisterBPoseCalibrationResult()
        //{
        //    HI5_Device.setBPoseCalibrationResultHandle(IntPtr.Zero, BPoseCalibrationResultCallback);

        //}

        //private static void BPoseCalibrationResultCallback(IntPtr cutomObject, BPoseCalibrationErrors result)
        //{
        //    m_HI5Status.OnCallBackBPosResult(result);
        //}

        private static void OnGloveStateChanged(IntPtr cutomObject, GloveMod gloveMod, GloveEvent gloveEvent)
        {
            //ruige 2019 6 25
            magneticStatus.OnEventChanged(gloveMod, gloveEvent);

            m_HI5Status.OnEventChanged(gloveMod, gloveEvent);
        }

        private static bool LoadCalibrationData()
        {
            return HI5_Calibration.LoadCalibrationData();
        }
        private static bool LoadBindTrackedObjectsInfo()
        {
            return HI5_BindInfoManager.LoadItems(false);
        }

    }
}