using System;
using Cinemachine;
using UnityEngine;

public class CameraAutomaticFollowDollyPath : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float speed = 0.03f;

    private CinemachineTrackedDolly dolly;
    private float pathPosition = 0f;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        if (virtualCamera != null)
            dolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
    }

    void Update()
    {
        if (dolly is not null)
        {
            pathPosition += speed * Time.deltaTime;
            dolly.m_PathPosition = pathPosition;
        }
    }
}
