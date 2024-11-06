using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    // target for camera to follow
    public Transform target;

    // speed of camera follow motion
    public float smoothSpeed = 1f;

    // camera component
    private Camera cam;

    // get camera
    void Start() {
        cam = GetComponent<Camera>();

    }

    // cam follow movement gets smoothed
    void LateUpdate() {
        if (target != null) {
            Vector3 camPosition = target.position;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, camPosition, smoothSpeed); // Smoothly move to the desired position
            transform.position = smoothedPosition;
        }
    }

    // set target for cam to follow (aka baseball)
    public void SetTarget(Transform newTarget) {
        target = newTarget;
        
        // zoom out cam after ball is hit and cam follows
        cam.orthographicSize = 10;
        
        // Debug.Log("Camera target set to ball");
    }
}

