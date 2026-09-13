using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.IO;
using System.Timers;
namespace HI5
{
    public class HI5_Manager_Thread
    {
        public static Vector3 leftOffset = new Vector3(-0.025f, 0.063f, -0.04f);
        public static Vector3 rightOffset = new Vector3(0.04f, -0.052f, -0.04f);

        public static float[] leftTrackerInitOffset = new float[] { -0.025f, 0.063f, 0.04f };
        public static float[] rightTrackerInitOffset = new float[] { 0.04f, -0.052f, 0.04f };
        public bool isInertia = false;
        public bool enableFingerAdbFixed = false;
        internal enum ECalibrationState
        {
            ECalibrationNone = 0,
            ECalibrationVing = 1,
            ECalibrationVend = 2,
            ECalibrationBing = 3,
            ECalibrationBend = 4,
            ECalibrationPing = 5,
            ECalibrationPend = 6
        }

        internal HI5_Operate_Command _Commands;
        private Thread phread = null;
        static internal HI5_MagneticStatus magneticStatus = new HI5_MagneticStatus();
        static internal HI5_GloveStatus m_HI5Status = new HI5_GloveStatus();
        internal Hi5_status m_Hi5_StatueData = new Hi5_status();
        internal HI5_Source m_HI5Source = new HI5_Source();
        internal Hi5_BVHDataQuene m_Hi5_BVHDataQuene = new Hi5_BVHDataQuene();
        internal ECalibrationState _CalibrationState = ECalibrationState.ECalibrationNone;
        internal bool isStop = false;
        SensorModulesOfDeviceStatus _SensorModulestatus = new SensorModulesOfDeviceStatus();
#if (UNITY_STANDALONE_WIN)
        public HI5_TrackedDeviceInterface m_leftTrackedDevice = null;
        public HI5_TrackedDeviceInterface m_rightTrackedDevice = null;
#endif
        GloveBVHData gloveData;
        //判断startdongle
        //
        internal bool IsStartDongle = false;
        public bool IsCalibrationComplete = false;
        internal bool IsLoadData = false;

