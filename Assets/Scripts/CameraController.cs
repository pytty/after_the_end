using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 cameraHome;

    private void Awake()
    {
        cameraHome = Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            RotateCamera();
        }
        else if (Input.GetMouseButton(2))
        {
            PanCamera();
        }

        if (Input.GetKey(KeyCode.Backspace))
        {
            ResetCamera();
        }

        if (Input.mouseScrollDelta.magnitude > 0)
            ZoomCamera();
    }

    private void RotateCamera()
    {
        float speed = 8.0f;

        float tiltAroundY = Input.GetAxis("Mouse X") * speed;
        float tiltAroundX = Input.GetAxis("Mouse Y") * -speed;
        // Do I have to use time.deltatime
        transform.localEulerAngles += new Vector3(tiltAroundX, tiltAroundY);
    }
    private void PanCamera()
    {
        float speed = 0.5f;

        //Do I have to use time.deltatime
        Vector3 mouseInput = new Vector3(-Input.GetAxis("Mouse X"), 0.0f, -Input.GetAxis("Mouse Y"));
        Vector3 newZDirection = new Vector3(transform.forward.x, 0.0f, transform.forward.z).normalized;
        Vector3 newXDirection = -Vector3.Cross(newZDirection, Vector3.up).normalized;
        Vector3 finalWorldMovement = newXDirection * mouseInput.x + newZDirection * mouseInput.z;


        transform.position += (finalWorldMovement * speed);
        if (mouseInput.magnitude != 0)
            print(mouseInput.magnitude / finalWorldMovement.magnitude);
        //transform.Translate(new Vector3(-Input.GetAxis("Mouse X"), 0.0f, -Input.GetAxis("Mouse Y")) * speed);
    }

    private void ZoomCamera()
    {
        float speed = 0.15f;
        Vector3 currentPosition = Camera.main.transform.position;
        Vector3 AB = -currentPosition + transform.position;
        Camera.main.transform.position += AB * Input.mouseScrollDelta.y * speed;

    }

    public void ResetCamera()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        Camera.main.transform.position = cameraHome;
    }
}
