using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    CinemachineTransposer cam;

    void ChangeCameraToStatic()
    {
        cam.m_XDamping = 0;
        cam.m_YDamping = 0;
        cam.m_ZDamping = 0;
    }

    private void Awake()
    {
        if (ReferenceManager.Instance.StaticCamera)
        {
            cam = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>();

            ChangeCameraToStatic();
        }
    }
}
