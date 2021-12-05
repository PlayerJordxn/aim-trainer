using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharcterCamera : MonoBehaviour
{
    public static CharcterCamera instance;

    [SerializeField] Slider _slider;
    [SerializeField] Text sliderText;

    [SerializeField] Transform playerCamera;
    [SerializeField] float mouseSensitivity;
    [SerializeField][Range(0.0f, 1.0f)] float mouseSmoothValue = 0.5f;

    Vector2 mouseDirection = Vector2.zero;
    Vector2 mouseDirectionVelocity = Vector2.zero;

    float cameraPitch;
    bool lockCursor;
    float playerSpeed;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        mouseSensitivity = PlayerPrefs.GetFloat("Sensititvty");

        if (mouseSensitivity == 0)
            _slider.value = 5;

        if(mouseSensitivity <= 0)
            mouseSensitivity = 5;

        if (playerSpeed <= 0)
            playerSpeed = 6f;
    }

    public void ValueChangeCheck()
    {
        sliderText.text = mouseSensitivity.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
        PlayerPrefs.SetFloat("Sensitivity", mouseSensitivity);
        sliderText.text = mouseSensitivity.ToString();
        mouseSensitivity = _slider.value;
        

        if(playerCamera)
        UpdateMouseLook();
    }

    void UpdateMouseLook()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));//Vector 2 because the camera does not move, the parent does

        mouseDirection = Vector2.SmoothDamp(mouseDirection, mouseDelta, ref mouseDirectionVelocity, mouseSmoothValue);//Smooths mouse movement

        cameraPitch -= mouseDelta.y * mouseSensitivity / 3;//Sensitivity for camera left and right
        
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);// Clamps the camera at 90/-90 degrees

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;// Rotate on the right vector using camera Pitch

        transform.Rotate(Vector3.up * mouseDelta.x * mouseSensitivity / 3); //Rotate on x-axis using mouseDelta
    }
}
