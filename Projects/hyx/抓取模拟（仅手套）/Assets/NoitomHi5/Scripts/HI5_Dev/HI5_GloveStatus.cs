using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Timers;

namespace HI5
{ 
    /// <summary>
    /// The connecting status of HI5 glove.
    /// </summary>
    public enum GloveStatus
    {
        /// <summary>
        /// The unknown status of glove.
        /// </summary>
        Unknown = -1,
        /// <summary>
        /// No dongle connected.
        /// </summary>
        NoDongle,
        /// <summary>
        /// No glove connected.
        /// </summary>
        NoGlove,
        /// <summary>
        /// The left glove is available.
        /// </summary>
        LeftGloveAvailable,
        /// <summary>
        /// The right glove is available.
        /// </summary>
        RightGloveAvailable,
        /// <summary>
        /// Both gloves are available.
        /// </summary>
        BothGloveAvailable,
    }

    /// <summary>
    /// Manage all the status of HI5 glove.
    /// </summary>
    public class HI5_GloveStatus
    {
        /// <summary>
        /// The power level of left glove.
        /// </summary>
        public PowerLevel LeftPower
        { 
            get 
            { 
                return (PowerLevel)Interlocked.Read(ref leftPL);
            }
            set
            {
                Interlocked.Exchange(ref leftPL, (long)value);
            }
        }
        private long leftPL = (long)PowerLevel.Full;

        /// <summary>
        /// The power level of right glove.
        /// </summary>
        public PowerLevel RightPower
        {
            get
            {
                return (PowerLevel)Interlocked.Read(ref rightPL);
            }
            set
            {
                Interlocked.Exchange(ref rightPL, (long)value);
            }
        }
        private long rightPL = (long)PowerLevel.Full;

        /// <summary>
        /// The magnetic field status around left glove.
        /// </summary>
        public MagneticStatus LeftMagneticStatus
        {
            get
            {
                return (MagneticStatus)Interlocked.Read(ref leftMS);
            }
            set
            {
                Interlocked.Exchange(ref leftMS, (long)value);
            }
           
        }
        private long leftMS;

        public MagneticStatus leftMS_temp
        {
            get
            {
                return (MagneticStatus)Interlocked.Read(ref _leftMS_temp);
            }
            set
            {
                Interlocked.Exchange(ref _leftMS_temp, (long)value);
            }

        }

        private long _leftMS_temp = (long)MagneticStatus.Unknown;

        /// <summary>
        /// The magnetic field status around right glove.
        /// </summary>
        public MagneticStatus RightMagneticStatus
        {
            get
            {
                return (MagneticStatus)Interlocked.Read(ref rightMS);
            }
            set
            {
                Interlocked.Exchange(ref rightMS, (long)value);
            }
        }
        private long rightMS;

        public MagneticStatus rightMS_temp
        {
            get
            {
                return (MagneticStatus)Interlocked.Read(ref _rightMS_temp);
            }
            set
            {
                Interlocked.Exchange(ref _rightMS_temp, (long)value);
            }
        }
        private long _rightMS_temp = (long)MagneticStatus.Unknown;
        /// <summary>
        /// Check whether the left glove is available.
        /// </summary>
        /// 
        public bool IsLeftGloveAvailable
        {
            get
            {
                //bool temp = (Interlocked.Read(ref isLeftGloveAvailable) == 1) ? true : false;
                //return temp;
                return isLeftGloveAvailableTemp;
            }
            set
            {
                //if (value)
                //    Interlocked.Exchange(ref isLeftGloveAvailable, 1);
                //else
                //    Interlocked.Exchange(ref isLeftGloveAvailable, 0);
                isLeftGloveAvailableTemp = value;
            }

        }
        private long isLeftGloveAvailable = 0;
        private bool isLeftGloveAvailableTemp = false;
        /// <summary>
        /// Check whether the right glove is available.
        /// </summary>
        public bool IsRightGloveAvailable
        {
            get
            {
                //bool temp = (Interlocked.Read(ref isRightGloveAvailable) == 1) ? true : false;
                //return temp;
                return isRightGloveAvailableTemp;
            }
            set
            {
                //if (value)
                //    Interlocked.Exchange(ref isRightGloveAvailable, 1);
                //else
                //    Interlocked.Exchange(ref isRightGloveAvailable, 0);
                isRightGloveAvailableTemp = value;
            }
        }
        private long isRightGloveAvailable = 0;
        private bool isRightGloveAvailableTemp = false;
        //public Action<Hand, PowerLevel> OnPowerLevelChanged;
        //public Action<Hand, MagneticStatus> OnMagneticStateChanged;

