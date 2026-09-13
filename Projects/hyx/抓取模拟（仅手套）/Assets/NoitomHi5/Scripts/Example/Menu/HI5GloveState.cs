using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HI5.VRCalibration;
namespace HI5
{
    public class HI5GloveState : MonoBehaviour
    {
        public HI5_Magnetic[] magnetics;
        public HI5Signal[] signal;
        public HI5Batterry[] batterys;
        public HI5HandPoint[] points;
        public BatteryInfo _LeftBatteryInfo;
        public BatteryInfo _RightBatteryInfo;
        public MagneticInfo _LeftMagneticInfo;
        public MagneticInfo _RightMagneticInfo;
        private MenuStateMachine m_MenuSM = null;
        public GameObject mBg;
        // Start is called before the first frame update
        void Start()
        {
            m_MenuSM = transform.parent.gameObject.GetComponent<MenuStateMachine>();
        }
        public void SetGloveStateVisible(bool isvisible)
        {
            gameObject.SetActive(isvisible);
            mBg.SetActive(isvisible);
        }
        // Update is called once per frame
        void Update()
        {
            if(m_MenuSM != null && m_MenuSM.State == MenuState.Exit)
            {
                SetGloveStateVisible(false);
            }
            if(HI5_Manager_Thread.Instance() != null)
            {
                for (int i = 0; i < 14; i++)
                {
                    SensorInfor sensorItem = HI5_Manager_Thread.Instance()._sensorInfor.Get(i);
                    if(i < 12)
                         magnetics[i].fresh((int)sensorItem._magneticValue);
                    signal[i].fresh((int)sensorItem._signalValue);
                    batterys[i].fresh((int)sensorItem._energyValue);
                    points[i].SetPointVisible((int)sensorItem._signalValue);
                }
                FreshGloveHandState();
            }
        }

        public bool IsOpenWarning()
        {
            if (HI5_Manager_Thread.Instance() != null)
            {
                uint RightenergyValueTemp = 100;
                uint RightmagneticValueTemp = 100;
                //右手
                for (int i = 0; i < 12; i++)
                {
                    SensorInfor sensorItem = HI5_Manager_Thread.Instance()._sensorInfor.Get(i);
                    if (RightenergyValueTemp > sensorItem._energyValue)
                        RightenergyValueTemp = sensorItem._energyValue;
                    if (RightmagneticValueTemp > sensorItem._magneticValue)
                        RightmagneticValueTemp = sensorItem._magneticValue;
                }
                if (RightenergyValueTemp < 30)
                    return true;
                if (RightmagneticValueTemp < 30)
                    return true;
                uint LeftenergyValueTemp = 100;
                uint LeftmagneticValueTemp = 100;
                //左手

                for (int i = 6; i < 12; i++)
                {
                    SensorInfor sensorItem = HI5_Manager_Thread.Instance()._sensorInfor.Get(i);
                    if (LeftenergyValueTemp > sensorItem._energyValue)
                        LeftenergyValueTemp = sensorItem._energyValue;
                    if (LeftmagneticValueTemp > sensorItem._magneticValue)
                        LeftmagneticValueTemp = sensorItem._magneticValue;
                }
                if (LeftenergyValueTemp < 30)
                    return true;
                if (LeftmagneticValueTemp < 30)
                    return true;
                
            }
            return false;
        }
        public bool IsRembler1Can()
        {
            SensorInfor sensorItem = HI5_Manager_Thread.Instance()._sensorInfor.Get(12);
            if(sensorItem != null)
            {
                if (sensorItem._online == 1)
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public bool IsRembler2Can()
        {
            SensorInfor sensorItem = HI5_Manager_Thread.Instance()._sensorInfor.Get(13);
            if (sensorItem != null)
            {
                if (sensorItem._online == 1)
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
        void FreshGloveHandState()
        {
            if (HI5_Manager_Thread.Instance() != null)
            {
                uint energyValueTemp = 100;
                uint magneticValueTemp = 100;
                //右手
                for (int i = 0; i <6 ; i++)
                {
                    SensorInfor sensorItem = HI5_Manager_Thread.Instance()._sensorInfor.Get(i);
                    if (energyValueTemp > sensorItem._energyValue)
                        energyValueTemp = sensorItem._energyValue;
                    if (magneticValueTemp > sensorItem._magneticValue)
                        magneticValueTemp = sensorItem._magneticValue;                  
                }
                _RightBatteryInfo.Fresh((int)energyValueTemp);
                _RightMagneticInfo.Fresh((int)magneticValueTemp);
                //左手
                energyValueTemp = 100;
                magneticValueTemp = 100;
                for (int i = 6; i < 12; i++)
                {
                    SensorInfor sensorItem = HI5_Manager_Thread.Instance()._sensorInfor.Get(i);
                    if (energyValueTemp > sensorItem._energyValue)
                        energyValueTemp = sensorItem._energyValue;
                    if (magneticValueTemp > sensorItem._magneticValue)
                        magneticValueTemp = sensorItem._magneticValue;
                }
                _LeftBatteryInfo.Fresh((int)energyValueTemp);
                _LeftMagneticInfo.Fresh((int)magneticValueTemp);
            }
        }
    }
}