        public bool IsHaveLoadFile = false;
        public Hi5_Thread_MonoBehaviour mHi5ThreadMonoBehaviour = null;
        //只用作判断是否连接
        internal bool IsConected
        {
            get
            {
                //bool temp =  (Interlocked.Read(ref isConnected) == 1) ?  true :  false;
                return isConnectedTem;
            }
            set
            {
                //if (value)
                //    Interlocked.Exchange(ref isConnected, 1);
                //else
                //    Interlocked.Exchange(ref isConnected, 0);
                isConnectedTem = value;
            }
        }
        internal long isConnected = 0;
        internal bool isConnectedTem = false;
        public long Calibrationpercent
        {
            get
            {
                return Interlocked.Read(ref _calibrationpercent);
            }
            set
            {
                Interlocked.Exchange(ref _calibrationpercent, value);
            }
        }
        internal long _calibrationpercent = 0;
        internal SensorInforS _sensorInfor = null;
        private static HI5_Manager_Thread _instance;
        private static readonly object _lockInstance = new object();
        public static HI5_Manager_Thread Instance()
        {
            if (_instance == null)
            {
                lock (_lockInstance)
                {
                    if (_instance == null)
                    {
                        _instance = new HI5_Manager_Thread();
                        _instance.init();
                        // LogFileModule.Open();
                    }
                }
            }
            return _instance;
        }
        public static void Clean()
        {
            //LogFileModule.Close();
            _instance = null;
        }
        internal HI5_Operate_Command GetCommand()
        {
            return _Commands;
        }
        GloveBVHData itemBvhData = new GloveBVHData();
        internal void Update()
        {
         
            if (!HI5_Device.IsDongleAvailable())
            {
                IsConected = false;
            }
            else
            {
                IsConected = true;
            }
            if (IsConected && IsStartDongle)
            {
                if (m_HI5Status != null)
                    m_HI5Status.MainThreadUpdate();
                bool isLeftUpdate = false;
                GloveBVHData leftData = new GloveBVHData();
                isLeftUpdate = m_Hi5_BVHDataQuene.GetBvhtemLeft(ref leftData);
                if (isLeftUpdate)
                {
                    m_HI5Source.UpdateTrackingData(leftData);
                }

                bool isRightUpdate = false;
                GloveBVHData RightData = new GloveBVHData();
                isRightUpdate = m_Hi5_BVHDataQuene.GetBvhtemRight(ref RightData);
                if (isRightUpdate)
                {

                    m_HI5Source.UpdateTrackingData(RightData);
                }
            }
            if (magneticStatus != null)
                magneticStatus.Update(Time.deltaTime);

        }
        internal void init()
        {
            _Commands = new HI5_Operate_Command();
            _Commands.init();
            IsStartDongle = false;
            IsConected = false;
            _CalibrationState = ECalibrationState.ECalibrationNone;
            _sensorInfor = new SensorInforS();
            _sensorInfor.init();
            _SensorModulestatus.Create();
            _SensorModulestatus = new SensorModulesOfDeviceStatus();
            m_HI5Status.m_Hi5_StatueData = m_Hi5_StatueData;
            if (CheckLoadFile())
                IsHaveLoadFile = true;
            else
                IsHaveLoadFile = false;
        }
        //校准指令
        internal void AddCalibrationCommand(HI5_Operate_Command.HI5_Calibration_Send_Data.ECalibration_Command param)
        {
            Calibrationpercent = 0;
            if (IsStartDongle && IsConected)
                _Commands.AddCalibrationCommand(param);
        }
        internal void AddOpticData(string sensorIdentity, OpticalSensorType type, float[] fpos, float[] frot)
        {
            if (IsStartDongle && IsConected)
                _Commands.AddOpticData(sensorIdentity, type, fpos, frot);
        }
        internal void AddOptiSensorBindData(GloveMod mod, string trackerID, OpticalSensorType sensorType)
        {
            if (IsStartDongle && IsConected)
                _Commands.AddOptiSensorBindData(mod, trackerID, sensorType);
        }
        internal void GamePause(bool isPause)
        {
            if (isPause)
            {
                SynCloseConnectEquipment();
                IsStartDongle = false;
            }
            else
            {
                RegisterEvent();
                SynConnectEquipment(false);
                IsStartDongle = true;
            }
        }
        internal void RequestConnect(bool inertia)
        {
            if (!IsStartDongle)
            {
                StartThread();
                RegisterEvent();
                SynConnectEquipment(inertia);
                IsStartDongle = true;
            }
        }
        internal void RequestCloseConnect()
        {
            if (IsStartDongle)
            {
                SynCloseConnectEquipment();
                StopThread();
                IsStartDongle = false;
            }
            if (IsConected)
            {
                IsConected = false;
            }
        }
        internal void StartThread()
        {
            if (phread != null)
            {
                phread.Abort();
                phread = null;
            }
            phread = new Thread(new ThreadStart(AsynUpdateThread));
            phread.Start();
        }

