using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
namespace HI5
{
    //public class Hi5_RingBuff 
    //{
    //    public:
    //        List<GloveBVHData>
    //}
    //public class RingBuffer<T>
    //{
    //    public long start = 0;
    //    /// <summary>
    //    /// protected long _x0, _x1, _x2, _x3, _x4, _x5, _x6;
    //    /// </summary>
    //    public long end = 0;
    //    public T[] buffer;

    //    public RingBuffer(int size)
    //    {
    //        buffer = new T[size + 1];
    //    }

    //    public bool Empty()
    //    {
    //        return start == end;
    //    }

    //    public bool Push(T data)
    //    {
    //        if (end >= start)
    //        {
    //            var freeLen = buffer.Length - (end - start);
    //            if (freeLen <= 1)
    //            {
    //                return false;
    //            }

    //            var cutLen0 = buffer.Length - end;
    //            if (cutLen0 >= 1)
    //            {
    //                buffer[end] = data;
    //                ++end;
    //            }
    //            else
    //            {
    //                buffer[0] = data;
    //                end = 1;
    //            }

    //        }
    //        else
    //        {
    //            var freeLen = start - end;
    //            if (freeLen <= 1)
    //            {
    //                return false;
    //            }

    //            buffer[end] = data;
    //            ++end;
    //        }

    //        return true;
    //    }

    //    public T Pop()
    //    {
    //        T retObj = default;

    //        if (end == start)
    //        {
    //            return retObj;
    //        }
    //        else if (end > start)
    //        {
    //            var usedLen = end - start;
    //            if (usedLen < 1)
    //            {
    //                return retObj;
    //            }

    //            retObj = buffer[start];
    //            ++start;
    //            return retObj;
    //        }
    //        else
    //        {
    //            var usedLen = buffer.Length - (start - end);
    //            if (usedLen < 1)
    //            {
    //                return retObj;
    //            }

    //            var cutLen0 = buffer.Length - start;
    //            if (cutLen0 >= 1)
    //            {
    //                retObj = buffer[start];
    //                ++start;
    //                return retObj;
    //            }
    //            else
    //            {
    //                retObj = buffer[0];
    //                start = 1;
    //                return retObj;
    //            }
    //        }
    //    }
    //}


    //// class Program
    // {
    //     const int RingBufferSize = 1024 * 1024 * 3;
    //     const int LoopNum = 100 * 5;

    //     static float st1 = 0.0f;
    //     static float st2 = 0.0f;

    //     static void testPushAndPop()
    //     {
    //         float t1 = 0.0f;
    //         float t2 = 0.0f;
    //         //var rg = new RingBuffer<byte>(RingBufferSize);
    //         //var rg = new RingBuffer1<byte>(RingBufferSize);
    //         var rg = new ByteRingBuffer(RingBufferSize);
    //         bool push = false;
    //         bool pop = false;

    //         var popThread = new Thread(() => {
    //             Stopwatch st = new Stopwatch();
    //             st.Start();
    //             for (var i = 0; i < RingBufferSize; i++)
    //             {
    //                 rg.Pop();
    //             }
    //             st.Stop();
    //             t1 += st.ElapsedMilliseconds;
    //             push = true;
    //         });

    //         var pushThread = new Thread(() => {
    //             Stopwatch st = new Stopwatch();
    //             st.Start();
    //             for (var ii = 0; ii < RingBufferSize; ii++)
    //             {
    //                 rg.Push(1);
    //             }
    //             st.Stop();
    //             t2 += st.ElapsedMilliseconds;
    //             pop = true;
    //         });

    //         pushThread.Start();
    //         popThread.Start();

    //         Thread.Sleep(70);

    //         while (!pop && !push)
    //         {
    //             Thread.Sleep(10);
    //         }

    //         st1 += t1;
    //         st2 += t2;

    //         Console.WriteLine("push avg time: " + t1);
    //         Console.WriteLine("pop avg time: " + t2);
    //     }



    //public class RingBufferManager
    //{
    //    public List<GloveBVHData> Buffer { get; set; } // 存放内存的数组
    //    public int DataCount { get; set; } // 写入数据大小
    //    public int DataStart { get; set; } // 数据起始索引
    //    public int DataEnd { get; set; }   // 数据结束索引
    //    public RingBufferManager(int bufferSize)
    //    {
    //        DataCount = 0; DataStart = 0; DataEnd = 0;
    //        Buffer = new List<GloveBVHData>(bufferSize);
    //    }

