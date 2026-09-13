using UnityEngine;
using System;
using System.IO;

namespace HI5
{
    /// <summary>
    /// HI5 calibration pose.
    /// </summary>
    public enum HI5_Pose
    {
        /// <summary>
        /// Unknown pose.
        /// </summary>
        Unknown = -1,
        /// <summary>
        /// Buddha Pose
        /// </summary>
        BPose = 0,
        /// <summary>
        /// Pinch Pose.
        /// </summary>
        PPose,

        //APose,

        //TPose,
        VPose = 4,
    }

    /// <summary>
    /// HI5 Calibration Class.
    /// </summary>
    public static class HI5_Calibration
    {
        /// <summary>
        /// Call it when B-pose or P-pose calibration complete.
        /// </summary>
        public static Action<HI5_Pose> OnCalibrationComplete;

        /// <summary>
        /// Is doing calibration B-pose or not.
        /// </summary>
        public static bool IsCalibratingBPose
        {
            get { return isCalibratingBPose; }
        }
        private static bool isCalibratingBPose = false;

        /// <summary>
        /// Is doing calibration P-pose or not.
        /// </summary>
        public static bool IsCalibratingPPose
        {
            get { return isCalibratingPPose; }
        }
        private static bool isCalibratingPPose = false;

        /// <summary>
        /// Get the default path of saving the calibration data.
        /// </summary>
        public static string DefaultPath
        {
            get { return m_Path; }
        }

        public static string DefaultPathAndName
        {
            get { return m_Path + m_Name; }
        }
        private static string m_Path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/HI5";
        private static string m_Name = "/CalibrationData";

        /// <summary>
        /// Call it before doing B-pose calibration.
        /// </summary>
        public static void ResetCalibration()
        {
            //ruige 2018 11 9
            //Debug.Log("ResetCalibration");
            //HI5_Manager.GetGloveStatus().BposErr = BPoseCalibrationErrors.BE_NotCalibrated;
            // HI5_Manager.GetGloveStatus().BposReceiveResult = false;
            if(HI5_Manager_Thread.Instance() != null)
                HI5_Manager_Thread.Instance().AddCalibrationCommand(HI5_Operate_Command.HI5_Calibration_Send_Data.ECalibration_Command.EPreCalibration);
            //HI5_Device.PrepareCalibration();
        }

        /// <summary>
        /// Start B/P-pose calibration.
        /// </summary>
        /// <param name="pose">
        /// The type of calibration pose by <see cref="HI5.HI5_Pose"/>.
        /// </param>
        public static void StartCalibration(HI5_Pose pose)
        {
            if (pose == HI5_Pose.BPose)
                isCalibratingBPose = true;

            if (pose == HI5_Pose.PPose)
                isCalibratingPPose = true;

            if (pose == HI5_Pose.VPose)
                HI5_Calibration.ResetCalibration();

            CalibrationPose tranferPose = TransferPoseEnum(pose);

            if (pose == HI5_Pose.BPose  && HI5_Manager_Thread.Instance() != null)
                HI5_Manager_Thread.Instance().AddCalibrationCommand(HI5_Operate_Command.HI5_Calibration_Send_Data.ECalibration_Command.EBCalibration);

            if (pose == HI5_Pose.PPose && HI5_Manager_Thread.Instance() != null)
                HI5_Manager_Thread.Instance().AddCalibrationCommand(HI5_Operate_Command.HI5_Calibration_Send_Data.ECalibration_Command.EPCalibration);

            if (pose == HI5_Pose.VPose && HI5_Manager_Thread.Instance() != null)
                HI5_Manager_Thread.Instance().AddCalibrationCommand(HI5_Operate_Command.HI5_Calibration_Send_Data.ECalibration_Command.EVCalibration);
            // HI5_Device.StartCalibration(tranferPose);
        }

        /// <summary>
        /// Get the percent of B/P-pose calibration.
        /// </summary>
        /// <param name="pose">
        /// The type of calibration pose by <see cref="HI5.HI5_Pose"/>.
        /// </param>
        /// <returns>
        /// The progress of the related calibration. The value is provided by percent number.
        /// </returns>
        public static int GetCalibrationProgress(HI5_Pose pose)
        {
            CalibrationPose tranferPose = TransferPoseEnum(pose);
            int percent = 0;
            if (HI5_Manager_Thread.Instance() != null)
            {
                percent = (int)HI5_Manager_Thread.Instance().Calibrationpercent;
            }           
            if (pose == HI5_Pose.BPose && percent == 100)
            {
                //ruige 2018 11 5
                //SaveBindTrackedObjectInfo();
                isCalibratingBPose = false;
                //HI5_Manager.GetGloveStatus().IsBposComplete = true;
                //SetDefalutOffset();
            }
            if (pose == HI5_Pose.PPose && percent == 100)
            {
                //SaveCalibrationData();
                //ruige 2018 11 5
                //if (HI5_Log_Manager.Instance != null)
                //    HI5_Log_Manager.Instance.WriteLog();
                //Debug.Log("Save Calibration Data " + value);
                isCalibratingPPose = false;
            }
            if (pose == HI5_Pose.VPose && percent == 100)
            {
                //SaveCalibrationData();
                //ruige 2018 11 5
                //if (HI5_Log_Manager.Instance != null)
                //    HI5_Log_Manager.Instance.WriteLog();
                //Debug.Log("Save Calibration Data " + value);
                //isCalibratingPPose = false;
            }
            return percent;
            //return HI5_Device.GetCalibratingPercent(tranferPose);
        }

