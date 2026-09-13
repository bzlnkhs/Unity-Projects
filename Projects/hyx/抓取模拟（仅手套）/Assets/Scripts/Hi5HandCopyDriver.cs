using HI5;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HI5.HI5_Glove_TransformData_Interface;

public class Hi5HandCopyDriver : MonoBehaviour
{
    [Header("Data Source")]
    public HI5_Glove_TransformData_Interface gloveInterface;

    [Header("Left Hand Model Bones")]
    public Transform leftHand;
    public Transform leftThumb1;
    public Transform leftThumb2;
    public Transform leftThumb3;
    public Transform leftIndex0;
    public Transform leftIndex1;
    public Transform leftIndex2;
    public Transform leftIndex3;
    public Transform leftMiddle0;
    public Transform leftMiddle1;
    public Transform leftMiddle2;
    public Transform leftMiddle3;
    public Transform leftRing0;
    public Transform leftRing1;
    public Transform leftRing2;
    public Transform leftRing3;
    public Transform leftPinky0;
    public Transform leftPinky1;
    public Transform leftPinky2;
    public Transform leftPinky3;

    [Header("Right Hand Model Bones")]
    public Transform rightHand;
    public Transform rightThumb1;
    public Transform rightThumb2;
    public Transform rightThumb3;
    public Transform rightIndex0;
    public Transform rightIndex1;
    public Transform rightIndex2;
    public Transform rightIndex3;
    public Transform rightMiddle0;
    public Transform rightMiddle1;
    public Transform rightMiddle2;
    public Transform rightMiddle3;
    public Transform rightRing0;
    public Transform rightRing1;
    public Transform rightRing2;
    public Transform rightRing3;
    public Transform rightPinky0;
    public Transform rightPinky1;
    public Transform rightPinky2;
    public Transform rightPinky3;

    private void LateUpdate()
    {
        if (gloveInterface == null) return;

        Dictionary<EHi5_Glove_TransformData_Bones, Transform> leftBones = gloveInterface.GetLeftHandTransform();
        Dictionary<EHi5_Glove_TransformData_Bones, Transform> rightBones = gloveInterface.GetRightHandTransform();

        ApplyLeft(leftBones);
        ApplyRight(rightBones);
    }

    private void ApplyLeft(Dictionary<EHi5_Glove_TransformData_Bones, Transform> bones)
    {
        if (bones == null) return;

        CopyRot(bones, EHi5_Glove_TransformData_Bones.Hand, leftHand);
        CopyRot(bones, EHi5_Glove_TransformData_Bones.HandThumb1, leftThumb1);
        CopyRot(bones, EHi5_Glove_TransformData_Bones.HandThumb2, leftThumb2);
        CopyRot(bones, EHi5_Glove_TransformData_Bones.HandThumb3, leftThumb3);

        CopyRot(bones, EHi5_Glove_TransformData_Bones.InHandIndex, leftIndex0);
        CopyRot(bones, EHi5_Glove_TransformData_Bones.HandIndex1, leftIndex1);
        CopyRot(bones, EHi5_Glove_TransformData_Bones.HandIndex2, leftIndex2);
        CopyRot(bones, EHi5_Glove_TransformData_Bones.HandIndex3, leftIndex3);

        CopyRot(bones, EHi5_Glove_TransformData_Bones.InHandMiddle, leftMiddle0);
        CopyRot(bones, EHi5_Glove_TransformData_Bones.HandMiddle1, leftMiddle1);
        CopyRot(bones, EHi5_Glove_TransformData_Bones.HandMiddle2, leftMiddle2);
        CopyRot(bones, EHi5_Glove_TransformData_Bones.HandMiddle3, leftMiddle3);

        CopyRot(bones, EHi5_Glove_TransformData_Bones.InHandRing, leftRing0);
        CopyRot(bones, EHi5_Glove_TransformData_Bones.HandRing1, leftRing1);
        CopyRot(bones, EHi5_Glove_TransformData_Bones.HandRing2, leftRing2);
        CopyRot(bones, EHi5_Glove_TransformData_Bones.HandRing3, leftRing3);

        CopyRot(bones, EHi5_Glove_TransformData_Bones.InHandPinky, leftPinky0);
        CopyRot(bones, EHi5_Glove_TransformData_Bones.HandPinky1, leftPinky1);
        CopyRot(bones, EHi5_Glove_TransformData_Bones.HandPinky2, leftPinky2);
        CopyRot(bones, EHi5_Glove_TransformData_Bones.HandPinky3, leftPinky3);
    }

    private void ApplyRight(Dictionary<EHi5_Glove_TransformData_Bones, Transform> bones)
    {
        if (bones == null) return;

        CopyRot(bones, EHi5_Glove_TransformData_Bones.Hand, rightHand);
        CopyRot(bones, EHi5_Glove_TransformData_Bones.HandThumb1, rightThumb1);
        CopyRot(bones, EHi5_Glove_TransformData_Bones.HandThumb2, rightThumb2);
        CopyRot(bones, EHi5_Glove_TransformData_Bones.HandThumb3, rightThumb3);

        CopyRot(bones, EHi5_Glove_TransformData_Bones.InHandIndex, rightIndex0);
        CopyRot(bones, EHi5_Glove_TransformData_Bones.HandIndex1, rightIndex1);
        CopyRot(bones, EHi5_Glove_TransformData_Bones.HandIndex2, rightIndex2);
        CopyRot(bones, EHi5_Glove_TransformData_Bones.HandIndex3, rightIndex3);

        CopyRot(bones, EHi5_Glove_TransformData_Bones.InHandMiddle, rightMiddle0);
        CopyRot(bones, EHi5_Glove_TransformData_Bones.HandMiddle1, rightMiddle1);
        CopyRot(bones, EHi5_Glove_TransformData_Bones.HandMiddle2, rightMiddle2);
        CopyRot(bones, EHi5_Glove_TransformData_Bones.HandMiddle3, rightMiddle3);

        CopyRot(bones, EHi5_Glove_TransformData_Bones.InHandRing, rightRing0);
        CopyRot(bones, EHi5_Glove_TransformData_Bones.HandRing1, rightRing1);
        CopyRot(bones, EHi5_Glove_TransformData_Bones.HandRing2, rightRing2);
        CopyRot(bones, EHi5_Glove_TransformData_Bones.HandRing3, rightRing3);

        CopyRot(bones, EHi5_Glove_TransformData_Bones.InHandPinky, rightPinky0);
        CopyRot(bones, EHi5_Glove_TransformData_Bones.HandPinky1, rightPinky1);
        CopyRot(bones, EHi5_Glove_TransformData_Bones.HandPinky2, rightPinky2);
        CopyRot(bones, EHi5_Glove_TransformData_Bones.HandPinky3, rightPinky3);
    }

    private void CopyRot(
        Dictionary<EHi5_Glove_TransformData_Bones, Transform> bones,
        EHi5_Glove_TransformData_Bones key,
        Transform target)
    {
        if (target == null) return;
        if (bones == null) return;
        if (!bones.ContainsKey(key)) return;
        if (bones[key] == null) return;

        target.localRotation = bones[key].localRotation;
    }
}