    //    public GloveBVHData this[int index]
    //    {
    //        get
    //        {
    //            if (index >= DataCount)
    //                throw new Exception("环形缓冲区异常，索引溢出");
    //            if (DataStart + index < Buffer.Count)
    //            {
    //                return Buffer[DataStart + index];
    //            }
    //            else
    //            {
    //                return Buffer[(DataStart + index) - Buffer.Count];
    //            }
    //        }
    //    }

    //    public int GetDataCount() // 获得当前写入的字节数
    //    {
    //        return DataCount;
    //    }

    //    public int GetReserveCount() // 获得剩余的字节数
    //    {
    //        return Buffer.Count - DataCount;
    //    }

    //    public void Clear()
    //    {
    //        DataCount = 0;
    //    }

    //    public void Clear(int count) // 清空指定大小的数据
    //    {
    //        if (count >= DataCount) // 如果需要清理的数据大于现有数据大小，则全部清理
    //        {
    //            DataCount = 0;
    //            DataStart = 0;
    //            DataEnd = 0;
    //        }
    //        else
    //        {
    //            if (DataStart + count >= Buffer.Count)
    //            {
    //                DataStart = (DataStart + count) - Buffer.Count;
    //            }
    //            else
    //            {
    //                DataStart += count;
    //            }
    //            DataCount -= count;
    //        }
    //    }

    //    public void WriteBuffer(byte[] buffer, int offset, int count)
    //    {
    //        long reserveCount = Buffer.Count - DataCount;
    //        if (reserveCount >= count)                          // 可用空间够使用
    //        {
    //            if (DataEnd + count < Buffer.Count)            // 数据没到结尾
    //            {
    //                Array.Copy(buffer, offset, Buffer, DataEnd, count);
    //                DataEnd += count;
    //                DataCount += count;
    //            }
    //            else           //  数据结束索引超出结尾 循环到开始
    //            {
    //                System.Diagnostics.Debug.WriteLine("缓存重新开始....");
    //                Int32 overflowIndexLength = (DataEnd + count) - Buffer.Length;      // 超出索引长度
    //                Int32 endPushIndexLength = count - overflowIndexLength;             // 填充在末尾的数据长度
    //                Array.Copy(buffer, offset, Buffer, DataEnd, endPushIndexLength);
    //                DataEnd = 0;
    //                offset += endPushIndexLength;
    //                DataCount += endPushIndexLength;
    //                if (overflowIndexLength != 0)
    //                {
    //                    Array.Copy(buffer, offset, Buffer, DataEnd, overflowIndexLength);
    //                }
    //                DataEnd += overflowIndexLength;                                     // 结束索引
    //                DataCount += overflowIndexLength;                                   // 缓存大小
    //            }
    //        }
    //        else
    //        {
    //            // 缓存溢出，不处理
    //        }
    //    }

    //    public void ReadBuffer(byte[] targetBytes, long offset, long count)
    //    {
    //        if (count > DataCount) throw new Exception("环形缓冲区异常，读取长度大于数据长度");
    //        long tempDataStart = DataStart;
    //        if (DataStart + count < Buffer.Length)
    //        {
    //            Array.Copy(Buffer, DataStart, targetBytes, offset, count);
    //        }
    //        else
    //        {
    //            long overflowIndexLength = (DataStart + count) - Buffer.Length;    // 超出索引长度
    //            long endPushIndexLength = count - overflowIndexLength;             // 填充在末尾的数据长度
    //            Array.Copy(Buffer, DataStart, targetBytes, offset, endPushIndexLength);

    //            offset += endPushIndexLength;

    //            if (overflowIndexLength != 0)
    //            {
    //                Array.Copy(Buffer, 0, targetBytes, offset, overflowIndexLength);
    //            }
    //        }
    //    }


    //    public void WriteBuffer(byte[] buffer)
    //    {
    //        WriteBuffer(buffer, 0, buffer.Length);
    //    }

    //}

