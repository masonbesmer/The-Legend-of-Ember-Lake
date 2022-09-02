using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class movement2 : MonoBehaviour
{
    public CharacterController pc;
    public Transform cam;

    [SerializeField] float speed;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpHeight = 4f;

    Vector3 velocity;
    bool isGrounded;

    public Animator anim;


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

        float x = -Input.GetAxisRaw("Horizontal");
        float z = -Input.GetAxisRaw("Vertical");

        if (z == 0 || x == 0)
        {
            anim.SetFloat("Blend", 0);
        }

        if (z == 1 || z == -1)
        {
            anim.SetFloat("Blend", 0.33f);
        }

        if (x == 1 || x == -1)
        {
            anim.SetFloat("Blend", 0.33f);
        }


        Vector3 move = transform.right.normalized * x + transform.forward.normalized * z;

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        pc.Move(move * speed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        pc.Move(velocity * Time.deltaTime);

        transform.LookAt(new Vector3(cam.position.x, transform.position.y, cam.position.z));


    }
}