        void AsynUpdateThread()
        {
            try
            {
                while (!isStop)
                {
                    if (_Commands != null)
                    {
                        if (IsConected && IsStartDongle)
                        {
                            HI5_Operate_Command.HI5_Operate_Data commandItem = null;
                            {
                                //校准数据
                                HI5_Operate_Command.HI5_Calibration_Send_Data CalibarationData = _Commands.GetSendCalibration();
                                if (CalibarationData != null)
                                {
                                    while (CalibarationData.IsHas())
                                    {
                                        HI5_Operate_Command.HI5_Calibration_Send_Data.ECalibration_Command command = CalibarationData.Get();
                                        AsynOperateCalibrationThread(command);
                                    }
                                    CalibarationData.Clean();
                                }
                                else
                                {
                                    //获取校准百分比
                                    if (_CalibrationState == ECalibrationState.ECalibrationVing
                                        || _CalibrationState == ECalibrationState.ECalibrationBing
                                        || _CalibrationState == ECalibrationState.ECalibrationPing)
                                    {
                                        Calibrationpercent = AsyGetClibrationPercent();
                                        //WriteCalibrationPercent();
                                    }

                                }
                                //发送光学数据
                                //左手光学数据
                                commandItem = _Commands.GetSendOpticData();
                                if (commandItem != null)
                                {
                                    //发送左手光学数据
                                    HI5_Optic_DataS item = (HI5_Optic_DataS)commandItem._Data;
                                    while (item.IsHas())
                                    {
                                        HI5_Optic_Data temp = item.Get();
                                        AsynSendOpticData(temp._sensorIdentity, temp._type, temp._fpos, temp._frot);
                                    }
                                    item.Clean();
                                }
                                commandItem = _Commands.GetSenserOpticData();
                                if (commandItem != null)
                                {
                                    OptiSensorBindDataS item = (OptiSensorBindDataS)commandItem._Data;
                                    while (item.IsHas())
                                    {
                                        OptiSensorBindData temp = item.Get();
                                        AsynSendOpticBindData(temp._mod, temp._trackerID, temp._sensorType);
                                    }
                                    item.Clean();
                                }
                                AsynReceivThreadData();
                            }
                        }
                    }
                    else
                    {
                        if (IsConected && IsStartDongle)
                        {
                            AsynReceivThreadData();
                        }
                    }
                    Thread.Sleep(17);
                }
            }
            catch (ThreadAbortException ex)
            {
                Debug.Log("Method1 ThreadAbortException: " + ex.ToString());
            }
            catch (Exception ex)
            {
                Debug.Log("Method1 Exception: " + ex.ToString());
            }
            finally
            {
                Debug.Log("Method1 Finally:");
            }
        }

        internal void StopThread()
        {
            try
            {
                if (phread != null)
                {
                    isStop = true;
                    phread.Join();
                    //  phread.Abort();
                    phread = null;
                }
            }
            catch (Exception ex)
            {
                Debug.Log("StopMethod1: " + ex.ToString());
            }
        }

        //internal void WriteCalibrationPercent()
        //{
        //    if(_CalibrationState == ECalibrationState.ECalibrationVing)
        //    {
        //        ThreadLog.addLog("ECalibrationState.ECalibrationVing"+ Calibrationpercent);
        //    }
        //   if(_CalibrationState == ECalibrationState.ECalibrationBing)
        //    {
        //        ThreadLog.addLog("ECalibrationState.ECalibrationBing" + Calibrationpercent);
        //    }
        //    if (_CalibrationState == ECalibrationState.ECalibrationPing)
        //    {
        //        ThreadLog.addLog("ECalibrationState.ECalibrationPing" + Calibrationpercent);
        //    }

        //}

        //public static void ConnectEquipment()
        //{
        //    HI5_Device.StartHI5Dongle();
        //    HI5_Device.SetReducedOutputDataFreq(1);
        //    HI5_Device.SetWristPositionInTrackerFrame(leftTrackerInitOffset, rightTrackerInitOffset);
        //}

        //public static void CloseConnectEquipment()
        //{
        //    if(HI5_Manager_Thread.Instance() != null && HI5_Manager_Thread.Instance().IsConected)
        //        HI5_Device.StopHI5Dongle();
        //    //HI5_Device.SetReducedOutputDataFreq(1);
        //    //HI5_Device.SetWristPositionInTrackerFrame(leftTrackerInitOffset, rightTrackerInitOffset);
        //}

        internal void SetHandThickness(float param)
        {
            HI5_Device.SetHandThickness(param);
        }

