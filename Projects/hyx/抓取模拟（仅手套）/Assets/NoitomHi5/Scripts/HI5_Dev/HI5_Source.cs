using System;
using System.Collections.Generic;
using UnityEngine;

namespace HI5
{
    /// <summary>
    /// Manage the received data of HI5 glove
    /// </summary>
    public class HI5_Source
    {
        /// <summary>
        /// The position data array of left hand bones.
        /// The array is referenced to Bones enum.
        /// </summary>
        public Vector3[] L_BonePos = new Vector3[(int)Bones.NumOfHI5Bones];
        /// <summary>
        /// The position data array of right hand bones.
        /// The array is referenced to Bones enum.
        /// </summary>
        public Vector3[] R_BonePos = new Vector3[(int)Bones.NumOfHI5Bones];
        /// <summary>
        /// The rotation data array of left hand bones.
        /// The array is referenced to Bones enum.
        /// </summary>
        public Vector3[] L_BoneRot = new Vector3[(int)Bones.NumOfHI5Bones];
        /// <summary>
        /// The rotation data array of right hand bones.
        /// The array is referenced to Bones enum.
        /// </summary>
        public Vector3[] R_BoneRot = new Vector3[(int)Bones.NumOfHI5Bones];

        private string AvatarName;
        private bool withDisp;
        //private int frameIndex;
        //private int count;

        /**
         * \cond INTERNAL_USE
         */
        /// <summary>
        /// Called internally. Update the position and rotation data of hand bones.
        /// </summary>
        /// <param name="gloveData"></param>
        internal void UpdateTrackingData(GloveBVHData gloveData)
        {
            //if displacement in bhv 
            

            GloveMod mode = gloveData.glove;
            int offset = 0;
            if (mode == GloveMod.GM_LeftGlove)
            {
                withDisp = gloveData.bWithDisp == 1;

                float[] data = gloveData.data;
                for (int i = 0; i < L_BoneRot.Length; i++)
                {
                    
                    //y -x -z 按欧拉角旋转给结果 
                    offset = withDisp ? 3 + 6 * i : 3 + 3 * i;
                    L_BoneRot[i].Set(data[offset+1], -data[offset+0], -data[offset+2]);
                    if(withDisp)
                    {
                        L_BonePos[i].Set(data[i * 6 + 0], data[i * 6 + 1], data[i * 6 + 2]);
                        //if (i == 0)
                        //    Debug.LogError("UpdateTrackingData left i  0" + L_BonePos[i]);
                        //else if (i == 1)
                        //    Debug.LogError("UpdateTrackingData left i  1" + L_BonePos[i]);
                    }
                        
                }
            }
            else if (mode == GloveMod.GM_RightGlove)
            {
                withDisp = gloveData.bWithDisp == 1;

                float[] data = gloveData.data;
                for (int i = 0; i < R_BoneRot.Length; i++)
                {
                    
                    offset = withDisp ? 3 + 6 * i : 3 + 3 * i;
                    R_BoneRot[i].Set(data[offset+1], -data[offset+0], -data[offset+2]);
                    if(withDisp)
                    {
                        R_BonePos[i].Set(data[i * 6 + 0], data[i * 6 + 1], data[i * 6 + 2]);
                        //if (i == 0)
                        //    Debug.LogError("UpdateTrackingData right i  0" + R_BonePos[i]);
                        //else if (i == 1)
                        //    Debug.LogError("UpdateTrackingData right i  1" + R_BonePos[i]);
                    }
                        
                }
            }
        }

        /**
         * \endcond
         */


        /// <summary>
        /// Get received rotation data of left/right hand specific bones.
        /// The index is referenced to Bones enum.
        /// </summary>
        /// <param name="boneIndex">
        /// The index of the specific bone. Reference by <see cref="HI5.Bones"/>.
        /// </param>
        /// <param name="handType">
        /// The type of <see cref="HI5.Hand"/>.
        /// </param>
        /// <returns>
        /// The received rotation of specific bone by <see cref="UnityEngine.Vector3"/>.
        /// </returns>
        public Vector3 GetReceivedRotation(int boneIndex, Hand handType)
        {
            return handType == Hand.LEFT ? L_BoneRot[boneIndex] : R_BoneRot[boneIndex];
        }

        /// <summary>
        /// Get received position data of left/right hand specific bones.
        /// The index is referenced to Bones enum.
        /// </summary>
        /// <param name="boneIndex">
        /// The index of the specific bone. Reference by <see cref="HI5.Bones"/>.
        /// </param>
        /// <param name="handType">
        /// The type of <see cref="HI5.Hand"/>.
        /// </param>
        /// <returns>
        /// The received position of specific bone by <see cref="UnityEngine.Vector3"/>.
        /// </returns>
        public Vector3 GetReceivedPosition(int boneIndex, Hand handType)
        {
            return handType == Hand.LEFT ? L_BonePos[boneIndex] : R_BonePos[boneIndex];
        }
    }
}
 