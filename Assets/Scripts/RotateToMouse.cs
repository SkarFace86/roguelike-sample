using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMouse : MonoBehaviour
{
    Vector3 moveDirection;
    float angle;

    // Update is called once per frame
    void Update()
    {
        moveDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        moveDirection.z = 0;
        moveDirection.Normalize();
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