        /// <summary>
        /// Save calibration data to default path.
        /// </summary>
        /// <returns>
        /// True, successfull saved calibration data to default path.
        /// False, failed saved calibration data to default path.
        /// </returns>
        public static bool SaveCalibrationData()
        {
            return true;
//             if (!CheckDirectoryExists(m_Path))
//                 CreateDirectory(m_Path);
//             //ruige 2018 11 5
//           // Debug.Log("m_Path ="+ m_Path);
//             //Debug.Log("SN problem ----- HI5_Calibration SaveCalibrationData HI5_Manager.GetGloveStatus()" + HI5_Manager.GetGloveStatus().BposErr.ToString());
//             if (HI5_Manager.GetGloveStatus().BposErr == BPoseCalibrationErrors.BE_CalibratedOK)
//             {
//                 HI5_BindInfoManager.SaveItems();
//                 return HI5_Device.SaveCalibrationData(m_Path + m_Name);
//             }
//             else
//                 return false;
            
        }

        /// <summary>
        /// Get the serial number of tracked object binded on left/right hand.
        /// </summary>
        /// <param name="handType">
        /// The type of <see cref="HI5.Hand"/>.
        /// </param>
        /// <returns>
        /// The serial number of device.
        /// </returns>
        public static string GetBindedTrackedObjectSerialNumber(Hand handType)
        {
            OpticalSensorType sensorType = OpticalSensorType.OST_Unknown;
            GloveMod mod = HandTypeToGloveMod(handType);
            return HI5_Device.GetOptiSensorBindState(mod, ref sensorType);
        }

        /// <summary>
        /// Get the type of tracked object binded on left/right hand.
        /// </summary>
        /// <param name="handType">
        /// The type of <see cref="HI5.Hand"/>.
        /// </param>
        /// <returns>
        /// The type of optical device.
        /// </returns>
        public static OPTDeviceType GetBindedTrackedObjectType(Hand handType)
        {
            GloveMod mod = HandTypeToGloveMod(handType);
            OpticalSensorType sensorType = OpticalSensorType.OST_Unknown;
            HI5_Device.GetOptiSensorBindState(mod, ref sensorType);
            OPTDeviceType tranferType = SensorTypeToDeviceType(sensorType);
            return tranferType;
        }

        /// <summary>
        /// Set the serial number of tracked object binded on left/right hand. This function called only after loading the PairInfo file successfully.
        /// </summary>
        /// <param name="handType">
        /// The type of <see cref="HI5.Hand"/>.
        /// </param>
        /// <param name="serialNumber">
        /// The serial number of the binded device. Input by <see cref="System.String"/>.
        /// </param>
        /// <param name="deviceType">
        /// The type of <see cref="HI5.OPTDeviceType"/>.
        /// </param>
        public static void SetTrackedObjectBindState(Hand handType, string serialNumber, OPTDeviceType deviceType)
        {
            OpticalSensorType sensorType = DeviceTypeToSensorType(deviceType);
            GloveMod mod = HandTypeToGloveMod(handType);
            HI5_Device.SetOptiSensorBindState(mod, serialNumber, sensorType);
        }

        /// <summary>
        /// Load the previous calibration data. 
        /// </summary>
        /// <returns>
        /// True, successfully loaded the calibration data.
        /// False, failed loaded the calibration data.
        /// </returns>
        public static bool LoadCalibrationData()
        {
            return true;
//             if (!CheckFileExists(m_Path + m_Name))
//             {
//                 Hi5_Log.LogWarning("NOTE!!! Please do the Hi5 calibration before using Hi5 gloves.");
//                 return false;
//             }
// 
//             if (!HI5_Manager.IsDongleAvailable())
//             {
//                 return false;
//             }
//             
//             return HI5_Device.LoadCalibrationData(m_Path + m_Name);
        }


