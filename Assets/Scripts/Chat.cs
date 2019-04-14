using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chat : MonoBehaviour
{
    public float Speed = 5;
    public float JumpForce = 5;

    private Rigidbody body;
    private new Camera camera;
    private bool isInAir;

    private Vector2 cameraRotation = new Vector2(0, 0);

    void Start()
    {
        body = GetComponent<Rigidbody>();
        camera = GetComponentInChildren<Camera>();
    }

    void Update() { }

    void FixedUpdate()
    {
        isInAir = !Physics.Raycast(transform.position, Vector3.down, 1.2f);
        if (Input.GetButtonDown("Jump") && !isInAir)
        {
            isInAir = true;
            body.AddForce(transform.up * JumpForce, ForceMode.VelocityChange);
        }

        Vector3 movement = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
        movement *= Speed;
        movement *= Time.fixedDeltaTime;

        transform.position += movement;

        cameraRotation = new Vector2(
            Mathf.Clamp(Input.GetAxis("Mouse Y") + cameraRotation.x, -85, 90),
            (Input.GetAxis("Mouse X") + cameraRotation.y) % 360
        );

        transform.rotation = Quaternion.Euler(0, cameraRotation.y, 0);
        camera.transform.localRotation = Quaternion.Euler(cameraRotation.x, 0, 0);
    }
}
