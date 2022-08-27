using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    public float gravity = -9.81f / 2f;
    public float jumpHeight = 3f;
    public float glideSpeed = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Vector3 velocity;
    bool isGrounded;
    bool gliding = false;

  //  Animator animator;

    private float turnSmoothVelocity;

    public Transform chController;
    bool inside = false;
    public float speedUpDown = 3.2f;

    // Start is called before the first frame update
    void Start()
    {
       // Cursor.lockState = CursorLockMode.Locked;
        //animator = GetComponent<Animator>();

        //Scarab Climb
        inside = false;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        bool jump = Input.GetButtonDown("Jump");
        bool forward;
        //If player is moving, play forward animation, if not, don't.
        if ((Input.GetAxisRaw("Vertical") != 0) || (Input.GetAxisRaw("Horizontal") != 0))
        {
            forward = true;
        }
        else
        {
            forward = false;
        }

        //check if the Player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (inside == true && Input.GetKey("w"))
        {
            velocity.y = jumpHeight * 2f;
            //chController.transform.position += Vector3.up / speedUpDown;
            Debug.Log("Climbing Up");
            controller.Move(velocity * Time.deltaTime);
        }

        if (inside == true && Input.GetKey("s"))
        {
            velocity.y = -1 * (jumpHeight * 2f);
            //chController.transform.position += Vector3.down / speedUpDown;
            Debug.Log("Climbing Down");
            if (isGrounded)
            {
                velocity.z = -1 * (jumpHeight * 2f);
            }
            controller.Move(velocity * Time.deltaTime);
        }



        if (inside == false)
        {
            //Animations
          //  animator.SetBool("Forward", forward);
           // animator.SetBool("Jump", jump);

            //Set velocity to -2 if grounded
            if (isGrounded && velocity.y < 0)
            {
                gliding = false;
                velocity.y = -2f;
            }

            //Phoenix Glide
            if (Input.GetButtonDown("Jump") && !isGrounded && !gliding && velocity.y < -glideSpeed)
            {
                gliding = true;
                velocity.y = -glideSpeed;
            }

            //Apply gravity and velocity of gravity to the Player
            if (!gliding)
            {
                velocity.y += 4f * gravity * Time.deltaTime;
            }

            //Let the Player jump if grounded
            if (Input.GetButton("Jump") && isGrounded /*&& !Input.GetButton("Descend")*/)
            {
                Debug.Log("Normal jump");
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            //Ouroboros Jumpdash
            if (Input.GetButton("Jump") && isGrounded /*&& Input.GetButton("Descend")*/)
            {
                Debug.Log("Jumpdash");
                velocity.y = Mathf.Sqrt(jumpHeight * -8f * gravity);
                speed = 36f;

            }
            else if (isGrounded)
            {
                speed = 6f;
            }
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


}