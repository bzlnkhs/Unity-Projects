using UnityEngine;
using System;
using System.Runtime.InteropServices;
/**
 * \cond INTERNAL_USE
 */
namespace HI5
{
    #region Data types

    /*
    public enum LeftFingers 
    {
        LeftForeArm             = 0,
        LeftHand                = 1,
        LeftHandThumb1          = 2,
        LeftHandThumb2          = 3,
        LeftHandThumb3          = 4,
        LeftInHandIndex         = 5,
        LeftHandIndex1          = 6,
        LeftHandIndex2          = 7,
        LeftHandIndex3          = 8,
        LeftInHandMiddle        = 9,
        LeftHandMiddle1         = 10,
        LeftHandMiddle2         = 11,
        LeftHandMiddle3         = 12,
        LeftInHandRing          = 13,
        LeftHandRing1           = 14,
        LeftHandRing2           = 15,
        LeftHandRing3           = 16,
        LeftInHandPinky         = 17,
        LeftHandPinky1          = 18,
        LeftHandPinky2          = 19,
        LeftHandPinky3          = 20,

        NumOfLeftBones
    }

    public enum RightFingers
    {
        RightForeArm            = 0,
        RightHand               = 1,
        RightHandThumb1         = 2,
        RightHandThumb2         = 3,
        RightHandThumb3         = 4,
        RightInHandIndex        = 5,
        RightHandIndex1         = 6,
        RightHandIndex2         = 7,
        RightHandIndex3         = 8,
        RightInHandMiddle       = 9,
        RightHandMiddle1        = 10,
        RightHandMiddle2        = 11,
        RightHandMiddle3        = 12,
        RightInHandRing         = 13,
        RightHandRing1          = 14,
        RightHandRing2          = 15,
        RightHandRing3          = 16,
        RightInHandPinky        = 17,
        RightHandPinky1         = 18,
        RightHandPinky2         = 19,
        RightHandPinky3         = 20,

        NumOfRightBones
    }
    */

    public enum GloveMod
    {
        GM_Unknown = -1,
        GM_BothGloves,
        GM_LeftGlove,
        GM_RightGlove,
        GM_MaxNumb,         //  Max number of this enum
    }

    public enum CalibrationPose
    {
        GCP_Unknown = -1,
        GCP_TPose,          // 双手水平站立
        GCP_APose,          // 双手垂直地面站立
        GCP_PPose,          // 大拇指+食指+中指，三指指尖捏合
        GCP_BPose,          // 佛手
        GCP_CPose,          // 云手
        GCP_VPose,          // 纯惯：双手合十向前；光混：双手合十向前，且双手腕夹角成60度
        GCP_MaxNumb,        // 最大校准动作数
    }

    public enum DongleRunningMode
    {
        Running_In_Unknown = -1,
        Running_In_App,
        Running_In_Boot,
        Running_MaxNumb,    //  Max number of this enum
    }

    public  enum WorkingHand
    {
        WH_Unknown = -1,
        WH_Auto,
        WH_SignleHand,
        WH_DoubleHands,
        WH_MaxNumb,             ///< Max number of this enum
    };

    public enum HI5GloveStatus
    {
        DV_Unknown = -1,
        DV_NoDongle,
        DV_NoGlove,
        DV_LeftGloveAvailable,
        DV_RightGloveAvailable,
        DV_BothGloveAvailable,
        DV_MaxNumb,         //  Max number of this enum
    }

    public enum OpticalSensorType
    {
        OST_Unknown = -1,
        OST_HTC_VIVE_Tracker,
        OST_HTC_VIVE_Controller,
        OST_Alice_Rigid_Body,
        OST_Microsoft_MR_Controller,
        OST_MaxNumb,         //  Max number of this enum
    }

    public enum BPoseCalibrationErrors
    {
        BE_NotCalibrated = -1,  // 未校准
        BE_CalibratedOK,        // 成功校准
        BE_NoImuData,           // 无Imu数据
        BE_NoOptiData,          // 无光学数据
        BE_WrongBPoseAction,    // BPose动作幅度太小或光学数据量太少，动作太快或太慢
        BE_BindingFailed,       // 光学传感器与手套绑定失败
        BE_MatchFailed,         // 光学传感器绑定关系与惯性手套模式不匹配
        BE_OptiImuCalcFailed,   // 计算光惯关系失败
        BE_MaxNumb,         //  Max number of this enum
    }