        /// <summary>
        /// Call when glove connecting status changed.
        /// </summary>
        public Action<GloveStatus> OnStatusChanged;

        /// <summary>
        /// Get the current glove status.
        /// </summary>
        public GloveStatus Status
        {
            get {
                //lock (_lockercurrentStatus)
                {
                    return currentStatus;
                }
            }
        }

        System.Object isCalibrationBposSuccesLock = new System.Object();

        //bpose 失败 bopose 开始false 成功 改为 true

        /// <summary>
        /// /// <summary>
        /// Get the Calibration Bpos Result.
        /// </summary>
        //public bool IsCalibrationBposSuccess
        //{
        //    get {
        //        lock(isCalibrationBposSuccesLock)
        //            return isCalibrationBposSucces;
        //    }
        //    set
        //    {
        //        lock (isCalibrationBposSuccesLock)
        //            isCalibrationBposSucces = value;
        //    }
        //}

        //ruige 2018 11 9
        //System.Object isReceiveBPosResultLock = new System.Object();
        //internal bool isReceiveBPosResult = false;
        //public bool BposReceiveResult
        //{
        //    get
        //    {
        //        lock (isReceiveBPosResultLock)
        //            return isReceiveBPosResult;
        //    }
        //    set
        //    {
        //        lock (isReceiveBPosResultLock)
        //            isReceiveBPosResult = value;
        //    }
        //}

        //bpos result
        //private BPoseCalibrationErrors bposErr = BPoseCalibrationErrors.BE_CalibratedOK;
        //System.Object bposErrLock = new System.Object();
        //public BPoseCalibrationErrors BposErr
        //{
        //    get
        //    {
        //        lock (bposErrLock)
        //            return bposErr;
        //    }
        //    set
        //    {
        //        lock (bposErrLock)
        //            bposErr = value;
        //    }
        //}

        public void StartCalibrationBpos()
        {
           // IsCalibrationBposSuccess = false;
           // IsBposComplete = false;
        }

        ///  <summary>
        /// Get the Calibration Bpos Result.
        /// </summary>
        //public bool isGloveBPosSuccess()
        //{
        //    if (IsCalibrationBposSuccess)
        //        Hi5_Log.Log("IsCalibrationBposSuccess TRUE");
        //    if (IsBposComplete)
        //        Hi5_Log.Log("IsBposComplete TRUE");
        //    if (IsCalibrationBposSuccess && IsBposComplete)
        //        return true;
        //    else
        //        return false;
        //}

        //internal bool IsBposComplete
        //{
        //    get
        //    {
        //       return isBposComplete;
        //    }
        //    set { isBposComplete = value; }
        //}

        //private bool isBposComplete = true;

        private System.Timers.Timer leftTimer;
        //private bool startLeftTimer = false;

        private System.Timers.Timer rightTimer;
        //private bool startRightTimer = false;

        private bool isRegistered = false;
        private static Thread statusThread = null;
        private static bool exitFlag = false;
        //ruige 2019 3 27
       // public static object _lockercurrentStatus = new object();
        public  GloveStatus currentStatus = GloveStatus.Unknown;
        private  bool  isCalibrationBposSucces = true;
        //ruige 2019 6 25
        private double m_WarningTime = 15000;

        internal Hi5_status m_Hi5_StatueData = null;

        /*
        public GloveStatus GetGloveStatus()
        {
            HI5GloveStatus status = HI5_Device.ReadDeviceStatus();
            return ConvertHI5GloveStatus(status);
        }
        */

