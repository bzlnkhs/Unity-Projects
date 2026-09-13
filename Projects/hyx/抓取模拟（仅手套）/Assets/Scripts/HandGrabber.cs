using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HI5;

public class HandGrabber : MonoBehaviour
{
    public enum HandSide
    {
        Left,
        Right
    }

    [Header("Hand")]
    public HandSide handSide = HandSide.Right;
    public Transform gripPoint;
    public Transform thumbTip;
    public Transform indexTip;

    [Header("Grab")]
    public float pinchDistance = 0.035f;
    public float releaseDistance = 0.05f;
    public float grabRadius = 0.08f;
    public LayerMask grabbableMask = ~0;

    private Grabbable currentGrab;
    private Transform originalParent;
    private bool isPinching;

    private void Update()
    {
        if (thumbTip == null || indexTip == null || gripPoint == null)
            return;

        float tipDistance = Vector3.Distance(thumbTip.position, indexTip.position);

        if (!isPinching && tipDistance <= pinchDistance)
            isPinching = true;
        else if (isPinching && tipDistance >= releaseDistance)
            isPinching = false;

        if (currentGrab == null)
        {
            if (isPinching)
            {
                TryGrab();
            }
        }
        else
        {
            if (!isPinching)
            {
                Release();
            }
            else
            {
                currentGrab.transform.position = gripPoint.position;
                currentGrab.transform.rotation = gripPoint.rotation;
            }
        }
    }

    private void TryGrab()
    {
        Collider[] hits = Physics.OverlapSphere(gripPoint.position, grabRadius, grabbableMask);

        Grabbable best = null;
        float bestDist = float.MaxValue;

        foreach (var hit in hits)
        {
            Grabbable g = hit.GetComponentInParent<Grabbable>();
            if (g == null) continue;

            float d = Vector3.Distance(gripPoint.position, g.transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                best = g;
            }
        }

        if (best != null)
        {
            currentGrab = best;
            originalParent = currentGrab.transform.parent;

            currentGrab.rb.velocity = Vector3.zero;
            currentGrab.rb.angularVelocity = Vector3.zero;
            currentGrab.rb.useGravity = false;
            currentGrab.rb.isKinematic = true;

            currentGrab.transform.SetParent(gripPoint);
            currentGrab.transform.position = gripPoint.position;
            currentGrab.transform.rotation = gripPoint.rotation;

            // 抓到时震动一下
            if (handSide == HandSide.Left)
                HI5_Manager.EnableLeftVibration(80);
            else
                HI5_Manager.EnableRightVibration(80);
        }
    }

    private void Release()
    {
        if (currentGrab == null) return;

        currentGrab.transform.SetParent(originalParent);
        currentGrab.rb.isKinematic = false;
        currentGrab.rb.useGravity = true;

        currentGrab = null;
    }

    private void OnDrawGizmosSelected()
    {
        if (gripPoint == null) return;
        Gizmos.DrawWireSphere(gripPoint.position, grabRadius);
    }
}
