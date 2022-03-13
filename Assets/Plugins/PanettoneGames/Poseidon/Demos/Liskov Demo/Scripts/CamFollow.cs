using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;// = new Vector3(0, 5, -10);
    [SerializeField] float smoothTime = 0.3F;

    private Vector3 velocity = Vector3.zero;

    private void Start() => offset = transform.position - target.position;

    void FixedUpdate()
    {
        Vector3 targetPosition = target.TransformPoint(offset);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        transform.LookAt(target);
    }
}
