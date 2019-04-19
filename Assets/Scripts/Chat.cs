using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chat : MonoBehaviour
{
    public float Speed = 5;
    public float JumpForce = 5;
    public float Gravity = -20;

    private CharacterController charaController;
    private new Camera camera;
    private float verticalVelocity;
    private Vector2 cameraRotation = new Vector2(0, 0);

    void Start()
    {
        charaController = GetComponent<CharacterController>();
        camera = GetComponentInChildren<Camera>();

        Vector3 initial = transform.rotation.eulerAngles;
        if (initial != Vector3.zero)
            cameraRotation = new Vector2(initial.x, initial.y);
    }

    void FixedUpdate()
    {
        // movement stuff
        Vector3 movement = 
            transform.forward * Input.GetAxis("Vertical") + 
            transform.right * Input.GetAxis("Horizontal");
        movement *= Speed;

        // because CharacterController#isGrounded is buggy
        bool isGroundedForReal = Physics.Raycast(transform.position, -transform.up, 1.2f);

        if (Input.GetButtonDown("Jump") && isGroundedForReal)
        {
            verticalVelocity = JumpForce;
        }
        else if (isGroundedForReal)
            verticalVelocity = 0;
        else 
            verticalVelocity += Gravity * Time.fixedDeltaTime; // mult by time cause gravity is in m/s²

        movement += transform.up * verticalVelocity;
        charaController.Move(movement * Time.fixedDeltaTime);

        // Camera and player rotation
        cameraRotation = new Vector2(
            Mathf.Clamp(Input.GetAxis("Mouse Y") + cameraRotation.x, -85, 90),
            (Input.GetAxis("Mouse X") + cameraRotation.y) % 360
        );
        transform.rotation = Quaternion.Euler(0, cameraRotation.y, 0);
        camera.transform.localRotation = Quaternion.Euler(cameraRotation.x, 0, 0);
    }
}
