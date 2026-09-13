using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HI5
{
    public class HI5Batterry : MonoBehaviour
    {
        public GameObject[] Points;
        // Start is called before the first frame update
        private int _perecent = 0;
        void Start()
        {
            for (int i = 0; i < Points.Length; i++)
            {
                Points[i].SetActive(false);
            }
            Points[0].SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {

        }

        internal void fresh(int percent)
        {
            int temp = percent / 25;
            _perecent = percent;
            for (int i = 0; i < Points.Length; i++)
            {
                Points[i].SetActive(false);
            }
            if(temp<=4)
            {
                
                if(temp == 0)
                {
                    if(percent == 0)
                    {
                        Points[0].SetActive(true);
                    }
                    else
                    {
                        Points[1].SetActive(true);
                    }
                }
                else
                {
                    Points[temp].SetActive(true);
                }
            }
                
        }
    }
}