    public class RingBuff<T> where T:new () 
    {
        protected enum ERingOffPositionStatue
        {
            EEmpty = 0,
            EUse = 1,
        }
        protected List<T> m_array;
        protected List<ERingOffPositionStatue> m_IsEmpty;     //占位判断是否空
        public int DataEnd { get; set; }   // 数据结束索引
        public int Capacity { get; set; }   //容量
        private object _LockSurplusCapacity;
        public  int SurplusCapacity    //剩余容量
        {
            get
            {
                lock(_LockSurplusCapacity)
                    return m_SurplusCapacity;
            }
            set
            {
                lock (_LockSurplusCapacity)
                    m_SurplusCapacity =value;
            }
        } 
        private int m_SurplusCapacity;
        static readonly int MaxCapacity = 256;  //最大容量
        static readonly int MaxCapacity_blance = 5;   //最大容量时容器冗余最少达到多少时开始缩帧
        static readonly int OffsetShrink = 3;    //进行缩帧时与 读取帧和写入帧安全保护距离
        private object _LockDataRead;
        private int m_DataRead;
        public  int DataRead      //读取位置
        {
            get
            {
                //lock(_LockDataRead)
                    return m_DataRead;
            }
             set
            {
                //lock (_LockDataRead)
                    m_DataRead = value;
            }
        }

        private object _LockDataWrite;
        private int m_DataWrite;
        public int DataWrite      //写入位置
        {
            get
            {
                //lock (_LockDataWrite)
                    return m_DataWrite;
            }
            set
            {
               // lock (_LockDataWrite)
                    m_DataWrite = value;
            }
        }

        private object _Lockloop;
        private int mloop = 0;
        public int Loop      //写入位置
        {
            get
            {
                lock (_Lockloop)
                    return mloop;
            }
            set
            {
                lock (_Lockloop)
                    mloop = value;
            }
        }
       

        private int m_DataCount;   //使用的总数据量  
        private object _LockDataCount;
        public int DataCount
        {
            get
            {
                lock (_LockDataCount)
                    return m_DataCount;
            }
            set
            {
                lock (_LockDataCount)
                    m_DataCount = value;
            }
        }

        public  delegate T  DelegateAndFrame(T a,T b );
        public DelegateAndFrame m_DelegateAndFrame;
        public void Init(int _capacity)
        {
            m_array = new List<T>(_capacity);
            m_IsEmpty = new List<ERingOffPositionStatue>(_capacity);
            for (int i = 0; i < _capacity; i++)
            {
                m_array.Add(new T());
                m_IsEmpty.Add(ERingOffPositionStatue.EEmpty);
            }
            DataCount = 0;
            DataRead = 0;
            DataWrite = 0;
            DataEnd = _capacity -1;
            SurplusCapacity = _capacity;
            Loop = 0;
            Capacity = _capacity;
        }

        private void CleanEmpty(int start,int end)
        {
            int cursor = start;
            int emptycursor = -1;
           // int cleanCount = 0;
            while (cursor != end)
            {
                if (m_IsEmpty[cursor] == ERingOffPositionStatue.EUse)
                {
                    if (emptycursor != -1)
                    {
                        m_array[emptycursor] = m_array[cursor];
                        m_IsEmpty[cursor] = ERingOffPositionStatue.EEmpty;
                        if (emptycursor == DataEnd)
                            emptycursor = 0;
                        else
                            emptycursor++;
                        cursor++;
                       // cleanCount++;
                    }
                }
                else 
                {
                    if(emptycursor == -1)
                    {
                        emptycursor = cursor;
                        cursor++;
                    }
                }                
            }
        }

        public void ExtendCapacity(int add_capacity)
        {
            if(m_array != null)
            {
                Capacity += add_capacity;
                DataEnd += add_capacity;
                SurplusCapacity += add_capacity;
                m_IsEmpty.Capacity += add_capacity;
                m_array.Capacity += add_capacity;
                for(int i = 0; i< add_capacity; i++)
                {
                    m_array.Add(new T());
                    m_IsEmpty.Add(ERingOffPositionStatue.EEmpty);
                }
            }
        }

