using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HI5
{
    public class HI5HandPoint : MonoBehaviour
    {
        public GameObject[] Points;       
        private int _percent = 0;
        // Start is called before the first frame update
        void Start()
        {
            for(int i=0; i< Points.Length; i++)
            {
                Points[i].SetActive(false);
            }
            Points[0].SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {

        }
        internal void SetPointVisible(int param)
        {
            _percent = param;
            int temp = _percent + 5;
            for (int i = 0; i < Points.Length; i++)
            {
                Points[i].SetActive(false);
            }
            if (temp == 0)
            {
                Points[0].SetActive(true);
            }
            else
            {
                if (temp >= 90)
                {
                    Points[3].SetActive(true);
                }
                else
                {
                    if (temp >= 70)
                    {
                        Points[2].SetActive(true);
                    }
                    else
                    {
                        Points[1].SetActive(true);
                    }
                }
            }
            //Points[param].SetActive(true);
        }
    }
}
