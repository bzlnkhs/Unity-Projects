using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HI5;

public class Hi5CalibrationManualSaver : MonoBehaviour
{
    [ContextMenu("Save Hi5 Calibration")]
    public void SaveCalibrationNow()
    {
        bool ok = HI5_Calibration.SaveCalibrationData();
        Debug.Log("[HI5] SaveCalibrationData = " + ok);
        Debug.Log("[HI5] DefaultPath = " + HI5_Calibration.DefaultPath);
        Debug.Log("[HI5] DefaultPathAndName = " + HI5_Calibration.DefaultPathAndName);
    }
}
