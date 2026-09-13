using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HI5
{
    public class HI5_Magnetic : MonoBehaviour
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
            int temp = percent / 25;
            _percent = percent;
            for (int i = 0; i < Points.Length; i++)
            {
                Points[i].SetActive(false);
            }
            if (percent >= 75)
            {
                Points[0].SetActive(true);

            }
            else if (percent >= 50)
            {
                Points[1].SetActive(true);

            }
            else if (percent >= 25)
            {
                Points[2].SetActive(true);
            }
            else
            {
                Points[3].SetActive(true);
            }
        }
    }
}
