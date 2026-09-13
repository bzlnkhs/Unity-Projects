using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//#if UNITY_STANDALONE_WIN || UNITY_EDITOR

namespace HI5
{
    public class HI5SensorInfor
    {
        public HI5SensorInfor()
        {
            _magneticValue = 0;
            _energyValue = 0;
            _signalValue = 0;
        }
        public int MagneticValue { get { return _magneticValue; } set { _magneticValue = value; } }
        public int EnergyValue { get { return _energyValue; } set { _energyValue = value; } }
        public int SignalValue { get { return _signalValue; } set { _signalValue = value; } }
        internal int _magneticValue;  
        internal int _energyValue;    
        internal int _signalValue;    
    };

    public class HI5_Glove_TransformData_Interface : MonoBehaviour
    {
        public static HI5_Glove_TransformData_Interface Instance = null;
        public enum EHi5_Glove_Sensor
        {
            Hand = 1,
            HandThumb,
            HandIndex,
            HandMiddle,
            HandRing,
            HandPinky
        }
        public enum EHi5_Glove_TransformData_Bones
        {
            /// <summary>
            /// The hand joint.
            /// </summary>
            Hand = 0,
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
        private Dictionary<EHi5_Glove_TransformData_Bones,Transform> LeftHandBones;
        private Dictionary<EHi5_Glove_TransformData_Bones, Transform> RightHandBones; 
        private Dictionary<EHi5_Glove_Sensor, HI5SensorInfor> LeftHandBonesSensorInfor;
        private Dictionary<EHi5_Glove_Sensor, HI5SensorInfor> RightHandBonesSensorInfor;
        public HI5_InertiaInstance mLeftHand;
        public HI5_InertiaInstance mRightHand;
        private Dictionary<int, HI5SensorInfor> VibrateSensorInfor;

        public Dictionary<EHi5_Glove_TransformData_Bones, Transform> GetLeftHandTransform( )
        {
            return LeftHandBones;
        }

        public Dictionary<EHi5_Glove_TransformData_Bones, Transform> GetRightHandTransform()
        {
            return RightHandBones;
        }

        public Dictionary<EHi5_Glove_Sensor, HI5SensorInfor> GetLeftHandSensors()
        {
            return LeftHandBonesSensorInfor;
        }

        public Dictionary<EHi5_Glove_Sensor, HI5SensorInfor> GetRightHandSensors()
        {
            return RightHandBonesSensorInfor;
        }

        public HI5SensorInfor GetRumblerSensorInfor(int RumblerIndex)
        {
            if (RumblerIndex == 1)
                return VibrateSensorInfor[RumblerIndex];
            else if (RumblerIndex == 2)
                return VibrateSensorInfor[RumblerIndex];
            else if (RumblerIndex == 3)
                return VibrateSensorInfor[RumblerIndex];
            else if (RumblerIndex == 4)
                return VibrateSensorInfor[RumblerIndex];
            return null;
        }

        private void Awake()
        {
            HI5_Glove_TransformData_Interface.Instance = this;
            BuildSensors();
        }
         
