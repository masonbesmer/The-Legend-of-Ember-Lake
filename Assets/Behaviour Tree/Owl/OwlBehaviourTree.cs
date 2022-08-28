using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlBehaviourTree : MonoBehaviour
{
    [SerializeField] float attackRange;
    Player player;

    Animator animator;

    private bool hasEntered;
    private bool hasExited;
    private Vector3 dockPosition;
    private Vector3 flyPosition;
    private Vector3 backPosition;
    private Vector3 firstPosition, middlePosition;
    private Vector3 randomPosition;

    private bool lookPlayer;
    private bool chasing;
    private bool positionReach;
    private float counter;

    private float attackedTime = 999f;
    private float attackedOffset = 5f;
    private void Awake()
    {
        counter = 0;
        //   dockPosition = default;
        lookPlayer = true;
        player = FindObjectOfType<Player>();
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Fly();

    }

    void Fly()
    {
        //if(positionReach)
        if(lookPlayer) transform.LookAt(player.transform);

        if (ExtensionMethodsBT.GetDistance(ExtensionMethodsBT.GetXZVector(player.transform.position), ExtensionMethodsBT.GetXZVector(transform.position)) <= attackRange && !hasEntered)
        {
            Debug.Log("Player in range");
            flyPosition = transform.position + (Vector3.up * 8f);
            hasEntered = true;
            animator.SetBool("fly", true);
        }
        else if (ExtensionMethodsBT.GetDistance(ExtensionMethodsBT.GetXZVector(player.transform.position), ExtensionMethodsBT.GetXZVector(transform.position)) >= attackRange && !hasExited && hasEntered)
        {
            Debug.Log("Player exiting range");
            hasExited = true;
        }

        if (hasExited && hasEntered)
        {
            hasEntered = false;
            Physics.Raycast(new Ray(transform.position, Vector3.down), out RaycastHit hitInfo, 10f);

            if (hitInfo.collider != null)
            {
                dockPosition = hitInfo.point;
            }
        }

        if (hasExited && !chasing)
        {
            if (dockPosition != null)
            {
                Debug.Log("Docking");
                transform.position = Vector3.MoveTowards(transform.position, dockPosition, Time.deltaTime * 5.0f);

                if (transform.position == dockPosition)
                {
                    animator.SetBool("fly", false);
                    hasExited = false;
                }
            }
        }

        if (hasEntered && !chasing && Mathf.Abs(Time.time - attackedTime) >= attackedOffset)
        {
            Debug.Log("Flying up");
            hasExited = false;
            transform.position = Vector3.MoveTowards(transform.position, flyPosition, Time.deltaTime * 5.0f);

            if(transform.position == flyPosition && !chasing)
            {
                lookPlayer = false;
                Debug.Log("Chasing");
                counter = 0;
                Vector3 direction = (player.transform.position - transform.position).normalized;
                backPosition = transform.position + (direction * 25) + (Vector3.up * 5);
                firstPosition = transform.position;
                middlePosition = player.transform.position;
                chasing = true; 
            }
        }

        if (chasing)
        {
            if(transform.position == backPosition && !positionReach)
            {
                randomPosition = Random.insideUnitSphere * 10f;
                attackedTime = Time.time;
                counter = 0;
                positionReach = true;
                Debug.Log("Pos reach");
            }
            else
            {
                if (positionReach)
                {
                    counter = (counter + Time.deltaTime * .5f);
                    transform.position = CubicBezierCurve(backPosition, middlePosition + randomPosition, firstPosition, counter);
                    Debug.Log("Position reach");
                    transform.LookAt(transform.forward);
                    if(transform.position == firstPosition)
                    {
                        positionReach = false;
                        chasing = false;
                        lookPlayer = true;
                    }
                }
                else
                {
                    counter = (counter + Time.deltaTime * 2.0f);
                    transform.LookAt(player.transform);
                    transform.position = CubicBezierCurve(firstPosition, middlePosition, backPosition, counter);
                }
            }
          //  counter %= 100;
        }
    }

    Vector3 CubicBezierCurve(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 p1 = Vector3.Lerp(a, b, t);
        Vector3 p2 = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(p1, p2, t);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
