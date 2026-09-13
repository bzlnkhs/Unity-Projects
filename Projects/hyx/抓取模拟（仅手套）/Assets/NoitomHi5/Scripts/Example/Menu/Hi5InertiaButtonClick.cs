using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HI5.VRInteraction
{
    public class Hi5InertiaButtonClick : MonoBehaviour
    {
        // Start is called before the first frame update
        public enum InertiaState
        {
            ENone = 0,
            ECalibrationPre = 1,
            ECalibrationIng = 2,
            CalibrationEnd = 3,
        };
        float mPercent;
        HI5_Pose mPose;
    
        public InertiaState mTestState = InertiaState.ENone;
        // Start is called before the first frame update
        void Start()
        {
            mPercent = 0;
            mTestState = InertiaState.ENone;
        }

        // Update is called once per frame
        //void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.V))
        //    {
        //        mTestState = InertiaTestState.ECalibrationPre;
        //        StartCoroutine(DelayInvokeV());
        //    }
        //    if (Input.GetKeyDown(KeyCode.B))
        //    {
        //        mTestState = InertiaTestState.ECalibrationPre;
        //        StartCoroutine(DelayInvokeB());
        //    }
        //    if (Input.GetKeyDown(KeyCode.P))
        //    {
        //        mTestState = InertiaTestState.ECalibrationPre;
        //        StartCoroutine(DelayInvokeP());
        //    }
        //}
        public void StartCalibration(HI5_Pose pose)
        {
            //if (pose == HI5_Pose.BPose)
            //    isCalibratingBPose = true;

            //if (pose == HI5_Pose.PPose)
            //    isCalibratingPPose = true;
            mPercent = 0;
        
            mPose = pose;
            if (pose == HI5_Pose.VPose)
                HI5_Calibration.ResetCalibration();

            CalibrationPose tranferPose = TransferPoseEnum(pose);

            if (pose == HI5_Pose.BPose && HI5_Manager_Thread.Instance() != null)
                HI5_Manager_Thread.Instance().AddCalibrationCommand(HI5_Operate_Command.HI5_Calibration_Send_Data.ECalibration_Command.EBCalibration);

            if (pose == HI5_Pose.PPose && HI5_Manager_Thread.Instance() != null)
                HI5_Manager_Thread.Instance().AddCalibrationCommand(HI5_Operate_Command.HI5_Calibration_Send_Data.ECalibration_Command.EPCalibration);

            if (pose == HI5_Pose.VPose && HI5_Manager_Thread.Instance() != null)
                HI5_Manager_Thread.Instance().AddCalibrationCommand(HI5_Operate_Command.HI5_Calibration_Send_Data.ECalibration_Command.EVCalibration);
            // HI5_Device.StartCalibration(tranferPose);
            StartCoroutine(UpdateCalibrationProgress());
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

        IEnumerator UpdateCalibrationProgress()
        {
            int calibrationProgress = 0;
            while (calibrationProgress < 100)
            {
                calibrationProgress = HI5_Calibration.GetCalibrationProgress(mPose);
                mPercent = (float)calibrationProgress;
                
                yield return null;
            }
            mPercent = (float)calibrationProgress;
            mTestState = InertiaState.CalibrationEnd;
           
        }
        IEnumerator DelayInvokeV()
        {
            mTestState = InertiaState.ECalibrationPre;
           
            yield return new WaitForSeconds(1);
            
            yield return new WaitForSeconds(1);
           
            yield return new WaitForSeconds(1);
            StartCalibration(HI5_Pose.VPose);
            mTestState = InertiaState.ECalibrationIng;

        }
        IEnumerator DelayInvokeB()
        {
            mTestState = InertiaState.ECalibrationPre;
           
            yield return new WaitForSeconds(1);
           
            yield return new WaitForSeconds(1);
           
            yield return new WaitForSeconds(1);
            StartCalibration(HI5_Pose.BPose);
            mTestState = InertiaState.ECalibrationIng;
        }
        IEnumerator DelayInvokeP()
        {
            mTestState = InertiaState.ECalibrationPre;
          
            yield return new WaitForSeconds(1);
            
            yield return new WaitForSeconds(1);
           
            yield return new WaitForSeconds(1);
            StartCalibration(HI5_Pose.PPose);
            mTestState = InertiaState.ECalibrationIng;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //Ray ray01 = new Ray(Camera.main.transform.position, Vector3.forward);
                RaycastHit hit;
                //判断是否碰撞到物体
                bool isCollider = Physics.Raycast(ray, out hit);
                //bool isCollider01= Physics.Raycast(Camera.main.transform.position, Vector3.forward, 
                //    10, LayerMask.GetMask("UI", "Enemy", "Player"));
                if (isCollider && hit.collider.gameObject != null)
                {
                    if(hit.collider.gameObject.GetComponent<VRButton>() != null)
                    {
                        hit.collider.gameObject.GetComponent<VRButton>().Rayscreen();
                        //Debug.Log(hit.collider.gameObject.name);
                    }
                    //Debug.Log(hit.collider.gameObject.name);
                    //print();
                }
            }
        }
    }
}

