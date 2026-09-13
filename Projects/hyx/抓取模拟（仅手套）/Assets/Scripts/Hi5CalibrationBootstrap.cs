using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HI5;

public class Hi5CalibrationBootstrap : MonoBehaviour
{
    private void Start()
    {
        bool ok = HI5_Calibration.LoadCalibrationData();
        Debug.Log("[HI5] LoadCalibrationData = " + ok);
        Debug.Log("[HI5] DefaultPath = " + HI5_Calibration.DefaultPath);
        Debug.Log("[HI5] DefaultPathAndName = " + HI5_Calibration.DefaultPathAndName);
    }
}