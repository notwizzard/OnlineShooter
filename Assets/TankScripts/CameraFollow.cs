using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform Target;
    private Transform Offset;
    [SerializeField] private float normalSmoothTime = 0.3f;
    /*[SerializeField] private float minDistance;
    [SerializeField] private float smoothRate;*/

    private Vector3 velocity = Vector3.zero;
    private float SmoothTime;

    private void Start()
    {
        SmoothTime = normalSmoothTime;
        Target = transform.root;
        Offset = transform.parent;
        transform.SetParent(null);
    }

    private void FixedUpdate()
    {
        Vector3 targetPosition = Offset.position;

        /*if ((transform.position - targetPosition).magnitude <= minDistance)
        {
            SmoothTime = normalSmoothTime / smoothRate;
        }
        else
        {
            SmoothTime = normalSmoothTime;
        }*/

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, SmoothTime);
        transform.LookAt(Target);
    }
}