        private void RegisterEvent()
        {
            RegisterGloveStateChangedCallback();
            RegisterDongleStateChangedHandle();
        }
        //连接设备
        void SynConnectEquipment(bool inertia)
        {
            if (inertia)
                HI5_Device.AppendSupportFlags("WithoutTracker");
            else
            {
                if(HI5_Device.TestSupportFlags("WithoutTracker"))
                {
                    HI5_Device.RemoveSupportFlags("WithoutTracker");
                }
            }
            HI5_Device.StartHI5Dongle();
            HI5_Device.ForceWorkingHand(WorkingHand.WH_DoubleHands);
            HI5_Device.SetReducedOutputDataFreq(1);
            HI5_Device.EnableFingerAdbFixed(enableFingerAdbFixed);
            HI5_Device.SetWristPositionInTrackerFrame(leftTrackerInitOffset, rightTrackerInitOffset);
            SetVibratorSupport(true);
            //只有android 发送 
#if UNITY_ANDROID
                        SetBindTrackedObjectInfo(true);
#endif
            IsStartDongle = true;
            isStop = false;
            IsLoadData = true;
        }

        //关闭设备
        public void SynCloseConnectEquipment()
        {
            if (HI5_Device.TestSupportFlags("WithoutTracker"))
            {
                HI5_Device.RemoveSupportFlags("WithoutTracker");
            }
   
            isStop = true;
            IsStartDongle = false;
            HI5_Device.StopHI5Dongle();
            m_HI5Status.ReleaseBothTimerThread();
            
        }

        //处理操作指令
        void AsynOperateCalibrationThread(HI5_Operate_Command.HI5_Calibration_Send_Data.ECalibration_Command param)
        {
            switch (param)
            {
                case HI5_Operate_Command.HI5_Calibration_Send_Data.ECalibration_Command.EPreCalibration:
                    Calibrationpercent = 0;
                    HI5_Device.PrepareCalibration();
                    break;
                case HI5_Operate_Command.HI5_Calibration_Send_Data.ECalibration_Command.EVCalibration:
                    HI5_Device.StartCalibration(CalibrationPose.GCP_VPose);
                    Calibrationpercent = 0;
                    _CalibrationState = ECalibrationState.ECalibrationVing;
                    break;

                case HI5_Operate_Command.HI5_Calibration_Send_Data.ECalibration_Command.EBCalibration:
                    HI5_Device.StartCalibration(CalibrationPose.GCP_BPose);
                    Calibrationpercent = 0;
                    _CalibrationState = ECalibrationState.ECalibrationBing;
                    break;
                case HI5_Operate_Command.HI5_Calibration_Send_Data.ECalibration_Command.EPCalibration:
                    HI5_Device.StartCalibration(CalibrationPose.GCP_PPose);
                    Calibrationpercent = 0;
                    _CalibrationState = ECalibrationState.ECalibrationPing;
                    break;
                default:
                    break;
            }
        }

