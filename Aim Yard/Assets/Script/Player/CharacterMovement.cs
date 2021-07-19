using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    bool isGrounded;

    float speed;
    float jumpForce;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (speed <= 0)
            speed = 4f;

        if (jumpForce <= 0)
            jumpForce = 3f;

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    public void Movement()
    {
        //Input
        float x = Input.GetAxisRaw("Horizontal") * speed;
        float y = Input.GetAxisRaw("Vertical") * speed;

        //Move
        Vector3 movePos = transform.right * x + transform.forward * y;
        Vector3 newMove = new Vector3(movePos.x, rb.velocity.y, movePos.z);
        rb.velocity = newMove;

        //Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
            rb.velocity = new Vector3(rb.velocity.z, jumpForce, rb.velocity.z);

    }

    //Ground Check
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
            isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
            isGrounded = false;
    }
}
