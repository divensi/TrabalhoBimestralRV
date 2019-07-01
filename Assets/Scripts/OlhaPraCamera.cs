using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OlhaPraCamera : MonoBehaviour
{
    private Vector3 targetPoint;
    
    void Start()
    {
    }
    
    void Update()
    {
        targetPoint = Camera.main.transform.position;
        targetPoint.y = transform.position.y;
        transform.LookAt(targetPoint, Vector3.up);
    }
}
