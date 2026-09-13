using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HI5
{

    //线程数据多线程操作
    public class Hi5_Data_Tread
    {
        public  object _lockerGloveAvailable = new object();
        //更新状态线程更新  主线程查询
        private bool isLeftGloveAvailable = false;
        private bool isRightGloveAvailable = false;
        public  bool  LeftGloveAvailable
        {
            get 
            {
                lock (_lockerGloveAvailable)
                {
                    return isLeftGloveAvailable;
                }
            }
           
        }
        public bool RightGloveAvailable
        {
            get
            {
                lock (_lockerGloveAvailable)
                {
                    return isRightGloveAvailable;
                }
            }

        }

        public void SetGloveAvailable(bool leftGloveAvailable, bool rightGloveAvailable)
        {
            lock (_lockerGloveAvailable)
            {
                isLeftGloveAvailable = leftGloveAvailable;
                isRightGloveAvailable = rightGloveAvailable;
            }
        }
    }
}

