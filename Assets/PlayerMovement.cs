using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] float speed = 12f;
    [SerializeField] float turnSmoothtime = 0.1f;
    [SerializeField] float gravity = -9.81f * 2f;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    [SerializeField] float turnSmoothVelocity;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //get input of movement buttons (X and Y)
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        //clamp input to 1
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        //self expalaining
        bool jump = Input.GetButtonDown("Jump");

        //generate bubble at the groundCheck position and see if it collides with something in the groundMask
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //let jump if grounded and jump button is pressed
        if (jump && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //move player based on shit above
        controller.Move(velocity * Time.deltaTime);
        
        //Player Movement Algorithm
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }
}
