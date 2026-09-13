using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HI5;

public class Hi5CalibrationHotkeySaver : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            bool ok = HI5_Calibration.SaveCalibrationData();
            Debug.Log("[HI5] SaveCalibrationData = " + ok);
            Debug.Log("[HI5] DefaultPath = " + HI5_Calibration.DefaultPath);
            Debug.Log("[HI5] DefaultPathAndName = " + HI5_Calibration.DefaultPathAndName);
        }
    }
}
