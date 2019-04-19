using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Souris : MonoBehaviour
{
    public float Speed = 5;
    public float JumpForce = 5;
    public float CameraDistance = 7;

    private Rigidbody body;
    private new Camera camera;
    private bool isInAir;

    private Animator anim;

    private Vector2 cameraRotation = new Vector2(20, 0);
        
    void Start() {
        body = GetComponent<Rigidbody>();
        camera = GetComponentInChildren<Camera>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        CameraDistance = Mathf.Clamp(Input.GetAxis("Mouse ScrollWheel") + CameraDistance, 1.5f, 10);

        isInAir = !Physics.Raycast(transform.position, Vector3.down, 1.2f);
        if (Input.GetButtonDown("Jump") && !isInAir)
        {
            isInAir = true;
            body.AddForce(transform.up * JumpForce, ForceMode.VelocityChange);
        }

        Vector3 movement = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
        //Set animation
        if (movement.x == 0 || movement.z == 0)
        {
            anim.SetBool("run", false);
        }
        else
        {
            anim.SetBool("run", true);
        }


        movement *= Speed;
        movement *= Time.fixedDeltaTime;

        body.MovePosition(body.position + movement);

        cameraRotation = new Vector2(
            Mathf.Clamp(Input.GetAxis("Mouse Y") + cameraRotation.x, -5, 80),
            (Input.GetAxis("Mouse X") + cameraRotation.y) % 360
        );

        camera.transform.localPosition = Quaternion.Euler(cameraRotation.x, 0, 0) * (Vector3.forward * -CameraDistance);
        transform.rotation = Quaternion.Euler(0, cameraRotation.y, 0);
        camera.transform.LookAt(transform.position + transform.up);
    }
}
