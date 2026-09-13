using System;
using UnityEngine;
using System.Collections.Generic;

/**
 * \cond INTERNAL_USE
 */

namespace HI5
{

    /*
    /// <summary>
    /// HI5 bones reference.
    /// </summary>
    public enum Bones
    {
        /// <summary>
        /// The fore arm joint.
        /// </summary>
        ForeArm = 0,
        /// <summary>
        /// The hand joint.
        /// </summary>
        Hand = 1,
        /// <summary>
        /// The metacarpal joint of thumb finger.
        /// </summary>
        HandThumb1,
        /// <summary>
        /// The proximal joint of thumb finger.
        /// </summary>
        HandThumb2,
        /// <summary>
        /// The distal joint of thumb finger.
        /// </summary>
        HandThumb3,
        /// <summary>
        /// The metacarpal joint of index finger.
        /// </summary>
        InHandIndex,
        /// <summary>
        /// The proximal joint of index finger.
        /// </summary>
        HandIndex1,
        /// <summary>
        /// The middle joint of index finger.
        /// </summary>
        HandIndex2,
        /// <summary>
        /// The distal joint of index finger.
        /// </summary>
        HandIndex3,
        /// <summary>
        /// The metacarpal joint of middle finger.
        /// </summary>
        InHandMiddle,
        /// <summary>
        /// The proximal joint of middle finger.
        /// </summary>
        HandMiddle1,
        /// <summary>
        /// The middle joint of middle finger.
        /// </summary>
        HandMiddle2,
        /// <summary>
        /// The distal joint of middle finger.
        /// </summary>
        HandMiddle3,
        /// <summary>
        /// The metacarpal joint of ring finger.
        /// </summary>
        InHandRing,
        /// <summary>
        /// The proximal joint of ring finger.
        /// </summary>
        HandRing1,
        /// <summary>
        /// The middle joint of ring finger.
        /// </summary>
        HandRing2,
        /// <summary>
        /// The distal joint of ring finger.
        /// </summary>
        HandRing3,
        /// <summary>
        /// The metacarpal joint of pinky finger.
        /// </summary>
        InHandPinky,
        /// <summary>
        /// The proximal joint of pinky finger.
        /// </summary>
        HandPinky1,
        /// <summary>
        /// The middle joint of pinky finger.
        /// </summary>
        HandPinky2,
        /// <summary>
        /// The distal joint of pinky finger.
        /// </summary>
        HandPinky3,
        /// <summary>
        /// The number of joints of Hi5 bones.
        /// </summary>
        NumOfHI5Bones,
    }
    */

    internal static class HI5_Bones
    {
        internal enum LeftBonesIndex
        {
            LeftForeArm = 0,
            LeftHand,
            LeftHandThumb1,
            LeftHandThumb2,
            LeftHandThumb3,
            LeftInHandIndex,
            LeftHandIndex1,
            LeftHandIndex2,
            LeftHandIndex3,
            LeftInHandMiddle,
            LeftHandMiddle1,
            LeftHandMiddle2,
            LeftHandMiddle3,
            LeftInHandRing,
            LeftHandRing1,
            LeftHandRing2,
            LeftHandRing3,
            LeftInHandPinky,
            LeftHandPinky1,
            LeftHandPinky2,
            LeftHandPinky3,
            NumOfLeftBones,    //  Max number of this enum
        }

        internal enum RightBonesIndex
        {
            RightForeArm = 0,
            RightHand,
            RightHandThumb1,
            RightHandThumb2,
            RightHandThumb3,
            RightInHandIndex,
            RightHandIndex1,
            RightHandIndex2,
            RightHandIndex3,
            RightInHandMiddle,
            RightHandMiddle1,
            RightHandMiddle2,
            RightHandMiddle3,
            RightInHandRing,
            RightHandRing1,
            RightHandRing2,
            RightHandRing3,
            RightInHandPinky,
            RightHandPinky1,
            RightHandPinky2,
            RightHandPinky3,
            NumOfRightBones,
        }

        /// <summary>
        /// Bind HI5 bones data to each transform of bones on hand.
        /// </summary>
        /// <param name="root">
        /// The root bone of the rigged hand model. Input by <see cref="UnityEngine.Transform"/>.
        /// </param>
        /// <param name="bones">
        /// All the bones of the rigged hand model. The sort of the array is referenced to <see cref="HI5.HI5_Bones.LeftBonesIndex"/> or <see cref="HI5.HI5_Bones.RightBonesIndex"/>.
        /// </param>
        /// <param name="prefix">
        /// The prefix of each bones. Input by <see cref="System.String"/>.
        /// </param>
        /// <param name="handType">
        /// The type of <see cref="HI5.Hand"/>.
        /// </param>
        /// <returns>
        /// Number of successfully binded bones. 
        /// </returns>
        internal static int Bind(Transform root, Transform[] bones, string prefix, Hand handType)
        {
            if (root == null)
            {
                Debug.LogError("[HI5_Helper] Root is null, bind failed.");
                return 0;
            }

            int counter = 0;
            Stack<Transform> stack = new Stack<Transform>();
            stack.Push(root);

            while (stack.Count > 0)
            {
                Transform t = stack.Pop();
                if (t.name.StartsWith(prefix))
                {
                    string bone_name = t.name.Substring(t.name.IndexOf(prefix) + prefix.Length);
                    int index = GetBoneIndex(bone_name, handType);
                    if (index >= 0)
                    {
                        bones[index] = t;
                        ++counter;
                    }
                }

                for (int i = 0; i < t.childCount; ++i)
                {
                    stack.Push(t.GetChild(i));
                }
            }

            return counter;
        }

        private static string GetLeftBoneName(int id)
        {
            return Enum.GetName(typeof(LeftBonesIndex), (LeftBonesIndex)id);
        }

        private static string GetRightBoneName(int id)
        {
            return Enum.GetName(typeof(RightBonesIndex), (RightBonesIndex)id);
        }

        private static int GetBoneIndex(string name, Hand type)
        {
            if (type == Hand.LEFT)
            {
                for (int i = 0; i < (int)LeftBonesIndex.NumOfLeftBones; ++i)
                {
                    if (GetLeftBoneName(i) == name)
                        return i;
                }
            }

            if (type == Hand.RIGHT)
            {
                for (int i = 0; i < (int)RightBonesIndex.NumOfRightBones; ++i)
                {
                    if (GetRightBoneName(i) == name)
                        return i;
                }
            }

            return -1;
        }
    }
}

/**
 * \endcond
 */
