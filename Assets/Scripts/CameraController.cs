using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButton(2))
        {
            RotateCamera();
        }

        if (Input.GetKey(KeyCode.Backspace))
        {
            ResetCamera();
        }
    }

    private void RotateCamera()
    {
        float speed = 10.0f;

        // Smoothly tilts a transform towards a target rotation.
        float tiltAroundY = Input.GetAxis("Mouse X") * speed;
        transform.Rotate(0, tiltAroundY, 0);
    }
    private void ResetCamera()
    {
        transform.rotation = Quaternion.identity;
    }
}
