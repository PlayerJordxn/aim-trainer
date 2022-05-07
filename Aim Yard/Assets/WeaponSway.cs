using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * 4f;
        float mouseY = Input.GetAxisRaw("Mouse Y") * 4f;

        Quaternion rotationX = Quaternion.AngleAxis(mouseX, Vector3.up);
        Quaternion rotationY = Quaternion.AngleAxis(-mouseY, Vector3.right);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, 8f * Time.deltaTime);


    }
}
