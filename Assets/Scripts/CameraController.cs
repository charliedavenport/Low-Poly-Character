﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform target;
    public Camera mainCamera { get; private set; }
    public float mouseSensitivity = 150f;

    private float mouseX;
    private float mouseY;
    private float rotY;
    private float rotX;


    private void Awake()
    {
        mainCamera = GetComponentInChildren<Camera>();
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, 0.4f);
    }

    private void LateUpdate()
    {
        mouseY = -Input.GetAxis("Mouse Y");
        mouseX = Input.GetAxis("Mouse X");

        rotY += mouseX * mouseSensitivity * Time.deltaTime;
        rotX += mouseY * mouseSensitivity * Time.deltaTime;
        rotX = Mathf.Clamp(rotX, -80f, 80f);

        Quaternion cameraRot = Quaternion.Euler(rotX, rotY, 0f);
        transform.rotation = cameraRot;
    }


}