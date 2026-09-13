using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HI5;
public class HI5_Glove_Calibration_Process_Interface : MonoBehaviour
{
    public enum InertiaTestState
    {
        ENone = 0,
        ECalibrationPre = 1,
        ECalibrationIng = 2,
        CalibrationEnd = 3,
    };
    float mPercent;
    HI5_Pose mPose;
   // public HI5_Calibration_Process_Cd _Cd = null;
    public Text percentUI;
    public InertiaTestState mTestState = InertiaTestState.ENone;
    // Start is called before the first frame update
    void Start()
    {
        mPercent = 0;
        mTestState = InertiaTestState.ENone;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            mTestState = InertiaTestState.ECalibrationPre;
            StartCoroutine(DelayInvokeV());
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            mTestState = InertiaTestState.ECalibrationPre;
            StartCoroutine(DelayInvokeB());
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            mTestState = InertiaTestState.ECalibrationPre;
            StartCoroutine(DelayInvokeP());
        }
    }
    public void StartCalibration(HI5_Pose pose)
    {
        mPercent = 0;
        mPose = pose;
        percentUI.text = mPercent.ToString();
        if (pose == HI5_Pose.VPose)
            HI5_Calibration.ResetCalibration();
        CalibrationPose tranferPose = TransferPoseEnum(pose);
        if (pose == HI5_Pose.BPose && HI5_Manager_Thread.Instance() != null)
            HI5_Manager_Thread.Instance().AddCalibrationCommand(HI5_Operate_Command.HI5_Calibration_Send_Data.ECalibration_Command.EBCalibration);
        if (pose == HI5_Pose.PPose && HI5_Manager_Thread.Instance() != null)
            HI5_Manager_Thread.Instance().AddCalibrationCommand(HI5_Operate_Command.HI5_Calibration_Send_Data.ECalibration_Command.EPCalibration);
        if (pose == HI5_Pose.VPose && HI5_Manager_Thread.Instance() != null)
            HI5_Manager_Thread.Instance().AddCalibrationCommand(HI5_Operate_Command.HI5_Calibration_Send_Data.ECalibration_Command.EVCalibration);
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
            percentUI.text = mPercent.ToString();
            yield return null;
        }
        mPercent = (float)calibrationProgress;
        mTestState = InertiaTestState.CalibrationEnd;
        percentUI.text = mPercent.ToString();
    }

    public float GetCalibrationProgress(HI5_Pose pose)
    {
        return HI5_Calibration.GetCalibrationProgress(mPose);
    }

    IEnumerator DelayInvokeV()
    {
        mTestState = InertiaTestState.ECalibrationPre;
        yield return new WaitForSeconds(1);
        percentUI.text = "3";
        yield return new WaitForSeconds(1);
        percentUI.text = "2";
        yield return new WaitForSeconds(1);
        percentUI.text = "1";
        yield return new WaitForSeconds(1);
        percentUI.text = "0";
        StartCalibration(HI5_Pose.VPose);
        mTestState = InertiaTestState.ECalibrationIng;
    }
    IEnumerator DelayInvokeB()
    {
        mTestState = InertiaTestState.ECalibrationPre;
        yield return new WaitForSeconds(1);
        percentUI.text = "3";
        yield return new WaitForSeconds(1);
        percentUI.text = "2";
        yield return new WaitForSeconds(1);
        percentUI.text = "1";
        yield return new WaitForSeconds(1);
        percentUI.text = "0";
        StartCalibration(HI5_Pose.BPose);
        mTestState = InertiaTestState.ECalibrationIng;
    }

    IEnumerator DelayInvokeP()
    {
        mTestState = InertiaTestState.ECalibrationPre;
        percentUI.text = "3";
        yield return new WaitForSeconds(1);
        percentUI.text = "2";
        yield return new WaitForSeconds(1);
        percentUI.text = "1";
        yield return new WaitForSeconds(1);
        percentUI.text = "0";
        StartCalibration(HI5_Pose.PPose);
        mTestState = InertiaTestState.ECalibrationIng;
    }
}