        /// <summary>
        /// Get the power level of left/right glove.
        /// </summary>
        /// <param name="handType">
        /// The type of <see cref="HI5.Hand"/>.
        /// </param>
        /// <returns>
        /// The related power level of left/right glove.
        /// </returns>
        public PowerLevel GetPowerLevel(Hand handType)
        {
            return handType == Hand.LEFT ? LeftPower : RightPower;
        }

        /// <summary>
        /// Get the magnetic field status of left/right glove.
        /// </summary>
        /// <param name="handType">
        /// The type of <see cref="HI5.Hand"/>.
        /// </param>
        /// <returns>
        /// The related magnetic field status of left/right glove.
        /// </returns>
        public MagneticStatus GetMagneticState(Hand handType)
        {
            return handType == Hand.LEFT ? LeftMagneticStatus : RightMagneticStatus;
        }

        /// <summary>
        /// Check whether left/right glove is available
        /// </summary>
        /// <param name="handType">
        /// The type of <see cref="HI5.Hand"/>.
        /// </param>
        /// <returns>
        /// True, the specific glove is available.
        /// False, the specific glove is inavailable.
        /// </returns>
        public bool IsGloveAvailable(Hand handType)
        {   
            return handType == Hand.LEFT ? IsLeftGloveAvailable : IsRightGloveAvailable;
        }

        /**
         * \cond INTERNAL_USE
         */
        //public void RegisterGloveStatusChangedCallBack()
        //{
        //    if (isRegistered)
        //        return;

        //    if (statusThread != null)
        //    {
        //        statusThread.Abort();
        //        statusThread = null;
        //    }

        //    statusThread = new Thread(new ThreadStart(UpdateThreadState));
        //    exitFlag = false;
        //    statusThread.Start();

        //    isRegistered = true;
        //}

        public void ReleaseGloveStatusChangedCallBack()
        {
            exitFlag = true;

            //if (statusThread != null)
            //{
            //    statusThread.Abort();
            //    statusThread = null;
            //}

            ReleaseBothTimerThread();
            isRegistered = false;
        }



        //internal void OnCallBackBPosResult(BPoseCalibrationErrors erro)
        //{
        //    BposErr = erro;
        //    Debug.Log("SN problem -----HI5_GloveStatus OnCallBackBPosResult()" + HI5_Manager.GetGloveStatus().BposErr.ToString());
        //    int errInt = (int)erro;
        //    if (erro == BPoseCalibrationErrors.BE_CalibratedOK)
        //    {
        //        IsCalibrationBposSuccess = true;
        //        //Hi5_Log.Log("BposSuccess"+ erro);
        //    }
        //    else
        //    {
        //        IsCalibrationBposSuccess = false;
        //       // Hi5_Log.Log("BposFail"+ erro);
        //    }
        //    BposReceiveResult = true;
        //}

