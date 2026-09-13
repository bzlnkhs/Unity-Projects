using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HI5
{
    public class HI5_MagneticStatus
    {
        private object _lockLeft = new object();
        public MagneticStatus StatusLeft
        {
            get 
            {
                lock (_lockLeft)
                    return statusLeft;
             }
            set
            {
                lock (_lockLeft)
                    statusLeft = value;
            }
        }
        public MagneticStatus statusLeft = MagneticStatus.Good;
        private object _lockRight = new object();
        public MagneticStatus StatusRight
        {
            get
            {
                lock (_lockRight)
                    return statusRight;
            }
            set
            {
                lock (_lockRight)
                    statusRight = value;
            }
        }
        public MagneticStatus statusRight = MagneticStatus.Good;
        static readonly float cd = 5000.0f;
        private float leftCd = cd;
        private float rightCd = cd;
        public void Update(float detatime)
        {
            //Debug.Log("HI5_MagneticStatus Update");
            if (StatusLeft == MagneticStatus.Fair)
            {
                leftCd -= (detatime*1000.0f);
                
                if (leftCd <= 0.0f)
                {
                    StatusLeft = MagneticStatus.Bad;
                    //Debug.Log(" GloveEvent.MagneticStatus.Bad");
                    leftCd = cd;
                }
            }
            if (StatusRight == MagneticStatus.Fair)
            {
                rightCd -= (detatime * 1000.0f); ;
                if (rightCd <= 0.0f)
                {
                    StatusRight = MagneticStatus.Bad;
                    rightCd = cd;
                }
            }
        }
        public void OnEventChanged(GloveMod gloveMod, GloveEvent gloveEvent)
        {
            //Debug.Log(" GloveEvent.GM_LeftGlove");
            if (gloveMod == GloveMod.GM_LeftGlove)
            {
               // Debug.Log(" GloveEvent.GM_LeftGlove");
                switch (gloveEvent)
                {               
                    case GloveEvent.GE_Magneticed:
                        StatusLeft = MagneticStatus.Fair;
                        leftCd = cd;
                        //Debug.Log(" GloveEvent.GE_Magneticed");
                        break;
                    case GloveEvent.GE_NotMagneticed:
                        StatusLeft = MagneticStatus.Good;               
                        break;
                }
            }

            if (gloveMod == GloveMod.GM_RightGlove)
            {
                switch (gloveEvent)
                {
                   
                    case GloveEvent.GE_Magneticed:
                        StatusRight = MagneticStatus.Fair;
                        rightCd = cd;
                        break;
                    case GloveEvent.GE_NotMagneticed:
                        StatusRight = MagneticStatus.Good;                        
                        break;
                }
            }          
        }

        public void RemoveDongle()
        {
            StatusLeft = MagneticStatus.Good;
            StatusRight = MagneticStatus.Good;
            
        }
    }
}
