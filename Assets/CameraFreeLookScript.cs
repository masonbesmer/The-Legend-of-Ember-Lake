using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraFreeLookScript : MonoBehaviour
{
    CinemachineFreeLook cameraLook;
    // Start is called before the first frame update
    void Start()
    {
        cameraLook = GetComponent<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    { 
        cameraLook.m_YAxis.Value = .91f;
    }
}