        public void OnEventChanged(GloveMod gloveMod, GloveEvent gloveEvent)
        {
            //Debug.Log("glvoe mode = " + gloveMod + " gloveEvent = " + gloveEvent.ToString());
            if (gloveMod == GloveMod.GM_LeftGlove)
            {
                switch (gloveEvent)
                {
                    case GloveEvent.GE_FullPower:
                        LeftPower = PowerLevel.Full;
                        break;
                    case GloveEvent.GE_NormalPower:
                        LeftPower = PowerLevel.Normal;
                        break;
                    case GloveEvent.GE_LowPower:
                        LeftPower = PowerLevel.Low;
                        break;
                    case GloveEvent.GE_Magneticed:
                        leftMS_temp = MagneticStatus.Bad;
                        SetLeftTimer();
                        LeftMagneticStatus = MagneticStatus.Fair;
                        Debug.Log("GloveMod.GM_LeftGlove  GloveEvent.GE_Magneticed");
                        break;
                    case GloveEvent.GE_NotMagneticed:
                        LeftMagneticStatus = MagneticStatus.Good;
                        ReleaseLeftTimerThread();
                        Debug.Log("GloveMod.GM_LeftGlove  GloveEvent.GE_NotMagneticed");
                        //leftTimer.Stop();
                        //leftTimer.Dispose();
                        break;
                }
            }

            if (gloveMod == GloveMod.GM_RightGlove)
            {
                switch (gloveEvent)
                {
                    case GloveEvent.GE_FullPower:
                        RightPower = PowerLevel.Full;
                        break;
                    case GloveEvent.GE_NormalPower:
                        RightPower = PowerLevel.Normal;
                        //Debug.Log("set right power level to " + rightPL.ToString());
                        break;
                    case GloveEvent.GE_LowPower:
                        RightPower = PowerLevel.Low;
                        break;
                    case GloveEvent.GE_Magneticed:
                        rightMS_temp = MagneticStatus.Bad;
                        SetRightTimer();
                        RightMagneticStatus = MagneticStatus.Fair;
                        //Debug.Log(" set rightMS = " + rightMS.ToString());
                        break;
                    case GloveEvent.GE_NotMagneticed:
                        RightMagneticStatus = MagneticStatus.Good;
                        ReleaseRightTimerThread();
                        //rightTimer.Stop();
                        //rightTimer.Dispose();
                        break;
                }
            }
            //ruige 2019 6 25
            //if (gloveMod == GloveMod.GM_BothGloves)
            //{
            //    switch (gloveEvent)
            //    {
            //        case GloveEvent.GE_FullPower:
            //            leftPL = PowerLevel.Full;
            //            rightPL = PowerLevel.Full;
            //            break;
            //        case GloveEvent.GE_NormalPower:
            //            leftPL = PowerLevel.Normal;
            //            rightPL = PowerLevel.Normal;
            //            break;
            //        case GloveEvent.GE_LowPower:
            //            leftPL = PowerLevel.Low;
            //            rightPL = PowerLevel.Low;
            //            break;
            //        case GloveEvent.GE_Magneticed:
            //            leftMS_temp = MagneticStatus.Bad;
            //            rightMS_temp = MagneticStatus.Bad;
            //            SetLeftTimer();
            //            SetRightTimer();
            //            leftMS = MagneticStatus.Fair;
            //            rightMS = MagneticStatus.Fair;
                       
            //            break;
            //        case GloveEvent.GE_NotMagneticed:
            //            leftMS = MagneticStatus.Good;
            //            rightMS = MagneticStatus.Good;
            //            ReleaseLeftTimerThread();
            //            ReleaseRightTimerThread();
            //            //Debug.Log("GloveMod.GM_LeftGlove  GloveEvent.GE_NotMagneticed");
            //            //leftTimer.Stop();
            //            //leftTimer.Dispose();
            //            break;
            //    }
            //}
        }

        /**
         * \endcond
         */
        public void MainThreadUpdate()
        {
            //if (HI5_Manager.modifyThreadSave)
            //{
            //    //lock (_lockercurrentStatus)
            //    {
            //        UpdateGloveStatus(currentStatus);
            //    }
            //}
            //2019 8 2
            if(m_Hi5_StatueData != null && m_Hi5_StatueData.isChange())
            {
                UpdateGloveStatus(m_Hi5_StatueData.GetstatusData());
                
            }
          

        }
        //private void UpdateThreadState()
        //{
        //    //Debug.Log("Start update HI5 status Thread");
        //    while (!exitFlag)
        //    {
        //        if (HI5_Device.IsDongleAvailable())
        //        {
        //            //Debug.Log("HI5_Device.IsDongleAvailable()");
        //            HI5GloveStatus newStatus = HI5_Device.ReadDeviceStatus();
        //            GloveStatus _newSatus = ConvertHI5GloveStatus(newStatus);

        //            if (currentStatus != _newSatus)
        //            {
        //                //没有使用
        //                if (OnStatusChanged != null)
        //                {
        //                    // Call OnStatusChanged() call back function
        //                    OnStatusChanged(_newSatus);

        //                    // Load Calibration Data
        //                    HI5_Calibration.LoadCalibrationData();
        //                }


