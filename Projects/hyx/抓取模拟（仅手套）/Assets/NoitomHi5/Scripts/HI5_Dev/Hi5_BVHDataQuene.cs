using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
namespace HI5
{
    public class Hi5_status
    {
         internal object _lock = new object();
         internal GloveStatus status;
         private bool ischange =false;
         public void WriteStatueData(GloveStatus statue )
        {
            //lock (_lock)
            //{
                status = statue;
                ischange = true;
           // }
        }

         public bool isChange()
        {
           // lock (_lock)
            //{
                return ischange;
            //}
        }
         public GloveStatus GetstatusData()
        {
            //lock (_lock)
            //{
                
                ischange = false;
                return status;
           // }
        }
    }
    public class Hi5_BVHDataQuene
    {
         private object _lockRight = new object();
        private object _lockLeft = new object();
        Queue<GloveBVHData> _queueBvh = new Queue<GloveBVHData>();
         Queue<GloveBVHData> _countQueueBvh = new Queue<GloveBVHData>();

        GloveBVHData _BvhRight = GloveBVHData.CreateBVHData();
        GloveBVHData _BvhLeft = GloveBVHData.CreateBVHData();

        bool IsLeftUpdata = false;
        bool IsRightUpdata = false;

        public bool  GetBvhtemLeft(ref GloveBVHData bvhdata)
        {
            bool isUpdate = false;
            lock (_lockLeft)
            {
                bvhdata.SetValue(_BvhLeft);
                isUpdate = IsLeftUpdata;
                IsLeftUpdata = false;
            }
            return isUpdate;
        }
        public bool  GetBvhtemRight(ref GloveBVHData bvhdata)
        {
            bool isUpdate = false;
            lock (_lockRight)
            {
                bvhdata.SetValue(_BvhRight);
                isUpdate = IsRightUpdata;
                IsRightUpdata = false;
            }
            return isUpdate;
        }

        public  void WriteBvhData(GloveBVHData data)
        {
            if(data.glove == GloveMod.GM_LeftGlove)
            {
                lock (_lockLeft)
                {
                    
                    _BvhLeft.SetValue(data);
                    IsLeftUpdata = true;
                }
                //for (int i = 0; i < _BvhLeft.data.Length; i++)
                //{
                //    float temp = _BvhLeft.data[i];
                //    string temp1 = "i=" + i;
                //    temp1 += "    WriteBvhDataleft =  " + temp;
                //    LogFileModule.GetInstance().ThreadWrite(temp1, "WriteBvhDataleft", LogType.Error);


                //}
            }
            if (data.glove == GloveMod.GM_RightGlove)
            {
                lock (_lockRight)
                {
                    _BvhRight.SetValue(data);
                    IsRightUpdata = true;
                }
            }
        }
    }
}