        private static bool CheckDirectoryExists(string path)
        {
            return Directory.Exists(path);
        }
        public  string DefaultPathAndName
        {
            get {
                string param;
                if (isInertia)
                    param = m_Path + m_InertiaName;
                else
                    param = m_Path + m_Name;
                return param; }
        }
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        private static string m_Path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/HI5";
#else
         private static string m_Path = Application.persistentDataPath;
#endif
        private static string m_Name = "/CalibrationData.xml";
        private static string m_InertiaName = "/CalibrationInertiaData.xml";
        private static bool CheckFileExists(string path)
        {
            return File.Exists(path);
        }
        private static void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }
        public bool CheckLoadFile()
        {
            if (isInertia)
                return CheckFileExists(m_Path + m_InertiaName);
            else
                return CheckFileExists(m_Path + m_Name);
        }
        public bool LoadCalibrationData()
        {
            string param;
            if (isInertia)
                param = m_Path + m_InertiaName;
            else
                param = m_Path + m_Name;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR

            if (!CheckFileExists(param))
            {
                Debug.LogWarning("NOTE!!! Please do the Hi5 calibration before using Hi5 gloves.");
                return false;
            }
            //Debug.LogError("LoadCalibrationData");
            return HI5_Device.LoadCalibrationData(param);
#else
            
            if (!CheckFileExists(param))
            {
               // Hi5_Log.LogWarning("NOTE!!! Please do the Hi5 calibration before using Hi5 gloves.");
                return false;
            }
            Debug.LogError("LoadCalibrationData");
            return HI5_Device.LoadCalibrationData(m_Path + m_Name); 
          
#endif
        }
        public bool SaveCalibrationData()
        {
            string param;
            if (isInertia)
                param = m_Path + m_InertiaName;
            else
                param = m_Path + m_Name;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            if (!CheckDirectoryExists(m_Path))
                CreateDirectory(m_Path);
            //ruige 2018 11 5
            // Debug.Log("m_Path ="+ m_Path);
            //Debug.Log("SN problem ----- HI5_Calibration SaveCalibrationData HI5_Manager.GetGloveStatus()" + HI5_Manager.GetGloveStatus().BposErr.ToString());
            IsHaveLoadFile = true;
           
            return HI5_Device.SaveCalibrationData(param);
#else 
            if (!CheckDirectoryExists(m_Path))
                CreateDirectory(m_Path);
             Debug.Log("m_Path ="+ m_Path);
             return HI5_Device.SaveCalibrationData(param);
#endif
        }
        int AsyGetClibrationPercent()
        {
            int percent = 0;
            switch (_CalibrationState)
            {
                case ECalibrationState.ECalibrationVing:
                    percent = GetCalibrationProgress(HI5_Pose.VPose);
                    if (percent == 100)
                    {
                        _CalibrationState = ECalibrationState.ECalibrationVend;
                        // SaveCalibrationData();
                    }
                    break;
                case ECalibrationState.ECalibrationBing:
                    percent = GetCalibrationProgress(HI5_Pose.BPose);
                    if (percent == 100)
                        _CalibrationState = ECalibrationState.ECalibrationBend;
                    break;
                case ECalibrationState.ECalibrationPing:
                    percent = GetCalibrationProgress(HI5_Pose.PPose);
                    if (percent == 100)
                    {
                        if(mHi5ThreadMonoBehaviour != null)
                            mHi5ThreadMonoBehaviour.ResetHandRoot();
                        _CalibrationState = ECalibrationState.ECalibrationPend;
                        IsCalibrationComplete = true;
                    }
                    break;
            }
            return percent;
        }

        //发送光学数据
        void AsynSendOpticData(string sensorIdentity,
                                OpticalSensorType type,
                                float[] fpos,
                                float[] frot)
        {
            //LogFileModule.GetInstance().ThreadWrite("AsynSendOpticData", "AsynSendOpticData", LogType.Error);
            //for(int i =0; i<3 ;i++)
            //{
            //    string temp = "i=" + i;
            //    temp += "    fpos =  " + fpos[i];
            //    LogFileModule.GetInstance().ThreadWrite(temp, "AsynSendOpticData", LogType.Error);
            //}
            //for (int i = 0; i < 4; i++)
            //{
            //    string temp = "i=" + i;
            //    temp += "    frot =  " + frot[i];
            //    LogFileModule.GetInstance().ThreadWrite(temp, "AsynSendOpticData", LogType.Error);
            //}
            HI5_Device.PushOpticalSensorData(sensorIdentity, type, fpos, frot);
        }
        //发送绑定信息数据
        void AsynSendOpticBindData(GloveMod mod, string trackerID, OpticalSensorType sensorType)
        {
            HI5_Device.SetOptiSensorBindState(mod, trackerID, sensorType);
        }

