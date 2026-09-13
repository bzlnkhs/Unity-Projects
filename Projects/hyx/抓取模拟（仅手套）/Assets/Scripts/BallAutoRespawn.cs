using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallAutoRespawn : MonoBehaviour
{
    public Vector3 spawnPosition;
    public Vector3 spawnRotationEuler;
    public float respawnY = 0.3f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (spawnPosition == Vector3.zero)
            spawnPosition = transform.position;
    }

    private void Update()
    {
        if (transform.position.y < respawnY)
        {
            Respawn();
        }
    }

    [ContextMenu("Respawn Now")]
    public void Respawn()
    {
        FixedJoint joint = GetComponent<FixedJoint>();
        if (joint != null)
            Destroy(joint);

        transform.SetParent(null);
        transform.position = spawnPosition;
        transform.rotation = Quaternion.Euler(spawnRotationEuler);

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = true;

        Grabbable g = GetComponent<Grabbable>();
        if (g != null)
            g.isGrabbed = false;
    }
}