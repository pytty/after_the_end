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
        float speed = 10.0f;

        float tiltAroundY = Input.GetAxis("Mouse X") * speed;
        //float tiltAroundX = Input.GetAxis("Mouse Y") * -speed;
        // Do I have to use time.deltatime
        transform.Rotate(0, tiltAroundY, 0);
        //transform.Rotate(tiltAroundX, tiltAroundY, 0);
    }
    private void PanCamera()
    {
        float speed = 0.5f;

        //Do I have to use time.deltatime
        transform.Translate(new Vector3(-Input.GetAxis("Mouse X"), 0.0f, -Input.GetAxis("Mouse Y")) * speed);
    }

    private void ZoomCamera()
    {
        float speed = 0.15f;
        Vector3 currentPosition = Camera.main.transform.position;
        Vector3 AB = -currentPosition + transform.position;
        Camera.main.transform.position += AB * Input.mouseScrollDelta.y * speed;

    }

    private void ResetCamera()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        Camera.main.transform.position = cameraHome;
    }
}
