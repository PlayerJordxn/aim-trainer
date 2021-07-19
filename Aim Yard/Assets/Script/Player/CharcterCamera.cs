using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharcterCamera : MonoBehaviour
{

    [SerializeField] Transform playerCamera;
    [SerializeField] float mouseSensitivity;
    [SerializeField][Range(0.0f, 1.0f)] float mouseSmoothValue = 0.5f;

    Vector2 mouseDirection = Vector2.zero;
    Vector2 mouseDirectionVelocity = Vector2.zero;

    float cameraPitch;
    bool lockCursor;
    float playerSpeed;

    // Start is called before the first frame update
    void Start()
    {

        lockCursor = true;

        if(lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;//Locks cursor at the center of the screen
            Cursor.visible = false;//Makes cursor invisable
        }

        if(mouseSensitivity <= 0)
        {
            mouseSensitivity = 5.0f;
        }

        if (playerSpeed <= 0)
        {
            playerSpeed = 6f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMouseLook();
    }

    void UpdateMouseLook()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));//Vector 2 because the camera does not move, the parent does

        mouseDirection = Vector2.SmoothDamp(mouseDirection, mouseDelta, ref mouseDirectionVelocity, mouseSmoothValue);//Smooths mouse movement

        cameraPitch -= mouseDelta.y * mouseSensitivity;//Sensitivity for camera left and right
        
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);// Clamps the camera at 90/-90 degrees

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;// Rotate on the right vector using camera Pitch

        transform.Rotate(Vector3.up * mouseDelta.x * mouseSensitivity); //Rotate on x-axis using mouseDelta
    }


}
