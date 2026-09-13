using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HI5;

public class PalmSnapGrabber : MonoBehaviour
{
    [Header("Hand")]
    public Transform gripPoint;
    public Transform thumbTip;
    public Transform indexTip;
    public Transform middleTip;
    public Transform ringTip;
    public Transform pinkyTip;

    [Header("Target")]
    public Grabbable targetBall;

    [Header("Settings")]
    public float touchDistance = 0.03f;
    public bool vibrateOnGrab = true;
    public int vibrateMs = 80;

    private bool grabbed = false;
    private Transform originalParent;

    private void Update()
    {
        if (targetBall == null || gripPoint == null)
            return;

        if (grabbed)
        {
            targetBall.transform.position = gripPoint.position;
            targetBall.transform.rotation = gripPoint.rotation;
            return;
        }

        if (IsTouchingAnyFinger(targetBall.transform))
        {
            GrabToPalm();
        }
    }

    private bool IsTouchingAnyFinger(Transform ball)
    {
        if (thumbTip != null && Vector3.Distance(thumbTip.position, ball.position) <= touchDistance) return true;
        if (indexTip != null && Vector3.Distance(indexTip.position, ball.position) <= touchDistance) return true;
        if (middleTip != null && Vector3.Distance(middleTip.position, ball.position) <= touchDistance) return true;
        if (ringTip != null && Vector3.Distance(ringTip.position, ball.position) <= touchDistance) return true;
        if (pinkyTip != null && Vector3.Distance(pinkyTip.position, ball.position) <= touchDistance) return true;

        return false;
    }

    private void GrabToPalm()
    {
        grabbed = true;
        originalParent = targetBall.transform.parent;

        if (targetBall.rb != null)
        {
            targetBall.rb.velocity = Vector3.zero;
            targetBall.rb.angularVelocity = Vector3.zero;
            targetBall.rb.useGravity = false;
            targetBall.rb.isKinematic = true;
        }

        targetBall.transform.SetParent(gripPoint);
        targetBall.transform.position = gripPoint.position;
        targetBall.transform.rotation = gripPoint.rotation;

        if (vibrateOnGrab)
        {
            HI5_Manager.EnableRightVibration(vibrateMs);
        }
    }
}
