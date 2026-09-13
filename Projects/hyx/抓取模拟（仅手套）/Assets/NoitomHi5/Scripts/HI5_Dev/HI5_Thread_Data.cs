using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HI5
{
   // public class HI5_Thread_Data 
    //{
        //传感器信息
        public class  SensorInfor
        {
            internal uint _magneticValue;  // best : 100
            internal uint _energyValue;    // best : 100
            internal uint _signalValue;    // best : 100
            internal int _online;         // 1 : online

            internal void SetInfor(uint magneticValue, uint energyValue, uint signalValue, int online)
            {
                _magneticValue = magneticValue;
                _energyValue = energyValue;
                _signalValue = signalValue;
                _online = online;
            }
        }
        public class SensorInforS
        {
            internal object _lock = new object();
            internal List<SensorInfor> _Sensors = new List<SensorInfor>();
            internal void init()
            {
                for (int i = 0; i < 16; i++)
                {
                    _Sensors.Add(new SensorInfor());
                }
            }

            internal void CleanData()
            {
                //lock (_lock)
                {
                    for (int i = 0; i < _Sensors.Count; i++)
                    {
                        _Sensors[i].SetInfor(0,
                            0,
                            0,
                           0);
                    }
                }
            }
            internal void Set(SensorModulesOfDeviceStatus statu)
            {
                //lock (_lock)
                {
                    for (int i = 0; i < statu.sensorModuleStatus.Length; i++)
                    {
                        _Sensors[i].SetInfor(statu.sensorModuleStatus[i].magneticValue,
                            statu.sensorModuleStatus[i].energyValue,
                            statu.sensorModuleStatus[i].signalValue,
                            statu.sensorModuleStatus[i].online);
                     }
                }
            }
            internal SensorInfor Get(int index)
            {
                SensorInfor temp = new SensorInfor();
                //lock (_lock)
                {
                    temp._magneticValue = _Sensors[index]._magneticValue;
                    temp._energyValue = _Sensors[index]._energyValue;
                    temp._signalValue = _Sensors[index]._signalValue;
                    temp._online = _Sensors[index]._online;
                }
                return temp;
            }
        }
        //光学绑定手腕信息
        public class  OptiSensorBindData
        {
            public static OptiSensorBindData Creat(GloveMod mod, string trackerID, OpticalSensorType sensorType)
            {
                OptiSensorBindData item = new OptiSensorBindData();
                item._mod = mod;
                item._trackerID = trackerID;
                item._sensorType = sensorType;
                return item;
            }
            internal GloveMod _mod;
            internal string _trackerID;
            internal OpticalSensorType _sensorType;
        }
        public class OptiSensorBindDataS
        {
            internal Queue<OptiSensorBindData> _OpticQue = null;
            internal void init()
            {
                _OpticQue = new Queue<OptiSensorBindData>();
            }
            internal void Add(OptiSensorBindData item)
            {

                _OpticQue.Enqueue(item);
            }
            internal OptiSensorBindData Get()
            {
                return _OpticQue.Dequeue();
            }
            internal bool IsHas()
            {
                if (_OpticQue.Count > 0)
                    return true;
                else
                    return false;
            }
            internal void Clean()
            {
                _OpticQue.Clear();
            }
        }       
        //光学数据
        internal class HI5_Optic_Data
        {
            public string _sensorIdentity;
            public OpticalSensorType _type;
            public float[] _fpos;
            public float[] _frot;
            public static HI5_Optic_Data Creat(string sensorIdentity,
                OpticalSensorType type,
                float[] fpos,
                float[] frot)
            {
                HI5_Optic_Data item = new HI5_Optic_Data();
                item._sensorIdentity = sensorIdentity;
                item._type = type;
                item._fpos = new float[fpos.Length];
                for (int i = 0; i < fpos.Length; i++)
                {
                    item._fpos[i] = fpos[i];
                }
                item._frot = new float[frot.Length];
                for (int i = 0; i < frot.Length; i++)
                {
                    item._frot[i] = frot[i];
                }
                return item;
            }
        }
        public class HI5_Optic_DataS
        {
            internal Queue<HI5_Optic_Data> _OpticQue = null;
            internal void init()
            {
                _OpticQue = new Queue<HI5_Optic_Data>();
            }
            internal void Add(HI5_Optic_Data item)
            {

                _OpticQue.Enqueue(item);
            }

            internal HI5_Optic_Data Get()
            {
                return _OpticQue.Dequeue();
            }
            internal bool IsHas()
            {
                if (_OpticQue.Count > 0)
                    return true;
                else
                    return false;
            }

            internal void Clean()
            {
                _OpticQue.Clear();
            }
        }

        //操作命令
        public class HI5_Operate_Command
        {
            internal enum EOperate_Command
            {
                EConnectEquipment = 1,  //连接设备
                ECalibrationOper = 2,   //校准操作
                ESendOpticData = 3,     //传送手光学数据
                EOptiSensorBindData = 4,     //传送手传感器绑定光学数据
                ECloseConnectEquipment = 5, // 断开连接设备
            }

            internal class HI5_Calibration_Send_Data
            {
                internal enum ECalibration_Command
                {
                    EPreCalibration = 1,     //Pre calibration
                    EVCalibration = 2,      //V pos
                    EBCalibration = 3,      //B pos
                    EPCalibration = 4,     //P pos
                }
                Queue<ECalibration_Command> _CalibrationQue = null;
                internal void init()
                {
                    _CalibrationQue = new Queue<ECalibration_Command>();
                }
                internal void Add(ECalibration_Command param)
                {
                    _CalibrationQue.Enqueue(param);
                }

                internal ECalibration_Command Get()
                {
                    return _CalibrationQue.Dequeue();
                }
                internal bool IsHas()
                {
                    if (_CalibrationQue.Count > 0)
                        return true;
                    else
                        return false;
                }

                internal void Clean()
                {
                    _CalibrationQue.Clear();
                }
            }

            internal class HI5_Operate_Data
            {
                public EOperate_Command _Command;
                public System.Object _Data;
            }
            //数据队列
            Dictionary<EOperate_Command, HI5_Operate_Data> _CommandDic = null;
            // HI5_Calibration_Send_Data _CalibrationOper = null;
            //数据锁
            internal object _lock = new object();
            internal void init()
            {
                lock (_lock)
                {
                    _CommandDic = new Dictionary<EOperate_Command, HI5_Operate_Data>();
                }
            }

            public void clean()
            {
                lock (_lock)
                {
                    _CommandDic.Clear();
                    _CommandDic = null;
                }
            }
            //获取连接设备指令
            internal HI5_Operate_Data GetConnectEquipmentCommand()
            {
                HI5_Operate_Data temp = null;
                lock (_lock)
                {
                    if (_CommandDic.ContainsKey(EOperate_Command.EConnectEquipment))
                    {
                        temp = _CommandDic[EOperate_Command.EConnectEquipment];
                        _CommandDic.Remove(EOperate_Command.EConnectEquipment);
                    }
                }
                return temp;
            }
            //获取关闭设备指令
            internal HI5_Operate_Data GetCloseConnectEquipmentCommand()
            {
                HI5_Operate_Data temp = null;
                lock (_lock)
                {
                    if (_CommandDic.ContainsKey(EOperate_Command.ECloseConnectEquipment))
                    {
                        temp = _CommandDic[EOperate_Command.ECloseConnectEquipment];
                        _CommandDic.Remove(EOperate_Command.ECloseConnectEquipment);
                    }
                }
                return temp;
            }

            //获取左手光学数据指令
            internal HI5_Operate_Data GetSendOpticData()
            {
                HI5_Operate_Data temp = null;
                lock (_lock)
                {
                    if (_CommandDic.ContainsKey(EOperate_Command.ESendOpticData))
                    {
                        temp = _CommandDic[EOperate_Command.ESendOpticData];
                        _CommandDic.Remove(EOperate_Command.ESendOpticData);
                    }
                }
                return temp;
            }

            internal HI5_Calibration_Send_Data GetSendCalibration()
            {
                HI5_Calibration_Send_Data Data = null;
                lock (_lock)
                {
                    if (_CommandDic.ContainsKey(EOperate_Command.ECalibrationOper))
                    {
                        HI5_Operate_Data temp = _CommandDic[EOperate_Command.ECalibrationOper];
                        Data = (HI5_Calibration_Send_Data)temp._Data;
                        _CommandDic.Remove(EOperate_Command.ECalibrationOper);
                    }
                }
                return Data;
            }

        internal HI5_Operate_Data GetSenserOpticData()
        {
            HI5_Operate_Data temp = null;
            lock (_lock)
            {
                if (_CommandDic.ContainsKey(EOperate_Command.EOptiSensorBindData))
                {
                    temp = _CommandDic[EOperate_Command.EOptiSensorBindData];
                    _CommandDic.Remove(EOperate_Command.EOptiSensorBindData);
                }
            }
            return temp;
        }
        //写入指令
        //internal void WritOperateData(HI5_Operate_Data param)
        //    {
        //        lock (_lock)
        //        {
        //            if (_CommandDic.ContainsKey(param._Command))
        //            {
        //                _CommandDic.Remove(param._Command);
        //            }
        //            _CommandDic.Add(param._Command, param);
        //        }
        //    }

        //internal void RequestConnect()
        //    {
        //        lock (_lock)
        //        {
        //            if (!_CommandDic.ContainsKey(EOperate_Command.EConnectEquipment))
        //            {
        //                HI5_Operate_Data item = new HI5_Operate_Data();
        //                item._Command = EOperate_Command.EConnectEquipment;
        //                _CommandDic.Add(EOperate_Command.EConnectEquipment, item);
        //            }
        //        }
        //    }
        //internal void RequestCloseConnect()
        //    {
        //        lock (_lock)
        //        {
        //            if (!_CommandDic.ContainsKey(EOperate_Command.ECloseConnectEquipment))
        //            {
        //                HI5_Operate_Data item = new HI5_Operate_Data();
        //                item._Command = EOperate_Command.ECloseConnectEquipment;
        //                _CommandDic.Add(EOperate_Command.ECloseConnectEquipment, item);
        //            }
        //        }
        //    }

        internal void AddOpticData(string sensorIdentity, OpticalSensorType type, float[] fpos, float[] frot)
            {
                lock (_lock)
                {
                    if (_CommandDic.ContainsKey(EOperate_Command.ESendOpticData))
                    {
                        HI5_Operate_Data item = _CommandDic[EOperate_Command.ESendOpticData];
                        HI5_Optic_DataS _SendData = (HI5_Optic_DataS)item._Data;
                        HI5_Optic_Data temp = HI5_Optic_Data.Creat(sensorIdentity, type, fpos, frot);
                        _SendData.Add(temp);
                    }
                    else
                    {
                        HI5_Operate_Data item = new HI5_Operate_Data();
                        item._Command = EOperate_Command.ECalibrationOper;
                        HI5_Optic_DataS _SendData = new HI5_Optic_DataS();
                        _SendData.init();
                        HI5_Optic_Data temp = HI5_Optic_Data.Creat(sensorIdentity, type, fpos, frot);
                        _SendData.Add(temp);
                        item._Data = (System.Object)_SendData;
                        _CommandDic.Add(EOperate_Command.ESendOpticData, item);
                    }
                }
            }
        //HI5_Optic_DataS
        internal void AddOptiSensorBindData(GloveMod mod, string trackerID, OpticalSensorType sensorType)
        {
            lock (_lock)
            {
                if (_CommandDic.ContainsKey(EOperate_Command.EOptiSensorBindData))
                {
                    HI5_Operate_Data item = _CommandDic[EOperate_Command.EOptiSensorBindData];
                    OptiSensorBindDataS _SendData = (OptiSensorBindDataS)item._Data;
                    OptiSensorBindData temp = OptiSensorBindData.Creat(mod, trackerID, sensorType);
                    _SendData.Add(temp);
                }
                else
                {
                      HI5_Operate_Data item = new HI5_Operate_Data();
                      item._Command = EOperate_Command.EOptiSensorBindData;
                      OptiSensorBindDataS _SendData = new OptiSensorBindDataS();
                      _SendData.init();
                      OptiSensorBindData temp = OptiSensorBindData.Creat(mod, trackerID, sensorType);
                      _SendData.Add(temp);
                      item._Data = (System.Object)_SendData;
                     _CommandDic.Add(EOperate_Command.EOptiSensorBindData, item);
                }
            }
        }
        internal void AddCalibrationCommand(HI5_Calibration_Send_Data.ECalibration_Command param)
        {
            lock (_lock)
            {
                if (_CommandDic.ContainsKey(EOperate_Command.ECalibrationOper))
                {
                     HI5_Operate_Data item = _CommandDic[EOperate_Command.ECalibrationOper];
                     HI5_Calibration_Send_Data _SendData = (HI5_Calibration_Send_Data)item._Data;
                     _SendData.Add(param);
                }
                else
                {
                     HI5_Operate_Data item = new HI5_Operate_Data();
                     item._Command = EOperate_Command.ECalibrationOper;
                     HI5_Calibration_Send_Data _SendData = new HI5_Calibration_Send_Data();
                     _SendData.init();
                     _SendData.Add(param);
                     item._Data = (System.Object)_SendData;
                     _CommandDic.Add(EOperate_Command.ECalibrationOper, item);
                 }
             }
         }
        
    }
}
