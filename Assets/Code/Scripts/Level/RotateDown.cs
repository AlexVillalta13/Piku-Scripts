using System;
using UnityEngine;

public class RotateDown : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = Vector3.zero;

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position;
            // transform.rotation = Quaternion.identity;
            // transform.SetLocalPositionAndRotation(target.position, Quaternion.Euler(90f, 0f, 0f));
            transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        }
    }
}
