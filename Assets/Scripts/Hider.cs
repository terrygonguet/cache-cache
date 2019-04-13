using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hider : MonoBehaviour
{
    public float Speed = 10;
    public float JumpForce = 100;
    public float CameraDistance = 7;

    private Rigidbody body;
    private new Camera camera;
    private Vector2 cameraRotation = new Vector2(20, 0);
        
    void Start() {
        body = GetComponent<Rigidbody>();
        camera = GetComponentInChildren<Camera>();
    }
    
    void Update()
    {
        Vector3 move = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
        move *= Speed * Time.deltaTime;
        move.y = body.velocity.y;

        body.velocity = move;

        if (Input.GetButtonDown("Jump"))
        {
            body.AddForce(0, JumpForce, 0);
        }

        CameraDistance = Mathf.Clamp(Input.GetAxis("Mouse ScrollWheel") + CameraDistance, 1, 10);

        cameraRotation = new Vector2(
            Mathf.Clamp(Input.GetAxis("Mouse Y") + cameraRotation.x, -5, 40),
            (Input.GetAxis("Mouse X") + cameraRotation.y) % 360
        );

        camera.transform.localPosition = Quaternion.Euler(cameraRotation.x, 0, 0) * new Vector3(0, 1, -CameraDistance);
        transform.rotation = Quaternion.Euler(0, cameraRotation.y, 0);
        camera.transform.LookAt(transform, Vector3.up);
    }
}
