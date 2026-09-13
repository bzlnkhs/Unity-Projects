using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HI5
{
    public class HI5Signal : MonoBehaviour
    {
        public GameObject[] Points;
        // Start is called before the first frame update
        private int _percent = 0;
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
            _percent = percent;
            int temp = _percent + 5;
            for (int i = 0; i < Points.Length; i++)
            {
                Points[i].SetActive(false);
            }
            {
                    if(temp >= 100)
                        Points[10].SetActive(true);
                    else if(temp >= 90)
                        Points[9].SetActive(true);
                    else if (temp >= 80)
                        Points[8].SetActive(true);
                    else if (temp >= 70)
                        Points[7].SetActive(true);
                    else if (temp >= 60)
                        Points[6].SetActive(true);
                    else if (temp >= 50)
                        Points[5].SetActive(true);
                    else if (temp >= 40)
                        Points[4].SetActive(true);
                    else if (temp >= 30)
                        Points[3].SetActive(true);
                    else if (temp >= 20)
                        Points[2].SetActive(true);
                    else if (temp >= 0)
                    {
                        if (_percent == 0)
                            Points[0].SetActive(false); 
                        else
                            Points[1].SetActive(true);
                    }
         
            }

        }
    }
}
