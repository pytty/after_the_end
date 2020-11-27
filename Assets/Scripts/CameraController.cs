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
        //clamp to verticals
        //these values are hard coded, don't know their exact calculations (depends on camera's position)
        float max = 30.0f;
        float min = -45.0f;
        //convert "strictly positive" angle to "positive/negative" angle
        float xRot = transform.localEulerAngles.x;
        if (xRot > 270.0f)
            xRot -= 360.0f;
        //clamp and convert back to "strictly positive" angle
        xRot = Mathf.Clamp(xRot, min, max) + 360.0f;
        //set rotation to clamped rotation
        transform.localEulerAngles = new Vector3(
            xRot,
            transform.localEulerAngles.y,
            transform.localEulerAngles.z);
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
