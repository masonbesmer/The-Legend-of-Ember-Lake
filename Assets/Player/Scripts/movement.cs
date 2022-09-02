using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public CharacterController pc;

    [SerializeField] float speed;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpHeight = 4f;

    Vector3 velocity;
    bool isGrounded;

    private void Update()
    {
        move();
    }

    void move()
    {

        isGrounded = pc.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right.normalized * x + transform.forward.normalized * z;

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        pc.Move(move * speed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        pc.Move(velocity * Time.deltaTime);
    }
}
