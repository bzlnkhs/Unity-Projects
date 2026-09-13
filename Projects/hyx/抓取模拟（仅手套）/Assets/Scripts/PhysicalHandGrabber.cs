using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HI5;

public class PhysicalHandGrabber : MonoBehaviour
{
    [Header("Anchor")]
    public Rigidbody grabAnchorBody;
    public Transform palmCenter;

    [Header("Fingertips")]
    public Transform thumbTip;
    public Transform indexTip;
    public Transform middleTip;
    public Transform ringTip;
    public Transform pinkyTip;

    [Header("Target")]
    public Grabbable targetBall;

    [Header("Grab Settings")]
    public float palmRadius = 0.06f;
    public float fingerTouchDistance = 0.035f;
    public int requiredFingerCount = 2;
    public float releaseDistance = 0.09f;

    private FixedJoint currentJoint;
    private bool grabbed = false;

    private void Update()
    {
        if (targetBall == null || palmCenter == null || grabAnchorBody == null)
            return;

        int touchingFingers = CountTouchingFingers();
        float ballToPalm = Vector3.Distance(targetBall.transform.position, palmCenter.position);

        if (!grabbed)
        {
            if (ballToPalm <= palmRadius && touchingFingers >= requiredFingerCount)
            {
                Grab();
            }
        }
        else
        {
            if (touchingFingers == 0 || ballToPalm > releaseDistance)
            {
                Release();
            }
        }
    }

    private int CountTouchingFingers()
    {
        int count = 0;

        if (thumbTip != null && Vector3.Distance(thumbTip.position, targetBall.transform.position) <= fingerTouchDistance) count++;
        if (indexTip != null && Vector3.Distance(indexTip.position, targetBall.transform.position) <= fingerTouchDistance) count++;
        if (middleTip != null && Vector3.Distance(middleTip.position, targetBall.transform.position) <= fingerTouchDistance) count++;
        if (ringTip != null && Vector3.Distance(ringTip.position, targetBall.transform.position) <= fingerTouchDistance) count++;
        if (pinkyTip != null && Vector3.Distance(pinkyTip.position, targetBall.transform.position) <= fingerTouchDistance) count++;

        return count;
    }

    private void Grab()
    {
        if (grabbed || targetBall == null || targetBall.rb == null) return;

        currentJoint = targetBall.gameObject.AddComponent<FixedJoint>();
        currentJoint.connectedBody = grabAnchorBody;
        currentJoint.breakForce = Mathf.Infinity;
        currentJoint.breakTorque = Mathf.Infinity;
        currentJoint.enableCollision = false;

        targetBall.rb.useGravity = false;
        targetBall.isGrabbed = true;
        grabbed = true;

        HI5_Manager.EnableRightVibration(80);
    }

    private void Release()
    {
        if (!grabbed) return;

        if (currentJoint != null)
            Destroy(currentJoint);

        if (targetBall != null && targetBall.rb != null)
        {
            targetBall.rb.useGravity = true;
            targetBall.isGrabbed = false;
        }

        grabbed = false;
    }
}