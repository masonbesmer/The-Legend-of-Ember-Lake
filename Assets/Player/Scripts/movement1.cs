using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class movement1 : MonoBehaviour
{
    public CharacterController playerControler;
    public Transform mainCamera;

    [SerializeField] float speed;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpHeight = 4f;
    [SerializeField] float slowRotation;

    Vector3 velocity;
    bool isGrounded;

    public Animator anim;
    float turnSmoothTime = .1f;
    float currentSmoothAngleVelocity;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Move();
    }

    void Move()
    {

        isGrounded = playerControler.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }

        float horizontal = -Input.GetAxisRaw("Horizontal");
        float vertical = -Input.GetAxisRaw("Vertical");

        var direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.sqrMagnitude >= .1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float smoothTargetAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle + mainCamera.eulerAngles.y, ref currentSmoothAngleVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0, smoothTargetAngle, 0);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            playerControler.Move(moveDir * speed * Time.deltaTime);
           
        }


            velocity.y += gravity * Time.deltaTime;
            playerControler.Move(velocity * Time.deltaTime);
    }
}