        //接收数据
        void AsynReceivThreadData()
        {
            //LogFileModule.GetInstance().ThreadWrite("AsynReceiv", "Asy", LogType.Error);
            //读取手姿态数据
            gloveData.bUpdate = 0;
            HI5_Device.ReadBVHData(ref gloveData);
            while (gloveData.bUpdate == 1)
            {
                //  LogFileModule.GetInstance().ThreadWrite("AsynReceivThreadData update", "Asyn", LogType.Error);
                m_Hi5_BVHDataQuene.WriteBvhData(gloveData);
                gloveData.bUpdate = 0;
                HI5_Device.ReadBVHData(ref gloveData);
                if (gloveData.bUpdate == 0)
                    break;
            }
            //Debug.Log("AsynReceivThreadData");
            if (IsConected && IsStartDongle)
            {
                if(!isStop)
                {
                    HI5GloveStatus newStatus = HI5_Device.ReadDeviceStatus();
                    GloveStatus _newSatus = ConvertHI5GloveStatus(newStatus);
                    if (m_HI5Status.currentStatus != _newSatus)
                    {
                        m_HI5Status.currentStatus = _newSatus;
                        m_Hi5_StatueData.WriteStatueData(_newSatus);
                    }

                    m_HI5Status.UpdateGloveVisible(_newSatus);
                    //读取sensor信息
                    int result = HI5_Device.ReadSensorStatus(ref _SensorModulestatus);
                    _sensorInfor.Set(_SensorModulestatus);
                }
                
            }
            else
            {

            }
            
        }

        private void RegisterGloveStateChangedCallback()
        {
            HI5_Device.setGloveStateChangedHandle(IntPtr.Zero, OnGloveStateChanged);
        }

        private void RegisterDongleStateChangedHandle()
        {
            HI5_Device.setDongleStateChangedHandle(IntPtr.Zero, DongleStateChangedCallback);
        }


        static private void DongleStateChangedCallback(IntPtr customObject, IntPtr hid, DongleEvent action)
        {
            if (action == DongleEvent.DE_Insert)
            {
                int a = 10;
            }
            else if (action == DongleEvent.DE_Removed)

            {
                //清楚数据
                if (HI5_Manager_Thread.Instance() != null && HI5_Manager_Thread.Instance()._sensorInfor != null)
                {
                    HI5_Manager_Thread.Instance()._sensorInfor.CleanData();
                }

            }
        }


        static private void OnGloveStateChanged(IntPtr cutomObject, GloveMod gloveMod, GloveEvent gloveEvent)
        {
            magneticStatus.OnEventChanged(gloveMod, gloveEvent);
            m_HI5Status.OnEventChanged(gloveMod, gloveEvent);
        }

        internal int GetCalibrationProgress(HI5_Pose pose)
        {
            CalibrationPose tranferPose = HI5_Calibration.TransferPoseEnum(pose);
            int percent = HI5_Device.GetCalibratingPercent(tranferPose);

            if (pose == HI5_Pose.BPose && percent == 100)
            {
                _CalibrationState = ECalibrationState.ECalibrationBend;
            }
            if (pose == HI5_Pose.PPose && percent == 100)
            {
                _CalibrationState = ECalibrationState.ECalibrationPend;
            }
            if (pose == HI5_Pose.VPose && percent == 100)
            {
                _CalibrationState = ECalibrationState.ECalibrationVend;
            }
            return percent;
        }
        internal GloveStatus ConvertHI5GloveStatus(HI5GloveStatus status)
        {
            switch (status)
            {
                case HI5GloveStatus.DV_Unknown:
                    return GloveStatus.Unknown;
                case HI5GloveStatus.DV_NoDongle:
                    return GloveStatus.NoDongle;
                case HI5GloveStatus.DV_NoGlove:
                    return GloveStatus.NoGlove;
                case HI5GloveStatus.DV_LeftGloveAvailable:
                    return GloveStatus.LeftGloveAvailable;
                case HI5GloveStatus.DV_RightGloveAvailable:
                    return GloveStatus.RightGloveAvailable;
                case HI5GloveStatus.DV_BothGloveAvailable:
                    return GloveStatus.BothGloveAvailable;
            }
            return GloveStatus.Unknown;
        }

