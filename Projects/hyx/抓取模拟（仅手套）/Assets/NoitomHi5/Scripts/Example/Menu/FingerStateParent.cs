using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HI5;
public class FingerStateParent : MonoBehaviour
{
    public FingerStateScript mFingerState;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(mFingerState != null)
        {
            if(HI5_Manager_Thread.Instance() != null)
            {
                if(!HI5_Manager_Thread.Instance().IsHaveLoadFile)
                {
                    mFingerState.isHaveLoadFile = false;
                }
                else
                {
                    mFingerState.isHaveLoadFile = true;
                }
            }
        }
    }

    
}