        private void Update()
        {
            if (LeftHandBones == null && mLeftHand != null)
            {
                LeftHandBones = new Dictionary<EHi5_Glove_TransformData_Bones, Transform>();

                LeftHandBones.Add(EHi5_Glove_TransformData_Bones.Hand, mLeftHand.HandBones[(int)Bones.Hand]);

                LeftHandBones.Add(EHi5_Glove_TransformData_Bones.HandThumb1, mLeftHand.HandBones[(int)Bones.HandThumb1]);
                LeftHandBones.Add(EHi5_Glove_TransformData_Bones.HandThumb2, mLeftHand.HandBones[(int)Bones.HandThumb2]);
                LeftHandBones.Add(EHi5_Glove_TransformData_Bones.HandThumb3, mLeftHand.HandBones[(int)Bones.HandThumb3]);

                LeftHandBones.Add(EHi5_Glove_TransformData_Bones.InHandIndex, mLeftHand.HandBones[(int)Bones.InHandIndex]);
                LeftHandBones.Add(EHi5_Glove_TransformData_Bones.HandIndex1, mLeftHand.HandBones[(int)Bones.HandIndex1]);
                LeftHandBones.Add(EHi5_Glove_TransformData_Bones.HandIndex2, mLeftHand.HandBones[(int)Bones.HandIndex2]);
                LeftHandBones.Add(EHi5_Glove_TransformData_Bones.HandIndex3, mLeftHand.HandBones[(int)Bones.HandIndex3]);

                LeftHandBones.Add(EHi5_Glove_TransformData_Bones.InHandMiddle, mLeftHand.HandBones[(int)Bones.InHandMiddle]);
                LeftHandBones.Add(EHi5_Glove_TransformData_Bones.HandMiddle1, mLeftHand.HandBones[(int)Bones.HandMiddle1]);
                LeftHandBones.Add(EHi5_Glove_TransformData_Bones.HandMiddle2, mLeftHand.HandBones[(int)Bones.HandMiddle2]);
                LeftHandBones.Add(EHi5_Glove_TransformData_Bones.HandMiddle3, mLeftHand.HandBones[(int)Bones.HandMiddle3]);

                LeftHandBones.Add(EHi5_Glove_TransformData_Bones.InHandRing, mLeftHand.HandBones[(int)Bones.InHandRing]);
                LeftHandBones.Add(EHi5_Glove_TransformData_Bones.HandRing1, mLeftHand.HandBones[(int)Bones.HandRing1]);
                LeftHandBones.Add(EHi5_Glove_TransformData_Bones.HandRing2, mLeftHand.HandBones[(int)Bones.HandRing2]);
                LeftHandBones.Add(EHi5_Glove_TransformData_Bones.HandRing3, mLeftHand.HandBones[(int)Bones.HandRing3]);

                LeftHandBones.Add(EHi5_Glove_TransformData_Bones.InHandPinky, mLeftHand.HandBones[(int)Bones.InHandPinky]);
                LeftHandBones.Add(EHi5_Glove_TransformData_Bones.HandPinky1, mLeftHand.HandBones[(int)Bones.HandPinky1]);
                LeftHandBones.Add(EHi5_Glove_TransformData_Bones.HandPinky2, mLeftHand.HandBones[(int)Bones.HandPinky2]);
                LeftHandBones.Add(EHi5_Glove_TransformData_Bones.HandPinky3, mLeftHand.HandBones[(int)Bones.HandPinky3]);

            }
            if (RightHandBones == null && mRightHand != null)
            {
                RightHandBones = new Dictionary<EHi5_Glove_TransformData_Bones, Transform>();
                RightHandBones.Add(EHi5_Glove_TransformData_Bones.Hand, mRightHand.HandBones[(int)Bones.Hand]);

                RightHandBones.Add(EHi5_Glove_TransformData_Bones.HandThumb1, mRightHand.HandBones[(int)Bones.HandThumb1]);
                RightHandBones.Add(EHi5_Glove_TransformData_Bones.HandThumb2, mRightHand.HandBones[(int)Bones.HandThumb2]);
                RightHandBones.Add(EHi5_Glove_TransformData_Bones.HandThumb3, mRightHand.HandBones[(int)Bones.HandThumb3]);

                RightHandBones.Add(EHi5_Glove_TransformData_Bones.InHandIndex, mRightHand.HandBones[(int)Bones.InHandIndex]);
                RightHandBones.Add(EHi5_Glove_TransformData_Bones.HandIndex1, mRightHand.HandBones[(int)Bones.HandIndex1]);
                RightHandBones.Add(EHi5_Glove_TransformData_Bones.HandIndex2, mRightHand.HandBones[(int)Bones.HandIndex2]);
                RightHandBones.Add(EHi5_Glove_TransformData_Bones.HandIndex3, mRightHand.HandBones[(int)Bones.HandIndex3]);

                RightHandBones.Add(EHi5_Glove_TransformData_Bones.InHandMiddle, mRightHand.HandBones[(int)Bones.InHandMiddle]);
                RightHandBones.Add(EHi5_Glove_TransformData_Bones.HandMiddle1, mRightHand.HandBones[(int)Bones.HandMiddle1]);
                RightHandBones.Add(EHi5_Glove_TransformData_Bones.HandMiddle2, mRightHand.HandBones[(int)Bones.HandMiddle2]);
                RightHandBones.Add(EHi5_Glove_TransformData_Bones.HandMiddle3, mRightHand.HandBones[(int)Bones.HandMiddle3]);

                RightHandBones.Add(EHi5_Glove_TransformData_Bones.InHandRing, mRightHand.HandBones[(int)Bones.InHandRing]);
                RightHandBones.Add(EHi5_Glove_TransformData_Bones.HandRing1, mRightHand.HandBones[(int)Bones.HandRing1]);
                RightHandBones.Add(EHi5_Glove_TransformData_Bones.HandRing2, mRightHand.HandBones[(int)Bones.HandRing2]);
                RightHandBones.Add(EHi5_Glove_TransformData_Bones.HandRing3, mRightHand.HandBones[(int)Bones.HandRing3]);

                RightHandBones.Add(EHi5_Glove_TransformData_Bones.InHandPinky, mRightHand.HandBones[(int)Bones.InHandPinky]);
                RightHandBones.Add(EHi5_Glove_TransformData_Bones.HandPinky1, mRightHand.HandBones[(int)Bones.HandPinky1]);
                RightHandBones.Add(EHi5_Glove_TransformData_Bones.HandPinky2, mRightHand.HandBones[(int)Bones.HandPinky2]);
                RightHandBones.Add(EHi5_Glove_TransformData_Bones.HandPinky3, mRightHand.HandBones[(int)Bones.HandPinky3]);

            }
            SetSensorData();
        }