        public bool  WriteData(T inValue)
        {
            //扩容
            if (Capacity < MaxCapacity)
            {
                if (SurplusCapacity <= 2)
                {
                    ExtendCapacity(((int)Capacity) * 2);
                }
            }
            else
            {
                //追帧处理
                if(SurplusCapacity <= MaxCapacity_blance)
                {
                    int cleanCount = 0;//清除数量
                    if (m_IsEmpty.Count > DataRead && m_IsEmpty.Count> DataWrite)
                    {
                        int start = DataRead;
                        int end = DataWrite-1;
                        int count = OffsetShrink;
                        int isLoop = Loop;
                        if(Loop >1)
                        {
                            return false;
                        }
                        //偏移start和end
                        while (count >0)
                        {
                           if(end == 0)
                            {
                                end = DataEnd;
                                isLoop--;
                            }
                           else
                            {
                                end--;
                            }
                            if (m_IsEmpty[end] == ERingOffPositionStatue.EUse)
                            {
                                count--;
                            }
                        }
                        count = OffsetShrink;
                        while (count > 0)
                        {
                            if (start == DataEnd)
                            {
                                start = 0;
                                isLoop--;

                            }
                            else
                            {
                                start++;
                            }
                            if (m_IsEmpty[start] == ERingOffPositionStatue.EUse)
                            {
                                count--;
                            }
                        }
                        int shrinkPre = 0;
                        int shrinkIndex = start;
                        int coutWhile = 2; 

                        //两项合成一项操作
                        while(shrinkIndex != end)
                        {
                           if (m_IsEmpty[shrinkIndex] != ERingOffPositionStatue.EUse)
                           {
                                if (shrinkIndex == DataEnd)
                                {
                                    shrinkIndex = 0;
                                }
                                else
                                {
                                    shrinkIndex++;
                                }
                            }
                            else
                            {
                                if(coutWhile == 2)
                                {
                                    shrinkPre = shrinkIndex;
                                    coutWhile--;
                                }
                                else if (coutWhile == 1)
                                {
                                    if(m_DelegateAndFrame != null)
                                    {
                                        T temp = m_DelegateAndFrame(m_array[shrinkPre], m_array[shrinkIndex]);
                                        cleanCount++;
                                        m_array[shrinkPre] = temp;
                                        
                                    }
                                    m_IsEmpty[shrinkIndex] = ERingOffPositionStatue.EEmpty;
                                    coutWhile = 2;
                                }
                                if (shrinkIndex == DataEnd)
                                {
                                    shrinkIndex = 0;
                                }
                                else
                                {
                                    shrinkIndex++;
                                }
                            }

                        }
                        //清除空位
                        CleanEmpty(start,end);
                        DataCount -= cleanCount;
                        SurplusCapacity += cleanCount;
                    }
                }
            }
               
            if (m_IsEmpty.Count > DataWrite)
            {
                if(Loop > 1 
                    || ((Loop == 1) && (DataWrite >= DataRead) ) 
                    ||  ((Loop == 0) && (DataWrite <= DataRead)))
                {
                    return false;
                }
                else
                {
                    while (m_IsEmpty[DataWrite] != ERingOffPositionStatue.EEmpty)
                    {
                        if (DataWrite == DataEnd)
                        {
                            Loop += 1;
                            DataWrite = 0;
                        }
                        else
                        {
                            DataWrite++;
                        }
                    }
                    m_IsEmpty[DataWrite] = ERingOffPositionStatue.EUse;
                    m_array[DataWrite] = inValue;
                    DataCount += 1;
                    SurplusCapacity -= 1;
                    if (DataWrite == DataEnd)
                    {
                        Loop += 1;
                        DataWrite = 0;
                    }
                    else
                    {
                        DataWrite++;
                    }
                    return true;
                }  
            }
            else
                return false;
        }
        public bool ReadData(out T outValue)
        {
            if(DataCount <= 1)  
            {
                outValue = default(T);//始终保持保留最后一个读写数据不去读这样就不会发生争抢锁
                return false;
            }
            else
            {
                if (m_IsEmpty.Count > DataRead)
                {
                    if (Loop > 1
                  || ((Loop == 1) && (DataWrite >= DataRead))
                  || ((Loop == 0) && (DataWrite <= DataRead)))
                    {
                        outValue = default(T);
                        return false;
                    }
                    else
                    {
                        while (m_IsEmpty[(int)DataRead] != ERingOffPositionStatue.EUse)
                        {
                            if (DataRead == DataEnd)
                            {
                                Loop -= 1;
                                DataRead = 0;
                            }
                            else
                            {
                                DataRead++;
                            }
                        }
                        m_IsEmpty[DataRead] = ERingOffPositionStatue.EEmpty;
                        outValue = m_array[DataRead];
                        DataCount -= 1;
                        SurplusCapacity += 1;
                        if (DataRead == DataEnd)
                        {
                            Loop -= 1;
                            DataRead = 0;
                        }
                        else
                        {
                            DataRead++;
                        }
                        return true;
                    }
                }
                else
                {
                    outValue = default(T);
                    return false;
                }
            }
        }
    }
}
