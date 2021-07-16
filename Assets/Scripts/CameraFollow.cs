using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target = null;
    private Vector3 offset;
    void Start()
    {
        if (target != null)
        {
            offset = transform.position - target.position;
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, target.position.y, target.position.z) + offset, Time.deltaTime * 3);
        }
    }
}