        //                currentStatus = _newSatus;
        //                //ruige 2019 8 2 //ruige 2019 8 2
        //                Hi5_Statue.WriteStatueData(_newSatus);
        //                UpdateGloveVisible(_newSatus);
        //                //UpdateGloveStatus(currentStatus);

        //            }
        //            else
        //            {
        //                UpdateGloveVisible(currentStatus);
        //            }
        //        }
        //        else
        //        {
        //            IsLeftGloveAvailable = false;
        //            IsRightGloveAvailable = false;
        //        }
        //        Thread.Sleep(20);
        //    }
        //}


        /*状态接受线程都更新*/
        public void UpdateGloveVisible(GloveStatus status)
        {
            if (status == GloveStatus.BothGloveAvailable)
            {
                IsLeftGloveAvailable = true;
                IsRightGloveAvailable = true;
                return;
            }

             if (status == GloveStatus.LeftGloveAvailable)
            {
                IsLeftGloveAvailable = true;
                IsRightGloveAvailable = false;
               
                ReleaseRightTimerThread();
                return;
            }

             if (status == GloveStatus.RightGloveAvailable)
            {
                IsLeftGloveAvailable = false;
                IsRightGloveAvailable = true;
              
                ReleaseLeftTimerThread();
                return;
            }
             //ruige 2019 6 26
            if (status == GloveStatus.NoDongle || status == GloveStatus.NoGlove || status == GloveStatus.Unknown
                || (HI5_Manager_Thread.Instance() != null && (!HI5_Manager_Thread.Instance().IsConected) ))
            {
                IsLeftGloveAvailable = false;
                IsRightGloveAvailable = false;

                //ReleaseBothTimerThread();
                return;
            }
        }
        //更新手套状态
        private void UpdateGloveStatus(GloveStatus status)
        {

            // Load calibration data everytime when glove status changed.
            HI5_Calibration.LoadCalibrationData();

            //ruige bind dev SerialNumber OpticalSensorType
            // Set binded tracked object info everytime when glove status chagned.
            // HI5_Manager.SetBindTrackedObjectInfo();

            //if (HI5_Manager_Thread.Instance() != null)
            //    HI5_Manager_Thread.Instance().SetBindTrackedObjectInfo();


        }

        

        /*
        private void PowerLevelChanged(GloveMod mod, GloveEvent gloveEvent)
        {
            //if (OnPowerLevelChanged == null)
              //  return;


            if (mod == GloveMod.GM_LeftGlove)
            {
                switch (gloveEvent)
                {
                    case GloveEvent.GE_FullPower:
                        //OnPowerLevelChanged(Hand.LEFT, PowerLevel.Full);
                        leftPL = PowerLevel.Full;
                        break;
                    case GloveEvent.GE_NormalPower:
                        //OnPowerLevelChanged(Hand.LEFT, PowerLevel.Normal);
                        leftPL = PowerLevel.Normal;
                        break;
                    case GloveEvent.GE_LowPower:
                        //OnPowerLevelChanged(Hand.LEFT, PowerLevel.Low);
                        leftPL = PowerLevel.Low;
                        break;
                }
            }

            if (mod == GloveMod.GM_RightGlove)
            {
                switch (gloveEvent)
                {
                    case GloveEvent.GE_FullPower:
                        //OnPowerLevelChanged(Hand.RIGHT, PowerLevel.Full);
                        rightPL = PowerLevel.Full;
                        break;
                    case GloveEvent.GE_NormalPower:
                        //OnPowerLevelChanged(Hand.RIGHT, PowerLevel.Normal);
                        rightPL = PowerLevel.Normal;
                        break;
                    case GloveEvent.GE_LowPower:
                        //OnPowerLevelChanged(Hand.RIGHT, PowerLevel.Low);
                        rightPL = PowerLevel.Low;
                        break;
                }
            }
        }

        private void MagneticStateChanged(GloveMod mod, GloveEvent gloveEvent)
        {
            //if (OnMagneticStateChanged == null)
              //  return;

            if (mod == GloveMod.GM_LeftGlove)
            {
                switch (gloveEvent)
                {
                    case GloveEvent.GE_Magneticed:
                        //OnMagneticStateChanged(Hand.LEFT, MagneticStatus.Bad);

                        leftMS_temp = MagneticStatus.Bad;
                        SetLeftTimer();
                        leftMS = MagneticStatus.Fair;
                        break;
                    case GloveEvent.GE_NotMagneticed:
                        //OnMagneticStateChanged(Hand.LEFT, MagneticStatus.Good);
                        leftMS = MagneticStatus.Good;
                        leftTimer.Stop();
                        leftTimer.Dispose();
                        break;
                }
            }

            if (mod == GloveMod.GM_RightGlove)
            {

                switch (gloveEvent)
                {
                    case GloveEvent.GE_Magneticed:
                        //OnMagneticStateChanged(Hand.RIGHT, MagneticStatus.Bad);
                        rightMS_temp = MagneticStatus.Bad;
                        SetRightTimer();
                        rightMS = MagneticStatus.Fair;
                        break;
                    case GloveEvent.GE_NotMagneticed:
                        //OnMagneticStateChanged(Hand.RIGHT, MagneticStatus.Good);
                        rightMS = MagneticStatus.Good;
                        rightTimer.Stop();
                        rightTimer.Dispose();
                        break;
                }
            }
        }
        */