    public enum VibratingLevel
    {
        VL_High =1,    //	强振动
        VL_Middle,  //	中等振动
        VL_Low,     //	弱振动
    };

    enum VibratorTag
    {
        VT_All = 0,     //	所有振子
        VT_1 = 1,       //	1号振子
        VT_2 = 2,       //	2号振子
        VT_3 = 3,       //	3号振子
        VT_4 = 4,       //	4号振子
    };
    

       [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Ver
    {
        int major;
        int minor;
        int rev;
        int build;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DongleInentity
    {
        byte VID;
        byte PID;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        string SerialNumber;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        string DevicePath;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DongleFirmwareVersion
    {
        Ver bootVer;
        Ver appVer;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GloveFirmwareVersion
    {
        GloveMod gloveType;
        Ver bootVer;
        Ver appM0Ver;
        Ver appM4Ver;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TimeStamp
    {
        public double timeStamp;  // ms, from 1970, time stamp when data gathered
        public double latency;    // ms, calculation times
    }

    //[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GloveBVHData
    {
        public GloveMod glove;       // Only 'GM_LeftGlove' or 'GM_RightGlove' in data, means this frame comes from right or left glove
        public TimeStamp timeStamp;  // time stamp
        public int bUpdate;          // true means new data updated by device
        public int bWithDisp;        // whether displacement in bvh
        public int frameIndex;       // frame index
        public int dataCount;        // count of data by float

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 160)]
        public float[] data;      // bvh data array /*160*/

        
        //public GloveBVHData(GloveMod mode, TimeStamp t, int bUpdate, int withDisp, int frameIndex, int dataCount, float[] data)
        //{
        //    this.glove = mode;
        //    this.timeStamp = t;
        //    this.bUpdate = bUpdate;
        //    this.bWithDisp = withDisp;
        //    this.frameIndex = frameIndex;
        //    this.dataCount = dataCount;
        //    this.data = data;
        //}

        public void SetValue(GloveBVHData paramData)
        {
            this.glove = paramData.glove;
            this.timeStamp.timeStamp = paramData.timeStamp.timeStamp;
            this.timeStamp.latency = paramData.timeStamp.latency;
            this.bUpdate = paramData.bUpdate;
            this.bWithDisp = paramData.bWithDisp;
            this.frameIndex = paramData.frameIndex;
            this.dataCount = paramData.dataCount;
            this.data = new float[paramData.data.Length];
            for (int i = 0; i < paramData.data.Length; i++)
            {
                this.data[i] = paramData.data[i];
                
            }
        }
        static public GloveBVHData CreateBVHData(GloveBVHData paramData)
        {
            GloveBVHData glovedata = new GloveBVHData();
            glovedata.glove = paramData.glove;
            glovedata.timeStamp.timeStamp = paramData.timeStamp.timeStamp;
            glovedata.timeStamp.latency = paramData.timeStamp.latency;
            glovedata.bUpdate = paramData.bUpdate;
            glovedata.bWithDisp = paramData.bWithDisp;
            glovedata.frameIndex = paramData.frameIndex;
            glovedata.dataCount = paramData.dataCount;
            glovedata.data = new float[paramData.data.Length];
            for (int i = 0; i < paramData.data.Length; i++)
            {
                glovedata.data[i] = paramData.data[i];
            }
            return glovedata;
        }

        static public GloveBVHData CreateBVHData()
        {
            GloveBVHData glovedata = new GloveBVHData();
            glovedata.glove = GloveMod.GM_Unknown;
            glovedata.timeStamp.timeStamp = 0.0f;
            glovedata.timeStamp.latency = 0.0f;
            glovedata.bUpdate = 1;
            glovedata.bWithDisp = 1;
            glovedata.frameIndex = 1;
            glovedata.dataCount = 0;
            glovedata.data = new float[160];
            for (int i = 0; i < 160; i++)
            {
                glovedata.data[i] =0.0f;
            }
            return glovedata;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CalibrationPercent
    {
        public CalibrationPose pos;     // calibration posture
        public int percent;             // calibration percent
    }

    public enum DongleEvent
    {
        DE_Insert,
        DE_Removed,
    }

    public enum GloveEvent
    {
        GE_Unknown = -1,
       // GE_PowerKeyClicked, // Power键的单击事件（长按、双击等都不会触发）
        GE_LowPower,       // Low power
        GE_NormalPower,    // Normal power
        GE_FullPower,      // Full power
        GE_NotMagneticed,  // 未被磁化
        GE_Magneticed,     // 已被磁化
        GE_RadioFrequencyChanged,   // 通讯射频发生变化
        GE_MaxNumb,        // Max number of this enum
    }

    /*
    public struct BvhDataHeader
    {
        ushort Token1; //!< Package start token: 0xDDFF
        DATA_VER DataVersion; //!< Version of community data format. e.g.: 1.1.0.0
        ushort DataCount; //!< Values count
        uint WithDisp; //!< With/out dispement
        uint WithReference; //!< With/out reference bone data at first
        uint AvatarIndex; //!< Avatar index

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        byte[] AvatarName; //!< Avatar name

        uint FrameIndex; //!< Frame index
        uint Reserved; //!< Reserved, only enable this package has 64bytes length
        uint Reserved1; //!< Reserved, only enable this package has 64bytes length
        uint Reserved2; //!< Reserved, only enable this package has 64bytes length
        ushort Token2; //!< Package end token: 0xEEFF
    }
    */

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SensorModuleStatus
    {
        public uint magneticValue;  // best : 100
        public uint energyValue;    // best : 100
        public uint signalValue;    // best : 100
        public int online;         // 1 : online
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SensorModulesOfDeviceStatus
    {
        public void Create()
        {
            cbSize = (uint)Marshal.SizeOf(typeof(SensorModulesOfDeviceStatus));
            sensorModuleStatus = new SensorModuleStatus[16];
        }              
        public uint cbSize;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public SensorModuleStatus[] sensorModuleStatus;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct  VibrationEffect
    {
        public VibratingLevel vibratingLevel;  //	振动强度等级
        public uint duration;              //	单次振动持续时间
        public uint times;             //	振动次数
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct VibrationParam
    {
        public VibratorTag vibratorTag;        //	振子标签
        public VibrationEffect vibrationEffect;    //	振动效果参数
    };
    #endregion


    #region delegates

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void DongleStateChangedCallback(IntPtr customObject, IntPtr hid, DongleEvent action);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void GloveStateChangedCallback(IntPtr cutomObject, GloveMod gloveMod, GloveEvent gloveEvent);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void BPoseCalibrationResultCallback(IntPtr cutomObject, BPoseCalibrationErrors result);


    #endregion


    #region HI5GLOVE API Wrapper
    internal class HI5_Device
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        private const string ReaderImportor = "Hi5Glove";
#elif UNITY_ANDROID
        private const string ReaderImportor = "libHi5Glove";
#endif
        //private const string ReaderImportor = "HI5Glove";
        //private const string ReaderImportor = "libHI5Glove";


        /*******************************************************/
        /*            Global settings                          */
        /*******************************************************/

        // TODO: need to be tested
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        public static extern void setDongleStateChangedHandle(IntPtr userData, DongleStateChangedCallback handle);

        // TODO: need to be tested
        /// <summary>
        /// ruige glove state change callback
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="handle"></param>
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        public static extern void setGloveStateChangedHandle(IntPtr userData, GloveStateChangedCallback handle);

        // TODO: need to be tested
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        public static extern void setBPoseCalibrationResultHandle(IntPtr userData, BPoseCalibrationResultCallback handle);

        // TODO: need to be tested
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern void restartCapture();
        public static void ReConnect()
        {
            restartCapture();
        }


        /*******************************************************/
        /*           Service start/stop                        */
        /*******************************************************/
        /// <summary>
        /// ruige glove dongle connect
        /// </summary>
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern void hi5DongleStart();
        public static void StartHI5Dongle()
        {
            hi5DongleStart();
        }
#elif UNITY_ANDROID
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern void hi5DongleStart(IntPtr context);
        public static void StartHI5Dongle()
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");
            hi5DongleStart(context.GetRawObject());
        }
#endif

        //ruige GloveStatus HI5_GloveStatus
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern bool isDongleAvailable();
        public static bool IsDongleAvailable()
        {
            return isDongleAvailable();
        }

        /*
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern IntPtr getDongleSN();
        public static string GetDongleSN()
        {
            IntPtr dongleSN = getDongleSN();
            string dongleSN_ = Marshal.PtrToStringAnsi(dongleSN);
            return dongleSN_;
        }
        */

        // TODO: need to be tested, current always get null
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern string getDongleSN();
        public static string GetDongleSerialNumber()
        {
            //string dongleSN = Marshal.PtrToStringAnsi((IntPtr)getDongleSN());
            return getDongleSN();
        }
        //HI5_Manager DisConnect
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern void hi5DongleStop();
        public static void StopHI5Dongle()
        {
            hi5DongleStop();
        }

        /*******************************************************/
        /*           Dongle Settings                           */
        /*******************************************************/
        //HI5_Manager Connect
        // TODO: need to be tested
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern void setReducedOutputDataFreq(UInt32 reduceCount);
        public static void SetReducedOutputDataFreq(int reduceCount)
        {
            setReducedOutputDataFreq((UInt32)reduceCount);
        }

        /*
        // True: fixed, False: free to move
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern void enableGameModeForFingers(bool enable);
        public static void EnableGameModeForFingers(bool enable)
        {
            enableGameModeForFingers(enable);
        }
        */
         
        // True: fixed, False: free to move
        
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern void enableFingerAdbFixed(bool enable);
        public static void EnableFingerAdbFixed(bool enable)
        {
            enableFingerAdbFixed(enable);
        }
        

        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern void setHandThickness(float handThickness);
        public static void SetHandThickness(float handThickness)
        {
            setHandThickness(handThickness);
        }

        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern float getHandThickness();
        public static float GetHandThickness()
        {
            return getHandThickness();
        }

        //HI5_Manager SetDefalutHandOffset
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern void setWristPositionInTrackerFrame(IntPtr leftTrackerOffset/*xyz*/, IntPtr rightTrackerOffset/*xyz*/);
        public static void SetWristPositionInTrackerFrame(float[] leftTrackerOffset, float[] rightTrackerOffset)
        {
            int size = Marshal.SizeOf(typeof(float));
            IntPtr leftTrackerOffset_ptr = Marshal.AllocHGlobal(size * leftTrackerOffset.Length);
            IntPtr rightTrackerOffset_ptr = Marshal.AllocHGlobal(size * rightTrackerOffset.Length);
            Marshal.Copy(leftTrackerOffset, 0, leftTrackerOffset_ptr, leftTrackerOffset.Length);
            Marshal.Copy(rightTrackerOffset, 0, rightTrackerOffset_ptr, rightTrackerOffset.Length);
            setWristPositionInTrackerFrame(leftTrackerOffset_ptr, rightTrackerOffset_ptr);
            Marshal.FreeHGlobal(leftTrackerOffset_ptr);
            Marshal.FreeHGlobal(rightTrackerOffset_ptr);
        }

        // HI5_Manager GetLeftWristPoisitionOffset GetRightWristPoisitionOffset
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern void getWristPositionInTrackerFrame(IntPtr leftTrackerOffset/*xyz*/, IntPtr rightTrackerOffset/*xyz*/);
        public static void GetWristPositionInTrackerFrame(ref float[] leftTrackerOffset, ref float[] rightTrackerOffset)
        {
            int size = Marshal.SizeOf(typeof(float));
            IntPtr leftTrackerOffset_ptr = Marshal.AllocHGlobal(size * leftTrackerOffset.Length);
            IntPtr rightTrackerOffset_ptr = Marshal.AllocHGlobal(size * rightTrackerOffset.Length);
            getWristPositionInTrackerFrame(leftTrackerOffset_ptr, rightTrackerOffset_ptr);
            Marshal.Copy(leftTrackerOffset_ptr, leftTrackerOffset, 0, leftTrackerOffset.Length);
            Marshal.Copy(rightTrackerOffset_ptr, rightTrackerOffset, 0, rightTrackerOffset.Length);
            Marshal.FreeHGlobal(leftTrackerOffset_ptr);
            Marshal.FreeHGlobal(rightTrackerOffset_ptr);
        }

        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern void setBvhToOpt(IntPtr in_q_bvh_to_opt/*sxyz*/);
        public static void SetBvhToOpt(float[] in_q_bvh_to_opt)
        {
            int size = Marshal.SizeOf(typeof(float));
            IntPtr in_q_ptr = Marshal.AllocHGlobal(size * in_q_bvh_to_opt.Length);
            Marshal.Copy(in_q_bvh_to_opt, 0, in_q_ptr, in_q_bvh_to_opt.Length);
            setBvhToOpt(in_q_ptr);
            Marshal.FreeHGlobal(in_q_ptr);
        }

        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern void getBvhToOpt(IntPtr out_q_bvh_to_opt/*sxyz*/);
        public static void GetBvhToOpt(ref float[] out_q_bvh_to_opt)
        {
            int size = Marshal.SizeOf(typeof(float));
            IntPtr out_q_ptr = Marshal.AllocHGlobal(size * out_q_bvh_to_opt.Length);
            getBvhToOpt(out_q_ptr);
            Marshal.Copy(out_q_ptr, out_q_bvh_to_opt, 0, out_q_bvh_to_opt.Length);
            Marshal.FreeHGlobal(out_q_ptr);
        }

        // TODO: need to be tested
        //OpticalSensorType  SerialNumber
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern void setOptiSensorBindState(GloveMod hand, IntPtr trackerIdentity, OpticalSensorType sensorType);
        public static void SetOptiSensorBindState(GloveMod mod, string trackerID, OpticalSensorType sensorType)
        {
            IntPtr strPtr = Marshal.StringToHGlobalAnsi(trackerID);
            setOptiSensorBindState(mod, strPtr, sensorType);
            Marshal.FreeHGlobal(strPtr);
        }

        /*
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern void setOptiSensorBindState(GloveMod hand, IntPtr trackerIdentity);
        public static void SetOptiSensorBindState(GloveMod mod, string trackerID)
        {
            IntPtr strPtr = Marshal.StringToHGlobalAnsi(trackerID);
            setOptiSensorBindState(mod, strPtr);
            Marshal.FreeHGlobal(strPtr);
        }
        */

        // TODO: need to be tested and deleted
        //ruige OpticalSensorType  SerialNumber
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern int getOptiSensorBindState(GloveMod hand, IntPtr buffer, UInt32 buffLen, out OpticalSensorType sensorType);
        public static string GetOptiSensorBindState(GloveMod hand, ref OpticalSensorType sensorType)
        {
            UInt32 assumedLength = 64;
            IntPtr buffer = Marshal.AllocHGlobal((int)assumedLength);
            //OpticalSensorType sensorType = OpticalSensorType.OST_Unknown;

            int realLength = getOptiSensorBindState(hand, buffer, assumedLength, out sensorType);
            if (realLength == 0)
                return null;

            string sn = Marshal.PtrToStringAnsi(buffer);

            Marshal.FreeHGlobal(buffer);
            return sn;
        }

        // TODO: need to be tested 
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern bool getDongleFirmwareVersion(out DongleFirmwareVersion dataBuffer);
        public static DongleFirmwareVersion GetDongleFirmwareVersion(ref DongleFirmwareVersion dataBuffer)
        {
            DongleFirmwareVersion version = new DongleFirmwareVersion();
            getDongleFirmwareVersion(out version);
            return version;
        }

        // TODO: need to be tested 
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern bool getGloveFirmwareVersion(out GloveFirmwareVersion left, out GloveFirmwareVersion right);
        public static void GetGloveFirmwareVersion(ref GloveFirmwareVersion left, ref GloveFirmwareVersion right)
        {
            getGloveFirmwareVersion(out left, out right);
        }

        // TODO: need to be tested 
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern bool getGlovesPowerLevel(out int leftGlove, out int rightGlove);
        public static void GetLeftGlovePowerLevel(ref int left, ref int right)
        {
            getGlovesPowerLevel(out left, out right);
        }

        // TODO: need to be tested 
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern int getCurrentRadioFrequency();
        public static int GetCurrentRadioFrequency()
        {
            return getCurrentRadioFrequency();
        }

        // TODO: need to be tested 
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern DongleRunningMode getDongleRunningMode();
        public static DongleRunningMode GetDongleRunningMode()
        {
            return getDongleRunningMode();
        }

        //HI5_Manager EnableLeftVibration
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern void enableLeftGloveVibrator(int timeSpan/*ms*/);
        public static void EnableLeftGloveVibration(int timeSpan)
        {
            enableLeftGloveVibrator(timeSpan);
        }


        //HI5_Manager EnableRightVibration
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern void enableRightGloveVibrator(int timeSpan/*ms*/);
        public static void EnableRightGloveVibration(int timeSpan)
        {
            enableRightGloveVibrator(timeSpan);
        }

        //HI5_Manager EnableBothGlovesVibration
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern void enableGlovesVibrator(int leftTimeSpan/*ms*/, int rightTimeSpan/*ms*/);
        public static void EnableGlovesVibration(int leftTimeSpan, int rightTimeSpan)
        {
            enableGlovesVibrator(leftTimeSpan, rightTimeSpan);
        }

        // TODO: need to be tested 
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern int getDongleRadioFrequencyTable(IntPtr freqTab /*[32]*/);
        public static int GetDongleRadioFrequencyTable(int[] freqTabArray)
        {
            int size = Marshal.SizeOf(typeof(int));

            IntPtr freqTab = Marshal.AllocHGlobal(size * freqTabArray.Length);

            Marshal.Copy(freqTabArray, 0, freqTab, freqTabArray.Length);

            int result = HI5_Device.getDongleRadioFrequencyTable(freqTab);

            Marshal.FreeHGlobal(freqTab);

            return result;
        }

        // TODO: need to be tested 
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern bool setDongleRadioFrequency(int freq, bool save);
        public static bool SetDongleRadioFrequency(int freq, bool save)
        {
            return setDongleRadioFrequency(freq, save);
        }

        /*******************************************************/
        /*        Optical Data                                 */
        /*******************************************************/

        // TODO: need to be tested
        //ruige HI5_DataTransform.PushOpticalData
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern void pushOpticalSensorData(string sensorIdentity, OpticalSensorType type, IntPtr pos/*xyz*/, IntPtr quat/*sxyz*/);
        public static void PushOpticalSensorData(string sensorIdentity, OpticalSensorType type, float[] fpos, float[] frot)
        {
            int size = Marshal.SizeOf(typeof(float));

            IntPtr pos = Marshal.AllocHGlobal(size * fpos.Length);
            IntPtr rot = Marshal.AllocHGlobal(size * frot.Length);

            Marshal.Copy(fpos, 0, pos, fpos.Length);
            Marshal.Copy(frot, 0, rot, frot.Length);

            HI5_Device.pushOpticalSensorData(sensorIdentity, type, pos, rot);

            Marshal.FreeHGlobal(pos);
            Marshal.FreeHGlobal(rot);
        }

        // TODO: need to be tested
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern void pushOpticalSensorDataWithHandType(GloveMod handType, OpticalSensorType type, IntPtr pos/*xyz*/, IntPtr quat/*sxyz*/);
        public static void PushOpticalSensorDataWithHandType(GloveMod handType, OpticalSensorType type, float[] fpos, float[] frot)
        {
            int size = Marshal.SizeOf(typeof(float));

            IntPtr pos = Marshal.AllocHGlobal(size * fpos.Length);
            IntPtr rot = Marshal.AllocHGlobal(size * frot.Length);

            Marshal.Copy(fpos, 0, pos, fpos.Length);
            Marshal.Copy(frot, 0, rot, frot.Length);

            HI5_Device.pushOpticalSensorDataWithHandType(handType, type, pos, rot);

            Marshal.FreeHGlobal(pos);
            Marshal.FreeHGlobal(rot);
        }

        /*
        // Already deleted, need to be removed
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern void pushTrackerData(bool isLeft, IntPtr pos, IntPtr quat);
        public static void PushTrackerData(bool isLeft, float[] fpos, float[] frot)
        {
            int size = Marshal.SizeOf(typeof(float));

            IntPtr pos = Marshal.AllocHGlobal(size * fpos.Length);
            IntPtr rot = Marshal.AllocHGlobal(size * frot.Length);

            Marshal.Copy(fpos, 0, pos, fpos.Length);
            Marshal.Copy(frot, 0, rot, frot.Length);

            HI5_Device.pushTrackerData(isLeft, pos, rot);

            Marshal.FreeHGlobal(pos);
            Marshal.FreeHGlobal(rot);
        }
        */

        /*******************************************************/
        /*        BVH Data                                     */
        /*******************************************************/
        //ruige glove gesture data BVH 
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern void readBVHData(out GloveBVHData gloveBVHDataBuffer);
        public static void ReadBVHData(ref GloveBVHData dataBuff)
        {
            readBVHData(out dataBuff);
        }


        //ruige read GloveStatus
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern void readDeviceStatus(out HI5GloveStatus deviceStatusBuffer);
        public static HI5GloveStatus ReadDeviceStatus()
        {
            HI5GloveStatus status = HI5GloveStatus.DV_Unknown;
            readDeviceStatus(out status);
            return status;
        }

        /*******************************************************/
        /*        Gloves calibration                           */
        /*******************************************************/
        //标定前准备
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern void calibrationPrepare();
        public static void PrepareCalibration()
        {
            calibrationPrepare();
        }

        //开始标定
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern void startCalibration(CalibrationPose posture);
        public static void StartCalibration(CalibrationPose posture)
        {
            startCalibration(posture);
        }

        //标定进度
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern CalibrationPercent getCalibratingPercent(CalibrationPose pose);
        public static int GetCalibratingPercent(CalibrationPose pose)
        {
            CalibrationPercent cp = getCalibratingPercent(pose);
            int percent = cp.percent;
            return percent;
        }


        // LoadCalibrationData ? glove statue changge Load
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern bool loadCalibrationData(string file_name);
        public static bool LoadCalibrationData(string file_name)
        {
            return loadCalibrationData(file_name);
        }

        //ruige p_pos complete saveCalibrationData
        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern bool saveCalibrationData(string file_name);
        public static bool SaveCalibrationData(string file_name)
        {
            return saveCalibrationData(file_name);
        }

        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern void appendSupportFlags(string support);
        public static void AppendSupportFlags(string support)
        {
            appendSupportFlags(support);
        }

        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern void removeSupportFlags(string support);
        public static void RemoveSupportFlags(string support)
        {
            removeSupportFlags(support);
        }

        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern bool testSupportFlags(string support);
        public static bool TestSupportFlags(string support)
        {
            return testSupportFlags(support);
        }

        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern int readSensorModulesOfDeviceStatus(ref SensorModulesOfDeviceStatus deviceSensorModuleStatus);
        public static int ReadSensorStatus(ref SensorModulesOfDeviceStatus deviceSensorModuleStatus)
        {
            return readSensorModulesOfDeviceStatus(ref deviceSensorModuleStatus);
        }

        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern void forceWorkingHand(WorkingHand workingHand);
        public static void ForceWorkingHand(WorkingHand workingHand)
        {
             forceWorkingHand(workingHand);
        }

        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern void enableFreeVibratorSupport();
        public static void EnableFreeVibratorSupport()
        {
            enableFreeVibratorSupport();
        }

        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern void disableFreeVibratorSupport();
        public static void DisableFreeVibratorSupport()
        {
            disableFreeVibratorSupport();
        }

        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern bool isFreeVibratorSupportEnabled();
        public static bool IsFreeVibratorSupportEnabled()
        {
           return isFreeVibratorSupportEnabled();
            //return false;
        }

        [DllImport(ReaderImportor, CharSet = CharSet.Ansi)]
        private static extern void queuedFreeVibratorVibration(out VibrationParam vibrationParam);
        public static void QueuedFreeVibratorVibration(ref VibrationParam vibrationParam)
        {
             queuedFreeVibratorVibration(out vibrationParam);
        }
       
    }

#endregion
}

/**
 * \endcond
 */
