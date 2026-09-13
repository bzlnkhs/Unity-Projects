using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HI5
{
    public class HI5_Calibration_Process_Cd : MonoBehaviour
    {
        public GameObject[] cd;
        // Start is called before the first frame update
        public void SetCd(int Index,bool Isvisible)
        {           
            for(int i=0; i< cd.Length; i++)
            {
                cd[i].SetActive(false);
            }
            if(Isvisible)
            {
                Index--;
                if (Index == 3)
                {
                    cd[0].SetActive(true);
                }
                else if (Index == 2)
                {
                    cd[1].SetActive(true);
                }
                else if (Index == 1)
                {
                    cd[2].SetActive(true);
                }
            }
            
        }
    }
}

