using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Searcher : MonoBehaviour
{
    public float Speed = 500;
    public float JumpForce = 400;

    private Rigidbody body;
    private new Camera camera;
    private Vector2 cameraRotation = new Vector2(0, 0);

    void Start()
    {
        body = GetComponent<Rigidbody>();
        camera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        //Move your ass in the fix update
        Vector3 move = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
        move *= Speed * Time.deltaTime;
        move.y = body.velocity.y;

        //body.MovePosition
        body.velocity = move;

        if (Input.GetButtonDown("Jump"))
        {
            body.AddForce(0, JumpForce, 0);
        }

        cameraRotation = new Vector2(
            Mathf.Clamp(Input.GetAxis("Mouse Y") + cameraRotation.x, -85, 85),
            (Input.GetAxis("Mouse X") + cameraRotation.y) % 360
        );

        transform.rotation = Quaternion.Euler(0, cameraRotation.y, 0);
        camera.transform.localRotation = Quaternion.Euler(cameraRotation.x, 0, 0);
    }
}
