using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HI5;

public class PhysicalHandGrabberByContact : MonoBehaviour
{
    public Rigidbody grabAnchorBody;
    public BallContactSensor palmSensor;
    public BallContactSensor[] fingerSensors;

    public int requiredFingerCount = 2;
    public float releaseDelay = 0.12f;

    private FixedJoint currentJoint;
    private Grabbable currentBall;
    private bool grabbed = false;
    private float releaseTimer = 0f;

    private void Update()
    {
        if (!grabbed)
        {
            TryGrab();
        }
        else
        {
            CheckRelease();
        }
    }

    private void TryGrab()
    {
        if (palmSensor == null || palmSensor.currentBall == null) return;

        Grabbable candidate = palmSensor.currentBall;
        int count = CountFingerContacts(candidate);

        if (count >= requiredFingerCount)
        {
            currentBall = candidate;
            currentJoint = currentBall.gameObject.AddComponent<FixedJoint>();
            currentJoint.connectedBody = grabAnchorBody;
            currentJoint.breakForce = Mathf.Infinity;
            currentJoint.breakTorque = Mathf.Infinity;
            currentJoint.enableCollision = false;

            currentBall.rb.useGravity = false;
            currentBall.isGrabbed = true;
            grabbed = true;
            releaseTimer = 0f;

            HI5_Manager.EnableRightVibration(80);
        }
    }

    private void CheckRelease()
    {
        if (currentBall == null)
        {
            grabbed = false;
            return;
        }

        int count = CountFingerContacts(currentBall);

        if (count == 0)
        {
            releaseTimer += Time.deltaTime;
            if (releaseTimer >= releaseDelay)
            {
                Release();
            }
        }
        else
        {
            releaseTimer = 0f;
        }
    }

    private int CountFingerContacts(Grabbable target)
    {
        int count = 0;
        if (fingerSensors == null) return 0;

        foreach (var s in fingerSensors)
        {
            if (s != null && s.currentBall == target)
                count++;
        }

        return count;
    }

    private void Release()
    {
        if (currentJoint != null)
            Destroy(currentJoint);

        if (currentBall != null && currentBall.rb != null)
        {
            currentBall.rb.useGravity = true;
            currentBall.isGrabbed = false;
        }

        currentJoint = null;
        currentBall = null;
        grabbed = false;
        releaseTimer = 0f;
    }
}
