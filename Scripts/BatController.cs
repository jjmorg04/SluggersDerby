using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour {
    // speed of bat movement
    public float batSpeed = 10f;
    
    // speed of bat swing
    public float swingSpeed = 200f;
    
    // max bat swing angle
    public float maxSwingAngle = 45f;
    
    // angle of bat as is
    private float batAngle = 35f;
    
    // max transform values to keep bat confined in left side of screen
    public float minXConf = -9f;
    public float maxXConf = -3f;
    public float minYConf = -5f; 
    public float maxYConf = 5f;

    // 
    void FixedUpdate() {
        
        MoveBatWithMouse();

        // if left click, swing bat
        if (Input.GetMouseButton(0)) {
            SwingBat();
        }

        // if no left click, return bat to original angle
        else {
            ResetBatSwing();
        }
    }

    // function for bat movement with mouse
    void MoveBatWithMouse() {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // confine bat movement
        float clampedX = Mathf.Clamp(mousePosition.x, minXConf, maxXConf);
        float clampedY = Mathf.Clamp(mousePosition.y, minYConf, maxYConf);
        
        // based on confined values, bat moves with mouse
        transform.position = new Vector3(clampedX, clampedY, 0);
    }

    void SwingBat() {
        // increase bat angle with speed and time
        batAngle += swingSpeed * Time.fixedDeltaTime;

        // confine bat swing to max angle
        if (batAngle > maxSwingAngle) {
            batAngle = maxSwingAngle;
        }
        if (batAngle < -maxSwingAngle) {
            batAngle = -maxSwingAngle;
        }

        // apply new angle to bat
        transform.rotation = Quaternion.Euler(0, 0, -batAngle);
    }

    void ResetBatSwing() {
        // reset bat angle
        batAngle = 0;
        transform.rotation = Quaternion.Euler(0, 0, 35f);
    }
}