        //ruige  b_pos complete
        private static void SaveBindTrackedObjectInfo()
        {
            string leftSN = null;
            string rightSN = null;

            if(HI5_Manager_Thread.Instance() != null)
            {
                HI5_GloveStatus gloveStatus = HI5_Manager_Thread.Instance().GetGloveStatus();

                if (gloveStatus.IsLeftGloveAvailable)
                {
                    leftSN = HI5_Calibration.GetBindedTrackedObjectSerialNumber(Hand.LEFT);

                    if (leftSN != null)
                    {
                        if (leftSN == HI5_BindInfoManager.BindInfo.Right.SerialNumber)
                        {
                            HI5_BindInfoManager.BindInfo.Right.SerialNumber = "";
                            HI5_BindInfoManager.RightID = -1;
                        }

                        //Debug.Log("Binded left device serial number = " + leftSN.ToString());
                        OPTDeviceType leftType = HI5_Calibration.GetBindedTrackedObjectType(Hand.LEFT);
                        //Debug.Log("Binded left device type = " + leftType.ToString());
                        //ruige 2018 11 5
                        //Debug.Log("SN problem -----" + "HI5_Calibration SaveBindTrackedObjectInfo " + "BindInfo.Left.DeviceType" + leftType.ToString() + "SN " + leftSN.ToString() );
                        HI5_BindInfoManager.BindInfo.SetObjectSN(Hand.LEFT, leftSN);
                        HI5_BindInfoManager.BindInfo.SetDeviceType(Hand.LEFT, leftType);
                    }
                }

                if (gloveStatus.IsRightGloveAvailable)
                {
                    rightSN = HI5_Calibration.GetBindedTrackedObjectSerialNumber(Hand.RIGHT);
                    if (rightSN != null)
                    {
                        if (rightSN == HI5_BindInfoManager.BindInfo.Left.SerialNumber)
                        {
                            HI5_BindInfoManager.BindInfo.Left.SerialNumber = "";
                            HI5_BindInfoManager.LeftID = -1;
                        }
                        //Debug.Log("Binded right device serial number = " + rightSN.ToString());
                        OPTDeviceType rightType = HI5_Calibration.GetBindedTrackedObjectType(Hand.RIGHT);
                        //Debug.Log("Binded left device type = " + rightType.ToString());
                        //ruige 2018 11 5
                        //Debug.Log("SN problem -----" + "HI5_Calibration SaveBindTrackedObjectInfo " + "BindInfo.Right.DeviceType" + rightType.ToString() + "SN " + rightSN.ToString());
                        HI5_BindInfoManager.BindInfo.SetObjectSN(Hand.RIGHT, rightSN);
                        HI5_BindInfoManager.BindInfo.SetDeviceType(Hand.RIGHT, rightType);
                    }
                }
            }
            if (leftSN == null && rightSN == null)
                return;
            //ruige modify 
            //HI5_BindInfoManager.SaveItems();
            //Debug.Log("Saved binded info locally!");
        }
        private static void SetDefaultOffset()
        {
            if (HI5_Manager_Thread.Instance() != null)
            {
                HI5_GloveStatus gloveStatus = HI5_Manager_Thread.Instance().GetGloveStatus();
                if (gloveStatus.Status == GloveStatus.LeftGloveAvailable || gloveStatus.Status == GloveStatus.RightGloveAvailable)
                {
                    //HI5_Manager_Thread.SetDefaultHandOffset();
                }
            }           
        }
        public static CalibrationPose TransferPoseEnum(HI5_Pose inputPose)
        {
            if (inputPose == HI5_Pose.BPose)
                return CalibrationPose.GCP_BPose;
            else if (inputPose == HI5_Pose.PPose)
                return CalibrationPose.GCP_PPose;
            //else if (inputPose == HI5_Pose.APose)
            //    return CalibrationPose.GCP_APose;
            //else if (inputPose == HI5_Pose.TPose)
            //    return CalibrationPose.GCP_TPose;
            else if (inputPose == HI5_Pose.VPose)
                return CalibrationPose.GCP_VPose;
            else
                return CalibrationPose.GCP_Unknown;
        }
        private static GloveMod HandTypeToGloveMod(Hand type)
        {
            if (type == Hand.LEFT)
                return GloveMod.GM_LeftGlove;

            if (type == Hand.RIGHT)
                return GloveMod.GM_RightGlove;

            return GloveMod.GM_Unknown;
        }

        private static OPTDeviceType SensorTypeToDeviceType(OpticalSensorType type)
        {
            if (type == OpticalSensorType.OST_HTC_VIVE_Controller)
                return OPTDeviceType.HTC_VIVE_Controller;
            if (type == OpticalSensorType.OST_HTC_VIVE_Tracker)
                return OPTDeviceType.HTC_VIVE_Tracker;
            return OPTDeviceType.Unknown;
        }

        private static OpticalSensorType DeviceTypeToSensorType(OPTDeviceType type)
        {
            if (type == OPTDeviceType.HTC_VIVE_Controller)
                return OpticalSensorType.OST_HTC_VIVE_Controller;
            if (type == OPTDeviceType.HTC_VIVE_Tracker)
                return OpticalSensorType.OST_HTC_VIVE_Tracker;
            return OpticalSensorType.OST_Unknown;
        }

        #region System IO Operations
        private static bool CheckDirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        private static bool CheckFileExists(string path)
        {
            return File.Exists(path);
        }

        private static void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }
        internal static void RemoveFile(string path)
        {
            File.Delete(path);
        }

        #endregion


    }
}