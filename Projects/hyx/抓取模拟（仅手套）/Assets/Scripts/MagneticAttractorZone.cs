using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticAttractorZone : MonoBehaviour
{
    [Header("Target")]
    public Transform magneticCenter;

    [Header("Attraction")]
    public float attractionForce = 8f;
    public float maxSpeed = 1.2f;
    public float stopRadius = 0.03f;
    public bool horizontalOnly = true;

    [Header("Stabilize")]
    public float centerDrag = 4f;
    public float normalDrag = 1.2f;

    private void OnTriggerStay(Collider other)
    {
        Grabbable g = other.GetComponent<Grabbable>();
        if (g == null) return;
        if (g.rb == null) return;
        if (g.isGrabbed) return;
        if (magneticCenter == null) return;

        Vector3 targetPos = magneticCenter.position;
        Vector3 dir = targetPos - g.transform.position;

        if (horizontalOnly)
            dir.y = 0f;

        float dist = dir.magnitude;

        if (dist <= stopRadius)
        {
            g.rb.velocity = Vector3.Lerp(g.rb.velocity, Vector3.zero, 0.2f);
            g.rb.angularVelocity = Vector3.Lerp(g.rb.angularVelocity, Vector3.zero, 0.2f);
            g.rb.drag = centerDrag;
            return;
        }

        g.rb.drag = normalDrag;

        Vector3 forceDir = dir.normalized;
        g.rb.AddForce(forceDir * attractionForce, ForceMode.Acceleration);

        if (g.rb.velocity.magnitude > maxSpeed)
        {
            g.rb.velocity = g.rb.velocity.normalized * maxSpeed;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Grabbable g = other.GetComponent<Grabbable>();
        if (g == null || g.rb == null) return;

        g.rb.drag = normalDrag;
    }

    private void OnDrawGizmos()
    {
        if (magneticCenter != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(magneticCenter.position, stopRadius);
        }
    }
}
