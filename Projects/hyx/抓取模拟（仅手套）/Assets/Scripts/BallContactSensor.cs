using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallContactSensor : MonoBehaviour
{
    public Grabbable currentBall;

    private void OnTriggerEnter(Collider other)
    {
        Grabbable g = other.GetComponent<Grabbable>();
        if (g == null)
            g = other.GetComponentInParent<Grabbable>();

        if (g != null)
            currentBall = g;
    }

    private void OnTriggerStay(Collider other)
    {
        Grabbable g = other.GetComponent<Grabbable>();
        if (g == null)
            g = other.GetComponentInParent<Grabbable>();

        if (g != null)
            currentBall = g;
    }

    private void OnTriggerExit(Collider other)
    {
        Grabbable g = other.GetComponent<Grabbable>();
        if (g == null)
            g = other.GetComponentInParent<Grabbable>();

        if (currentBall == g)
            currentBall = null;
    }
}
