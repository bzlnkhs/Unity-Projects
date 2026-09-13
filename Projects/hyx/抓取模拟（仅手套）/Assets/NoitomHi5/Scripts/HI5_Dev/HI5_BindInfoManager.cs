using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;
namespace HI5
{
    /// <summary>
    /// Manage the binded optical device information.
    /// </summary> 
    public static class HI5_BindInfoManager
    {
        /// <summary>
        /// Get or set the ID of tracked device binded on left glove.
        /// </summary>
        public static int LeftID
        {
            get { return leftID; }
            set { leftID = value; }
        }
        private static int leftID = -1;

        /// <summary>
        /// Get or set the ID of tracked device binded on right glove.
        /// </summary>
        public static int RightID
        {
            get { return rightID; }
            set { rightID = value; }
        }
        private static int rightID = -1;



        /// <summary>
        /// Get the bind state of left glove.
        /// True, the left glove is binded on one optical device.
        /// False, the left glove is not binded any optical devcie.
        /// </summary>
        public static bool IsLeftGloveBinded
        {
            get
            {
                if (leftID != -1)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Get the bind state of right glove.
        /// True, the right glove is binded on one optical device.
        /// False, the right glove is not binded any optical devcie.
        /// </summary>
        public static bool IsRightGloveBinded
        {
            get
            {
                if (rightID != -1)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// The instance of Hi5_Bind Info class, saved binded optical device informations.
        /// </summary>
        public static HI5_BindInfo BindInfo = new HI5_BindInfo();

        /// <summary>
        /// Get the default path of saving and reading binded device information file.
        /// </summary>
        public static string DefaultPath
        {
            get { return m_Path; }
        }

        //private static string m_path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/PairInfo.xml";
        private static string m_Path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/HI5";
        //private static string m_Name = "/OpticalDeviceBindInfo.xml";
        private static string m_Name = "OpticalDeviceBindInfo.xml";

        public static string GetFilePath()
        {

            string filePath = "";

#if UNITY_STANDALONE_WIN
            //Debug.Log("UNITY_STANDALONE_WIN");
            //filePath = Application.persistentDataPath + "/HI5";
            filePath = m_Path;

#endif
#if UNITY_ANDROID
        filePath = Application.persistentDataPath + "/HI5";
#endif
#if UNITY_EDITOR
            // Debug.Log("UNITY_EDITOR");
            //filePath = "Assets/Resources/" + "/HI5";
            filePath = m_Path;
#endif

            return filePath;
        }

        private static bool isLoaded = false;
        private static object loadLock = new object();

        /*
        public static bool IsLeftIDAvailable()
        {
            if (leftID != -1)
                return true;
            return false;
        }

        public static bool IsRightIDAvailable()
        {
            if (rightID != -1)
                return true;
            return false;
        }
        */

        /// <summary>
        /// Check the specific glove is binded on any optical device.
        /// </summary>
        /// <param name="handType">
        /// The type of <see cref="HI5.Hand"/>
        /// </param>
        /// <returns>
        /// True, the specific glove is binided.
        /// False, the specific glove is not binided.
        /// </returns>
        public static bool IsGloveBinded(Hand handType)
        {
            return handType == Hand.LEFT ? IsLeftGloveBinded : IsRightGloveBinded;
        }

        /// <summary>
        /// Check whether the device was binded on left or right glove. 
        /// </summary>
        /// <param name="handType">
        /// The type of <see cref="HI5.Hand"/>.
        /// </param>
        /// <param name="serialNumber">
        /// The serial number of the device.
        /// </param>
        /// <returns>
        /// True, the device was binded on specific glove.
        /// False, the device was not binded on specific glove.
        /// </returns>
        public static bool CheckDeviceBinded(Hand handType, string serialNumber)
        {
            if (handType == Hand.LEFT && serialNumber == BindInfo.Left.SerialNumber)
                return true;
            if (handType == Hand.RIGHT && serialNumber == BindInfo.Right.SerialNumber)
                return true;

            return false;
        }

        /// <summary>
        /// Save the binded optical device serail number of both hand locally.
        /// </summary>

        /// b_pos save
        ///
        public static void SaveItems()
        {
            string foldName = GetFilePath();
            if (!CheckFileExists(foldName))
            //if (!CheckDirectoryExists(m_Path))
                CreateDirectory(foldName);

            var xns = new XmlSerializerNamespaces();
            xns.Add(string.Empty, string.Empty);

            string fileName = GetFilePath()+"/" + m_Name;
            XmlSerializer serializer = new XmlSerializer(typeof(HI5_BindInfo));

            FileStream stream = new FileStream(fileName, FileMode.Create);
			StreamWriter streamWriter = new StreamWriter(stream, System.Text.Encoding.UTF8);
            //ruige 2018 11 5
            //Debug.Log("SN problem -----" + "HI5_BindInfoManager SaveItems BindInfo" + "BindInfo.Left.DeviceType" + BindInfo.Left.DeviceType.ToString() + " " + BindInfo.Left.SerialNumber+ "BindInfo.Right.DeviceType" + BindInfo.Right.DeviceType.ToString() + " "+ BindInfo.Right.SerialNumber);
            serializer.Serialize(streamWriter, BindInfo, xns);
			if(streamWriter != null)
				streamWriter.Close();
            stream.Close();

        }


        /*
        public static void SaveItems()
        {
            if (!CheckDirectoryExists(m_Path))
                CreateDirectory(m_Path);

            var xns = new XmlSerializerNamespaces();
            xns.Add(string.Empty, string.Empty);

            XmlSerializer serializer = new XmlSerializer(typeof(HI5_BindInfo));
            FileStream stream = new FileStream((m_Path + m_Name), FileMode.Create);
            serializer.Serialize(stream, BindInfo, xns);
            stream.Close();
        }
        */

        /// <summary>
        /// Load the binded optical device serail number of both hand locally.
        /// </summary>
        /// <returns>
        /// True, successfully load the device serial number.
        /// False, failed load the device serial number.
        /// </returns>

        /*
    public static bool LoadItems()
    {
        lock (loadLock)
        {
            if (isLoaded)
                return true;

            if (!CheckFileExists(m_Path + m_Name))
            {
                //Debug.LogWarning("No Bind Info File Loaded, Please Pair the Steam Tracked Objects on HI5 Glvoe.");
                return false;
            }

            string fileName = m_Path + m_Name;
            XmlSerializer serializer = new XmlSerializer(typeof(HI5_BindInfo));

            FileStream stream = new FileStream(fileName, FileMode.Create);
            var streamReader = new StreamReader(stream, System.Text.Encoding.UTF8);

            BindInfo = serializer.Deserialize(streamReader) as HI5_BindInfo;

            stream.Close();

            isLoaded = true;
            return true;
        }
    }
    */

        //ruige HI5_TrackedDeviceInterface  CheckDeviceBinded  HI5_GloveStatus UpdateGloveStatus HI5_Manager LoadBindTrackedObjectsInfo
        public static bool LoadItems(bool isForce)
        {
            bool IsAndroid = false;
# if UNITY_ANDROID
            IsAndroid = true;
#endif
#if (UNITY_STANDALONE_WIN || UNITY_EDITOR)
            IsAndroid = false;
#endif

        if(IsAndroid)
            return false;
            lock (loadLock)
            {
                if (isLoaded && !isForce)
                    return true;

                string foldName = GetFilePath();
                string fileName = GetFilePath() + "/" + m_Name;
                if (!CheckFileExists(fileName))
                //if (!CheckFileExists(m_Path + m_Name))
                {
                    //Debug.LogWarning("No Bind Info File Loaded, Please Pair the Steam Tracked Objects on HI5 Glvoe.");
                    return false;
                }
                
                XmlSerializer serializer = new XmlSerializer(typeof(HI5_BindInfo));
                FileStream stream = new FileStream(fileName, FileMode.Open);
                HI5_BindInfo temp = serializer.Deserialize(stream) as HI5_BindInfo;
                if ((temp.Left.SerialNumber == null || temp.Left.SerialNumber.Length == 0) &&
                    (temp.Right.SerialNumber == null || temp.Right.SerialNumber.Length == 0))
                {
                    stream.Close();                    
                    isLoaded = true;
                    return false;
                }
                else
                {
                    stream.Close();
                    BindInfo = temp;
                    isLoaded = true;
                    return true;
                }
               
            }
        } 

        /// <summary>
        /// Both binded tracked device information.
        /// </summary>
        internal class DeviceInfo
        {

            private static string leftSerialNumber;

            private static OPTDeviceType leftDeviceType;

            private static string rightSerialNumber;

            private static OPTDeviceType rightDeviceType;

            public static void SetObjectSN(Hand handType, string serialNumber)
            {
                if (handType == Hand.LEFT)
                    leftSerialNumber = serialNumber;

                if (handType == Hand.RIGHT)
                    rightSerialNumber = serialNumber;
            }

            public static string GetObjectSN(Hand handType)
            {
                return handType == Hand.LEFT ? leftSerialNumber : rightSerialNumber;
            }

            public void SetDeviceType(Hand handType, OPTDeviceType deviceType)
            {
                if (handType == Hand.LEFT)
                    leftDeviceType = deviceType;

                if (handType == Hand.RIGHT)
                    rightDeviceType = deviceType;
            }

            public OPTDeviceType GetDeviceType(Hand handType)
            {
                return handType == Hand.LEFT ? leftDeviceType : rightDeviceType;
            }
        }

        /**
         * \cond INTERNAL_USE
         */

        /// <summary>
        /// Both binded tracked device information.
        /// </summary>
        public class HI5_BindInfo
        {
            /// <summary>
            /// The left binded device.
            /// </summary>
            public HI5_BindedObject Left;
            /// <summary>
            /// The right binided device.
            /// </summary>
            public HI5_BindedObject Right;

            /// <summary>
            /// The constructor of HI5_BindInfo.
            /// </summary>
            public HI5_BindInfo()
            {
                Left = new HI5_BindedObject(Hand.LEFT, null, OPTDeviceType.Unknown);
                Right = new HI5_BindedObject(Hand.RIGHT, null, OPTDeviceType.Unknown);
            }

            /// <summary>
            /// Set the binded device serial number into current instance.
            /// </summary>
            /// <param name="handType">
            /// The type of <see cref="HI5.Hand"/>.
            /// </param>
            /// <param name="serialNumber">
            /// The serial number of the binded device. Input by <see cref="System.String"/>.
            /// </param>
            public void SetObjectSN(Hand handType, string serialNumber)
            {
                //ruige 2018 11 5
                //if (handType == Hand.LEFT)
                //{
                //    Debug.Log("SN problem -----" + "HI5_BindInfo SetObjectSN" + "left " + serialNumber);
                //}
                //else
                //{
                //    Debug.Log("SN problem -----" + "HI5_BindInfo SetObjectSN" + "right " + serialNumber);
                //}
                if (handType == Hand.LEFT)
                    Left.SerialNumber = serialNumber;

                if (handType == Hand.RIGHT)
                    Right.SerialNumber = serialNumber;
            }

            /// <summary>
            /// Get the binded device serial number. 
            /// </summary>
            /// <param name="handType">
            /// The type of <see cref="HI5.Hand"/>.
            /// </param>
            /// <returns>
            /// The serial number of this object.
            /// </returns>
            public string GetObjectSN(Hand handType)
            {
                return handType == Hand.LEFT ? Left.SerialNumber : Right.SerialNumber;
            }

            /// <summary>
            /// Set the device type on the specific hand.
            /// </summary>
            /// <param name="handType">
            /// The type of <see cref="HI5.Hand"/>.
            /// </param>
            /// <param name="deviceType">
            /// The type of <see cref="HI5.OPTDeviceType"/>.
            /// </param>
            public void SetDeviceType(Hand handType, OPTDeviceType deviceType)
            {
                if (handType == Hand.LEFT)
                    Left.DeviceType = deviceType;

                if (handType == Hand.RIGHT)
                    Right.DeviceType = deviceType;
                //ruige 2018 11 5
                //if (handType == Hand.LEFT)
                //{
                //    Debug.Log("SN problem -----" + "HI5_BindInfo SetDeviceType" + "left " + deviceType.ToString());
                //}
                //else
                //{
                //    Debug.Log("SN problem -----" + "HI5_BindInfo SetDeviceType" + "right " + deviceType.ToString());
                //}
            }

            /// <summary>
            /// Get the device type on the specific hand.
            /// </summary>
            /// <param name="handType">
            /// The type of <see cref="HI5.Hand"/>.
            /// </param>
            /// <returns>
            /// The type of optical device by <see cref="HI5.OPTDeviceType"/>.
            /// </returns>
            public OPTDeviceType GetDeviceType(Hand handType)
            {
                return handType == Hand.LEFT ? Left.DeviceType : Right.DeviceType;
            }
        }

        /// <summary>
        /// The binded tracked device information.
        /// </summary>
        public struct HI5_BindedObject
        {
            /// <summary>
            /// 
            /// 
            /// 
            /// Get and set on which hand the optical device binded.
            /// </summary>
            public Hand HandType;
            /// <summary>
            /// Get and set the device serial number.
            /// </summary>
            public string SerialNumber;

            /// <summary>
            /// Get and set the optical type of device.
            /// </summary>
            public OPTDeviceType DeviceType;

            /// <summary>
            /// The constructor of HI5_BindedObject.
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
            public HI5_BindedObject(Hand handType, string serialNumber, OPTDeviceType deviceType)
            {
                HandType = handType;
                SerialNumber = serialNumber;
                DeviceType = deviceType;
            }
        }

        /**
         * \endcond
         */


        // system IO part
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

    }


}

