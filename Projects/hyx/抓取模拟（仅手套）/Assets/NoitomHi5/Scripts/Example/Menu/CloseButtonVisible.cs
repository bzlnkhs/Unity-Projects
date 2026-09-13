using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HI5;
public class CloseButtonVisible : MonoBehaviour
{
     public bool isVisible = false;
    public GameObject _CloseButton = null;
    // Start is called before the first frame update
    void Awake()
    {
        isVisible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (HI5_Manager_Thread.Instance().IsCalibrationComplete)
        {
            isVisible = true;
        }
         _CloseButton.SetActive(isVisible);
    }
}
