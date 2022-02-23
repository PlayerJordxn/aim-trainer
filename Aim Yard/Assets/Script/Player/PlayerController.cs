using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public CharacterController cc;
    public GameObject spine001;

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
    [SerializeField] private AudioSource sandAudioSource;
    [SerializeField] private AudioClip sandAudioClip;

    [SerializeField] private AudioSource woodAudioSource;
    [SerializeField] private AudioClip woodAudioClip;

    private bool onSand = false;
    private bool onWood = false;


    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if(instance != null)
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (!cc)
            cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        WalkingAudio();
        //>> CAMERA MOVEMENT <<//
        //Input
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        float lookUp = mouseDelta.y * cameraRotationSpeed * Time.deltaTime;
        float lookLeft = mouseDelta.x * cameraRotationSpeed * Time.deltaTime;

        //Rotation
        rotationX -= lookUp;
        rotationX = Mathf.Clamp(rotationX, -90, 90);
        transform.Rotate(transform.eulerAngles.x, lookLeft, transform.eulerAngles.z);

        //New Rotation
        Vector3 mouseMovement = new Vector3(rotationX, transform.eulerAngles.y, transform.eulerAngles.z);
        spine001.transform.eulerAngles = mouseMovement;
        mouseMovement = Vector2.SmoothDamp(mouseMovement, mouseDelta, ref mouseDirectionVelocity, cameraSmoothValue);

        //>> PLAYER MOVEMENT <<//
        //Gravity
        verticalMovement -= gravity * Time.deltaTime;

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

        //Jump
        if (cc.isGrounded && Input.GetKeyDown(KeyCode.Space))
            verticalMovement = jumpForce;

    }

    void WalkingAudio()
    {
        if (cc.velocity.x > 0.1f || cc.velocity.y > 0.1f)
        {
            if (onSand && !onWood)
            {
                woodAudioSource.PlayOneShot(woodAudioClip);
            }

            if (onWood && !onSand)
            {
                sandAudioSource.PlayOneShot(sandAudioClip);

            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Sand"))
        {

            print("COLLSION SAND");
            onSand = true;
            onWood = false;
        }

        if (collision.collider.CompareTag("Wood"))
        {
            print("COLLSION WOOD");
            onWood = true;
            onSand = false;
        }
    }
    void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.collider.CompareTag("Sand"))
        {

            print("COLLSION SAND");
            onSand = true;
            onWood = false;
        }

        if (collisionInfo.collider.CompareTag("Wood"))
        {
            print("COLLSION WOOD");
            onWood = true;
            onSand = false;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.collider.CompareTag("Sand"))
        {
            onSand = false;
        }

        if (other.collider.CompareTag("Wood"))
        {
            onWood = false;
        }
    }
}