        private void SetLeftTimer()
        {
            // Create a timer with a two second interval.
            leftTimer = new System.Timers.Timer(m_WarningTime);
            // Hook up the Elapsed event for the timer. 
            leftTimer.Elapsed += OnLeftTimedEvent;
            leftTimer.AutoReset = true;
            leftTimer.Enabled = true;
        }

        private void OnLeftTimedEvent(System.Object source, ElapsedEventArgs e)
        {
           // Debug.Log("left timer stoped");
            if (leftMS_temp == MagneticStatus.Bad)
            {
                LeftMagneticStatus = MagneticStatus.Bad;
              //  Debug.Log("left MS turns " + leftMS.ToString());
                //ruige 2019 6 25
                //leftMS_temp = MagneticStatus.Unknown;
            }
            ReleaseLeftTimerThread();
            //leftTimer.Stop();
            //leftTimer.Dispose();
           // Debug.Log("left timer stoped");
        }

        private void SetRightTimer()
        {
            // Create a timer with a two second interval.
            rightTimer = new System.Timers.Timer(m_WarningTime);
            // Hook up the Elapsed event for the timer. 
            rightTimer.Elapsed += OnRightTimedEvent;
            rightTimer.AutoReset = true;
            rightTimer.Enabled = true;
            //startRightTimer = true;
        }

        private void OnRightTimedEvent(System.Object source, ElapsedEventArgs e)
        {
            if (rightMS_temp == MagneticStatus.Bad)
            {
                RightMagneticStatus = MagneticStatus.Bad;
                //Debug.Log("right MS turns " + rightMS.ToString());
            }
            ReleaseRightTimerThread();
        }

        
        internal void ReleaseBothTimerThread()
        {
            ReleaseLeftTimerThread();
            ReleaseRightTimerThread();
        }

        private void ReleaseLeftTimerThread()
        {
            if (leftTimer != null)
            {
                leftTimer.Stop();
                leftTimer.Dispose();
                //Debug.Log("left timer stoped");
            }
        }

        private void ReleaseRightTimerThread()
        {
            if (rightTimer != null)
            {
                rightTimer.Stop();
                rightTimer.Dispose();
                //Debug.Log("right timer stoped");
            }
        }
        
        /*
        private void ResetLeftStatus()
        {
            leftMS = MagneticStatus.Good;
            leftPL = PowerLevel.Full;
        }

        private void ResetRightStatus()
        {
            rightMS = MagneticStatus.Good;
            rightPL = PowerLevel.Full;
        }
        */

        public GloveStatus ConvertHI5GloveStatus(HI5GloveStatus status)
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



    }

}
