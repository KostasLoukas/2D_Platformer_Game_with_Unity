using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Camera mainCamera;

    float shakeAmount = 0;

    void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }


    public void Shake(float amount, float length)
    {
        shakeAmount = amount;
        InvokeRepeating("DoShake", 0, 0.01f);  //Invoking (calling) a method immediately and repeating that after 0.01 seconds
        Invoke("StopShake", length);
    }


    void DoShake()
    {
        if (shakeAmount > 0)
        {
            Vector3 cameraPosition = mainCamera.transform.position;

            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;  //just a random calculation for camera shake found on the internet
            float offsetY = Random.value * shakeAmount * 2 - shakeAmount;
            cameraPosition.x += offsetX;
            cameraPosition.y += offsetY;

            mainCamera.transform.position = cameraPosition;
        }
    }


    void StopShake()
    {
        CancelInvoke("DoShake");
        mainCamera.transform.localPosition = Vector3.zero;
    }
}
