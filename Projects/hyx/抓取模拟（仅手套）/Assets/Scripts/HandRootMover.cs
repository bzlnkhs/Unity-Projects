using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HandRootMover : MonoBehaviour
{
    public float moveSpeed = 0.6f;
    public float rotateSpeed = 90f;

    private Rigidbody rb;
    private Vector3 moveInput;
    private float yawInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float x = 0f;
        float y = 0f;
        float z = 0f;
        yawInput = 0f;

        if (Input.GetKey(KeyCode.A)) x -= 1f;
        if (Input.GetKey(KeyCode.D)) x += 1f;
        if (Input.GetKey(KeyCode.Q)) y -= 1f;
        if (Input.GetKey(KeyCode.E)) y += 1f;
        if (Input.GetKey(KeyCode.S)) z -= 1f;
        if (Input.GetKey(KeyCode.W)) z += 1f;

        if (Input.GetKey(KeyCode.J)) yawInput = -1f;
        if (Input.GetKey(KeyCode.L)) yawInput = 1f;

        moveInput = new Vector3(x, y, z).normalized;
    }

    private void FixedUpdate()
    {
        Vector3 move = moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        if (Mathf.Abs(yawInput) > 0.01f)
        {
            Quaternion delta = Quaternion.Euler(0f, yawInput * rotateSpeed * Time.fixedDeltaTime, 0f);
            rb.MoveRotation(rb.rotation * delta);
        }
    }
}