        private void SetTransformData(HI5_InertiaInstance handOriginal,
                                        List<Transform> hands,
                                        Bones boneOriginalType,
                                        EHi5_Glove_TransformData_Bones boneType)
        {
            if (hands[(int)boneType] == null)
                Debug.Log("boneType"+ (int)boneType);
            if (handOriginal.HandBones[(int)boneOriginalType] == null)
                Debug.Log("boneOriginalType" + (int)boneOriginalType);
            hands[(int)boneType].position = handOriginal.HandBones[(int)boneOriginalType].position;
            hands[(int)boneType].rotation = handOriginal.HandBones[(int)boneOriginalType].rotation;
        }

        private void BuildSensors()
        {
            LeftHandBonesSensorInfor = new Dictionary<EHi5_Glove_Sensor, HI5SensorInfor>();
            LeftHandBonesSensorInfor.Add(EHi5_Glove_Sensor.Hand, new HI5SensorInfor());
            LeftHandBonesSensorInfor.Add(EHi5_Glove_Sensor.HandThumb, new HI5SensorInfor());
            LeftHandBonesSensorInfor.Add(EHi5_Glove_Sensor.HandIndex, new HI5SensorInfor());
            LeftHandBonesSensorInfor.Add(EHi5_Glove_Sensor.HandMiddle, new HI5SensorInfor());
            LeftHandBonesSensorInfor.Add(EHi5_Glove_Sensor.HandRing, new HI5SensorInfor());
            LeftHandBonesSensorInfor.Add(EHi5_Glove_Sensor.HandPinky, new HI5SensorInfor());

            RightHandBonesSensorInfor = new Dictionary<EHi5_Glove_Sensor, HI5SensorInfor>();
            RightHandBonesSensorInfor.Add(EHi5_Glove_Sensor.Hand, new HI5SensorInfor());
            RightHandBonesSensorInfor.Add(EHi5_Glove_Sensor.HandThumb, new HI5SensorInfor());
            RightHandBonesSensorInfor.Add(EHi5_Glove_Sensor.HandIndex, new HI5SensorInfor());
            RightHandBonesSensorInfor.Add(EHi5_Glove_Sensor.HandMiddle, new HI5SensorInfor());
            RightHandBonesSensorInfor.Add(EHi5_Glove_Sensor.HandRing, new HI5SensorInfor());
            RightHandBonesSensorInfor.Add(EHi5_Glove_Sensor.HandPinky, new HI5SensorInfor());

            VibrateSensorInfor = new Dictionary<int, HI5SensorInfor>();
            VibrateSensorInfor.Add(1, new HI5SensorInfor());
            VibrateSensorInfor.Add(2, new HI5SensorInfor());
            VibrateSensorInfor.Add(3, new HI5SensorInfor());
            VibrateSensorInfor.Add(4, new HI5SensorInfor());
        }


