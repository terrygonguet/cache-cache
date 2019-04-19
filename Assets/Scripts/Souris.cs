using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Souris : MonoBehaviour
{
    public float Speed = 8;
    public float JumpForce = 8;
    public float CameraDistance = 7;
    public float Gravity = -20;

    private CharacterController charaController;
    private new Camera camera;
    private float verticalVelocity;
    private Vector2 cameraRotation = new Vector2(20, 0);
    private Animator anim;
        
    void Start() {
        charaController = GetComponent<CharacterController>();
        camera = GetComponentInChildren<Camera>();
        anim = GetComponentInChildren<Animator>();

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

        if (Input.GetButtonDown("Jump") && charaController.isGrounded)
        {
            verticalVelocity = JumpForce;
            anim.SetTrigger("Jump");
        }
        else
        {
            // reset if character is touching ground but still apply 1 frame of gravity
            if (charaController.isGrounded)
                verticalVelocity = 0;
            verticalVelocity += Gravity * Time.fixedDeltaTime; // mult by time cause gravity is in m/s²
        }

        movement += transform.up * verticalVelocity;
        charaController.Move(movement * Time.fixedDeltaTime);

        // Camera and player rotation
        cameraRotation = new Vector2(
            Mathf.Clamp(Input.GetAxis("Mouse Y") + cameraRotation.x, -5, 80),
            (Input.GetAxis("Mouse X") + cameraRotation.y) % 360
        );
        CameraDistance = Mathf.Clamp(Input.GetAxis("Mouse ScrollWheel") + CameraDistance, 1.5f, 10);
        camera.transform.localPosition = Quaternion.Euler(cameraRotation.x, 0, 0) * (Vector3.forward * -CameraDistance);
        transform.rotation = Quaternion.Euler(0, cameraRotation.y, 0);
        camera.transform.LookAt(transform.position + transform.up);

        //Set animation
        Vector2 horizontalVelocity = new Vector2(charaController.velocity.x, charaController.velocity.z);
        anim.SetFloat("Speed", horizontalVelocity.magnitude);
    }
}