        public bool SetBindTrackedObjectInfo(bool isLoad)
        {
            bool IsAndroid = false;
#if UNITY_ANDROID
            IsAndroid = true;
#endif
#if (UNITY_STANDALONE_WIN || UNITY_EDITOR)
            IsAndroid = false;
#endif
            if (!IsAndroid)
            {
                if (!isLoad)
                {
                    OpticalSensorType leftSensorType = DeviceTypeToSensorType(HI5_BindInfoManager.BindInfo.Left.DeviceType);
                    OpticalSensorType rightSensorType = DeviceTypeToSensorType(HI5_BindInfoManager.BindInfo.Right.DeviceType);

                    if (HI5_BindInfoManager.BindInfo.Left.SerialNumber != null && HI5_BindInfoManager.BindInfo.Left.SerialNumber.Length > 0)
                    {
                        //Debug.Log("LeftSerialNumber");
                        AddOptiSensorBindData(GloveMod.GM_LeftGlove, HI5_BindInfoManager.BindInfo.Left.SerialNumber, leftSensorType);
                    }
                    if (HI5_BindInfoManager.BindInfo.Right.SerialNumber != null && HI5_BindInfoManager.BindInfo.Right.SerialNumber.Length > 0)
                    {
                        //  Debug.Log("RightSerialNumber");
                        AddOptiSensorBindData(GloveMod.GM_RightGlove, HI5_BindInfoManager.BindInfo.Right.SerialNumber, rightSensorType);
                    }

                }
                else
                {
                    if (LoadBindTrackedObjectsInfo())
                    {
                        OpticalSensorType leftSensorType = DeviceTypeToSensorType(HI5_BindInfoManager.BindInfo.Left.DeviceType);
                        OpticalSensorType rightSensorType = DeviceTypeToSensorType(HI5_BindInfoManager.BindInfo.Right.DeviceType);
                        if (HI5_BindInfoManager.BindInfo.Left.SerialNumber != null && HI5_BindInfoManager.BindInfo.Left.SerialNumber.Length > 0)
                            AddOptiSensorBindData(GloveMod.GM_LeftGlove, HI5_BindInfoManager.BindInfo.Left.SerialNumber, leftSensorType);
                        if (HI5_BindInfoManager.BindInfo.Right.SerialNumber != null && HI5_BindInfoManager.BindInfo.Right.SerialNumber.Length > 0)
                            AddOptiSensorBindData(GloveMod.GM_RightGlove, HI5_BindInfoManager.BindInfo.Right.SerialNumber, rightSensorType);
                        //HI5_Device.SetOptiSensorBindState(GloveMod.GM_LeftGlove, HI5_BindInfoManager.BindInfo.Left.SerialNumber, leftSensorType);
                        //HI5_Device.SetOptiSensorBindState(GloveMod.GM_RightGlove, HI5_BindInfoManager.BindInfo.Right.SerialNumber, rightSensorType);
                        return true;
                    }
                }
                return true;
            }
            else
            {
                AddOptiSensorBindData(GloveMod.GM_LeftGlove, "Pico Neo3 Controller left",
                        OpticalSensorType.OST_HTC_VIVE_Tracker);
                AddOptiSensorBindData(GloveMod.GM_RightGlove, "Pico Neo3 Controller right",
                    OpticalSensorType.OST_HTC_VIVE_Tracker);
                //HI5_Device.SetOptiSensorBindState(GloveMod.GM_LeftGlove, "Pico Neo3 Controller left",
                //    OpticalSensorType.OST_HTC_VIVE_Tracker);
                //HI5_Device.SetOptiSensorBindState(GloveMod.GM_RightGlove, "Pico Neo3 Controller right",
                //    OpticalSensorType.OST_HTC_VIVE_Tracker);
                return true;
            }
            //if (HI5_Device.IsDongleAvailable())
            //{
            //    if (LoadBindTrackedObjectsInfo())
            //    {
            //        OpticalSensorType leftSensorType = DeviceTypeToSensorType(HI5_BindInfoManager.BindInfo.Left.DeviceType);
            //        OpticalSensorType rightSensorType = DeviceTypeToSensorType(HI5_BindInfoManager.BindInfo.Right.DeviceType);
            //        AddOptiSensorBindData(GloveMod.GM_LeftGlove, HI5_BindInfoManager.BindInfo.Left.SerialNumber, leftSensorType);
            //        AddOptiSensorBindData(GloveMod.GM_RightGlove, HI5_BindInfoManager.BindInfo.Right.SerialNumber, rightSensorType);
            //        //HI5_Device.SetOptiSensorBindState(GloveMod.GM_LeftGlove, HI5_BindInfoManager.BindInfo.Left.SerialNumber, leftSensorType);
            //        //HI5_Device.SetOptiSensorBindState(GloveMod.GM_RightGlove, HI5_BindInfoManager.BindInfo.Right.SerialNumber, rightSensorType);
            //        return true;
            //    }
            //    else
            //    {
            //        AddOptiSensorBindData(GloveMod.GM_LeftGlove, "Pico Neo3 Controller left",
            //            OpticalSensorType.OST_HTC_VIVE_Tracker);
            //        AddOptiSensorBindData(GloveMod.GM_RightGlove, "Pico Neo3 Controller right",
            //            OpticalSensorType.OST_HTC_VIVE_Tracker);
            //        //HI5_Device.SetOptiSensorBindState(GloveMod.GM_LeftGlove, "Pico Neo3 Controller left",
            //        //    OpticalSensorType.OST_HTC_VIVE_Tracker);
            //        //HI5_Device.SetOptiSensorBindState(GloveMod.GM_RightGlove, "Pico Neo3 Controller right",
            //        //    OpticalSensorType.OST_HTC_VIVE_Tracker);
            //        return true;
            //    }
            //}
            //return false;
        }
        private OpticalSensorType DeviceTypeToSensorType(OPTDeviceType type)
        {
            if (type == OPTDeviceType.HTC_VIVE_Controller)
                return OpticalSensorType.OST_HTC_VIVE_Controller;
            if (type == OPTDeviceType.HTC_VIVE_Tracker)
                return OpticalSensorType.OST_HTC_VIVE_Tracker;
            return OpticalSensorType.OST_Unknown;
        }

