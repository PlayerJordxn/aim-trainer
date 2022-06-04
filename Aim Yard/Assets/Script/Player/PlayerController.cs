using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController cc;
    private Camera cam;
    [SerializeField] public Transform spine001;

    //Movement
    [Header("Movement Settings")]
    public float speed = 0;
    public float gravity = 9.81f;
    private float verticalMovement = 0f;
    public float jumpForce = 8f;

    [Header("Camera Settings")]
    [Range(0, 1)] public float cameraSmoothValue = 0.5f;
    public float cameraRotationSpeed = 5f;
    private Vector2 mouseDirection = Vector2.zero;
    private Vector2 mouseDirectionVelocity = Vector2.zero;
    private float rotationX = 0f;

    [Header("Audio")]
    [SerializeField] private AudioSource shootAudioSource;
    [SerializeField] private AudioClip shootAudioClip;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (!cam) cam = GetComponentInChildren<Camera>();
        if (!cc) cc = GetComponent<CharacterController>();
        if (!spine001) spine001 = GameObject.FindGameObjectWithTag("Spine").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Gravity();
        Look();
        Movement();
        Jump();
        Shoot();
    }

    public void Shoot()
    {
        RaycastHit hit;

        bool shootInput = Input.GetKeyDown(KeyCode.Mouse0);
        bool castHit = Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, 50f);

        if(shootInput)
        {
            shootAudioSource.Play();

            if (castHit)
            {
                if (GridshotManager.instance.gameState == GridshotManager.GameState.PLAYING)
                {
                    CanvasManager.instance.totalShots++;
                }

                if (hit.collider.CompareTag("Target"))
                {
                    GameObject gridshotTarget = hit.collider.gameObject;
                    ObjectPool.instance.ReturnTarget(gridshotTarget);
                    GridshotManager.instance.currentTargetCount--;
                    CanvasManager.instance.score++;
                }
            }
        }
       
    }

    public void Gravity()
    {
        verticalMovement -= gravity * Time.deltaTime;
    }
    public void Look()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        float lookUp = mouseInput.y * cameraRotationSpeed * Time.deltaTime;
        float lookLeft = mouseInput.x * cameraRotationSpeed * Time.deltaTime;

        //Rotation
        rotationX -= lookUp;
        rotationX = Mathf.Clamp(rotationX, -90, 90);
        transform.Rotate(transform.eulerAngles.x, lookLeft, transform.eulerAngles.z);

        //New Rotation
        Vector3 mouseMovement = new Vector3(rotationX, transform.eulerAngles.y, transform.eulerAngles.z);
        spine001.transform.eulerAngles = mouseMovement;
        mouseMovement = Vector2.SmoothDamp(mouseMovement, mouseInput, ref mouseDirectionVelocity, cameraSmoothValue);
    }

    public void Movement()
    {
        //Input
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        //Move
        Vector3 moveForward = transform.forward * verticalInput;
        Vector3 moveSide = transform.right * horizontalInput;

        //New Move + Jump
        Vector3 direction = (moveForward + moveSide).normalized * speed;
        direction.y = verticalMovement;
        Vector3 distance = direction * Time.deltaTime;
        cc.Move(distance);
    }

    public void Jump()
    {
        if (cc.isGrounded && Input.GetKeyDown(KeyCode.Space))
            verticalMovement = jumpForce;
    }
}