        private void SetSensorData()
        {
            if (HI5_Manager_Thread.Instance() != null)
            {
                //右手
                {
                    SensorInfor sensorItem = HI5_Manager_Thread.Instance()._sensorInfor.Get(0);
                    if(sensorItem != null)
                    {
                        RightHandBonesSensorInfor[EHi5_Glove_Sensor.Hand].EnergyValue = (int)sensorItem._energyValue;
                        RightHandBonesSensorInfor[EHi5_Glove_Sensor.Hand].MagneticValue = (int)sensorItem._magneticValue;
                        RightHandBonesSensorInfor[EHi5_Glove_Sensor.Hand].SignalValue = (int)sensorItem._signalValue;
                    }
                    
                }

                {
                    SensorInfor sensorItem = HI5_Manager_Thread.Instance()._sensorInfor.Get(1);
                    if (sensorItem != null)
                    {
                        RightHandBonesSensorInfor[EHi5_Glove_Sensor.HandThumb].EnergyValue = (int)sensorItem._energyValue;
                        RightHandBonesSensorInfor[EHi5_Glove_Sensor.HandThumb].MagneticValue = (int)sensorItem._magneticValue;
                        RightHandBonesSensorInfor[EHi5_Glove_Sensor.HandThumb].SignalValue = (int)sensorItem._signalValue;
                    }
                }

                {
                    SensorInfor sensorItem = HI5_Manager_Thread.Instance()._sensorInfor.Get(2);
                    if (sensorItem != null)
                    {
                        RightHandBonesSensorInfor[EHi5_Glove_Sensor.HandIndex].EnergyValue = (int)sensorItem._energyValue;
                        RightHandBonesSensorInfor[EHi5_Glove_Sensor.HandIndex].MagneticValue = (int)sensorItem._magneticValue;
                        RightHandBonesSensorInfor[EHi5_Glove_Sensor.HandIndex].SignalValue = (int)sensorItem._signalValue;
                    }
                }

                {
                    SensorInfor sensorItem = HI5_Manager_Thread.Instance()._sensorInfor.Get(3);
                    if (sensorItem != null)
                    {
                        RightHandBonesSensorInfor[EHi5_Glove_Sensor.HandMiddle].EnergyValue = (int)sensorItem._energyValue;
                        RightHandBonesSensorInfor[EHi5_Glove_Sensor.HandMiddle].MagneticValue = (int)sensorItem._magneticValue;
                        RightHandBonesSensorInfor[EHi5_Glove_Sensor.HandMiddle].SignalValue = (int)sensorItem._signalValue;
                    }
                }

                {
                    SensorInfor sensorItem = HI5_Manager_Thread.Instance()._sensorInfor.Get(4);
                    if (sensorItem != null)
                    {
                        RightHandBonesSensorInfor[EHi5_Glove_Sensor.HandRing].EnergyValue = (int)sensorItem._energyValue;
                        RightHandBonesSensorInfor[EHi5_Glove_Sensor.HandRing].MagneticValue = (int)sensorItem._magneticValue;
                        RightHandBonesSensorInfor[EHi5_Glove_Sensor.HandRing].SignalValue = (int)sensorItem._signalValue;
                    }
                }

                {
                    SensorInfor sensorItem = HI5_Manager_Thread.Instance()._sensorInfor.Get(5);
                    if (sensorItem != null)
                    {
                        RightHandBonesSensorInfor[EHi5_Glove_Sensor.HandPinky].EnergyValue = (int)sensorItem._energyValue;
                        RightHandBonesSensorInfor[EHi5_Glove_Sensor.HandPinky].MagneticValue = (int)sensorItem._magneticValue;
                        RightHandBonesSensorInfor[EHi5_Glove_Sensor.HandPinky].SignalValue = (int)sensorItem._signalValue;
                    }
                }

                //左手
                {
                    SensorInfor sensorItem = HI5_Manager_Thread.Instance()._sensorInfor.Get(6);
                    if (sensorItem != null)
                    {
                        LeftHandBonesSensorInfor[EHi5_Glove_Sensor.Hand].EnergyValue = (int)sensorItem._energyValue;
                        LeftHandBonesSensorInfor[EHi5_Glove_Sensor.Hand].MagneticValue = (int)sensorItem._magneticValue;
                        LeftHandBonesSensorInfor[EHi5_Glove_Sensor.Hand].SignalValue = (int)sensorItem._signalValue;
                    }
                }

                {
                    SensorInfor sensorItem = HI5_Manager_Thread.Instance()._sensorInfor.Get(7);
                    if (sensorItem != null)
                    {
                        LeftHandBonesSensorInfor[EHi5_Glove_Sensor.HandThumb].EnergyValue = (int)sensorItem._energyValue;
                        LeftHandBonesSensorInfor[EHi5_Glove_Sensor.HandThumb].MagneticValue = (int)sensorItem._magneticValue;
                        LeftHandBonesSensorInfor[EHi5_Glove_Sensor.HandThumb].SignalValue = (int)sensorItem._signalValue;
                    }
                }

                {
                    SensorInfor sensorItem = HI5_Manager_Thread.Instance()._sensorInfor.Get(8);
                    if (sensorItem != null)
                    {
                        LeftHandBonesSensorInfor[EHi5_Glove_Sensor.HandIndex].EnergyValue = (int)sensorItem._energyValue;
                        LeftHandBonesSensorInfor[EHi5_Glove_Sensor.HandIndex].MagneticValue = (int)sensorItem._magneticValue;
                        LeftHandBonesSensorInfor[EHi5_Glove_Sensor.HandIndex].SignalValue = (int)sensorItem._signalValue;
                    }
                }

                {
                    SensorInfor sensorItem = HI5_Manager_Thread.Instance()._sensorInfor.Get(9);
                    if (sensorItem != null)
                    {
                        LeftHandBonesSensorInfor[EHi5_Glove_Sensor.HandMiddle].EnergyValue = (int)sensorItem._energyValue;
                        LeftHandBonesSensorInfor[EHi5_Glove_Sensor.HandMiddle].MagneticValue = (int)sensorItem._magneticValue;
                        LeftHandBonesSensorInfor[EHi5_Glove_Sensor.HandMiddle].SignalValue = (int)sensorItem._signalValue;
                    }
                }

                {
                    SensorInfor sensorItem = HI5_Manager_Thread.Instance()._sensorInfor.Get(10);
                    if (sensorItem != null)
                    {
                        LeftHandBonesSensorInfor[EHi5_Glove_Sensor.HandRing].EnergyValue = (int)sensorItem._energyValue;
                        LeftHandBonesSensorInfor[EHi5_Glove_Sensor.HandRing].MagneticValue = (int)sensorItem._magneticValue;
                        LeftHandBonesSensorInfor[EHi5_Glove_Sensor.HandRing].SignalValue = (int)sensorItem._signalValue;
                    }
                }

                {
                    SensorInfor sensorItem = HI5_Manager_Thread.Instance()._sensorInfor.Get(11);
                    if (sensorItem != null)
                    {
                        LeftHandBonesSensorInfor[EHi5_Glove_Sensor.HandPinky].EnergyValue = (int)sensorItem._energyValue;
                        LeftHandBonesSensorInfor[EHi5_Glove_Sensor.HandPinky].MagneticValue = (int)sensorItem._magneticValue;
                        LeftHandBonesSensorInfor[EHi5_Glove_Sensor.HandPinky].SignalValue = (int)sensorItem._signalValue;
                    }
                }

                //振子1
                {
                    SensorInfor sensorItem = HI5_Manager_Thread.Instance()._sensorInfor.Get(12);
                    if (sensorItem != null)
                    {
                        VibrateSensorInfor[1].EnergyValue = (int)sensorItem._energyValue;
                        VibrateSensorInfor[1].MagneticValue = (int)sensorItem._magneticValue;
                        VibrateSensorInfor[1].SignalValue = (int)sensorItem._signalValue;
                    }
                        
                }
                //振子2
                {
                    SensorInfor sensorItem = HI5_Manager_Thread.Instance()._sensorInfor.Get(13);
                    if (sensorItem != null)
                    {
                        VibrateSensorInfor[2].EnergyValue = (int)sensorItem._energyValue;
                        VibrateSensorInfor[2].MagneticValue = (int)sensorItem._magneticValue;
                        VibrateSensorInfor[2].SignalValue = (int)sensorItem._signalValue;
                    }
                }
                //振子3
                {
                    SensorInfor sensorItem = HI5_Manager_Thread.Instance()._sensorInfor.Get(14);
                    if (sensorItem != null)
                    {
                        VibrateSensorInfor[3].EnergyValue = (int)sensorItem._energyValue;
                        VibrateSensorInfor[3].MagneticValue = (int)sensorItem._magneticValue;
                        VibrateSensorInfor[3].SignalValue = (int)sensorItem._signalValue;
                    }
                }
                //振子4
                {
                    SensorInfor sensorItem = HI5_Manager_Thread.Instance()._sensorInfor.Get(15);
                    if (sensorItem != null)
                    {
                        VibrateSensorInfor[4].EnergyValue = (int)sensorItem._energyValue;
                        VibrateSensorInfor[4].MagneticValue = (int)sensorItem._magneticValue;
                        VibrateSensorInfor[4].SignalValue = (int)sensorItem._signalValue;
                    }

                }
            }
        }

       public void Vibrate(int rumblerIndex,
            int vibrateLevel,
            uint duration,
            uint times)
        {
            if (HI5_Manager_Thread.Instance() != null)
                HI5_Manager_Thread.Instance().Vibrate(rumblerIndex, vibrateLevel, duration, times);
        }
    }
}

//#endif