        private static bool LoadBindTrackedObjectsInfo()
        {
            return HI5_BindInfoManager.LoadItems(true);
        }

        public HI5_GloveStatus GetGloveStatus()
        {
            return m_HI5Status;
        }

        public HI5_Source GetHI5Source()
        {
            return m_HI5Source;
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
        public static void BindBones(Transform root, Transform[] bones, string prefix, Hand handType)
        {
            //HI5_Helper.Bind(root, bones, prefix, handType);
            HI5_Bones.Bind(root, bones, prefix, handType);
        }

        public  void SetVibratorSupport(bool isVibrator)
        {
            if (isVibrator)
            {
                HI5_Device.EnableFreeVibratorSupport();

            }
            else
            {
                HI5_Device.DisableFreeVibratorSupport();
            }
        }

        public  void Vibrate(int vibrationIndex, 
            int vibrateLevel,
            uint duration,
            uint times)
        {
            VibrationParam paramTemp;
            if (vibrationIndex == 1)
                paramTemp.vibratorTag = VibratorTag.VT_1;
            else if (vibrationIndex == 2)
                paramTemp.vibratorTag = VibratorTag.VT_2;
            else if(vibrationIndex == 3)
                paramTemp.vibratorTag = VibratorTag.VT_3;
            else if (vibrationIndex == 4)
                paramTemp.vibratorTag = VibratorTag.VT_4;
            else
                paramTemp.vibratorTag = VibratorTag.VT_All;
            paramTemp.vibrationEffect.times = times;
            paramTemp.vibrationEffect.duration = duration;
            if (vibrateLevel == 1)
                paramTemp.vibrationEffect.vibratingLevel = VibratingLevel.VL_Low;
            else if (vibrateLevel == 2)
                paramTemp.vibrationEffect.vibratingLevel = VibratingLevel.VL_Middle;
            else if (vibrateLevel == 3)
                paramTemp.vibrationEffect.vibratingLevel = VibratingLevel.VL_High;
            else
                paramTemp.vibrationEffect.vibratingLevel = VibratingLevel.VL_High;
            HI5_Device.QueuedFreeVibratorVibration(ref paramTemp);
        }
    }
}
