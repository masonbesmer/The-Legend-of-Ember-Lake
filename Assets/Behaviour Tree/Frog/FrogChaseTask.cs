using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
using System.Threading.Tasks;
public class FrogChaseTask : Node
{
    
    private Transform objectTransform;
    private Transform targetTransform;

    private float deceleration = 60f, acceleration = 2f;

    private readonly Vector3 defaultPosition;
    private Animator animator;

    private float chaseRange;
    private float returnRange;
    private bool wasChasing;

    private NavMeshAgent navAgent;
    private Rigidbody rigidbody;

    private float jumpRange, raycastInBetweenDistance;
    private LayerMask obstacleMask;

    private float lastJumpTime;
    private float jumpOffset = .5f, jumpRate, lookRange, attackRange;
    private bool hasDirection;
    private Vector3 jumpDirection, frontPosition, middlePosition;
    float counter = 0;
    float moveCounter = 0;
  //  private Vector3 defaultPosition;

  //  private Vector3[] checkAheadList;

    public FrogChaseTask(Transform targetTransform, Transform objectTransform, float chaseRange, float jumpRange,float jumpRate,float lookRange, float attackRange, float raysInBetweenDistance, LayerMask obstacleMask)
    {
        this.attackRange = attackRange;
        this.lookRange = lookRange;
        jumpDirection = Vector3.positiveInfinity;
        //   defaultPosition = objectTransform.position;
        lastJumpTime = 999f;
        this.obstacleMask = obstacleMask;
        this.jumpRange = jumpRange;
        this.jumpRate = jumpRate;
        this.raycastInBetweenDistance = raysInBetweenDistance;
       // checkAheadList = new Vector3[] {objectTransform.};
        this.targetTransform = targetTransform;
        this.objectTransform = objectTransform;

        defaultPosition = objectTransform.position;
        animator = this.objectTransform.GetComponent<Animator>();
        this.chaseRange = chaseRange;
    }



    public override NodeState Evaluate()
    {
        bool isWithinChaseRange = ExtensionMethodsBT.GetDistance(ExtensionMethodsBT.GetXZVector(defaultPosition), ExtensionMethodsBT.GetXZVector(targetTransform.transform.position)) <= chaseRange;
        bool isWithinHome = ExtensionMethodsBT.GetDistance(ExtensionMethodsBT.GetXZVector(defaultPosition), ExtensionMethodsBT.GetXZVector(objectTransform.transform.position)) >= jumpRange;

        if (isWithinHome && !isWithinChaseRange && Mathf.Abs(lastJumpTime - Time.time) >= jumpRate && !hasDirection)
        {
            parent.SetData("isJumping", true);
            animator.SetBool("isWithinRange", !isWithinChaseRange);
            // animator.SetBool("isWithinRange", true);
            animator.SetBool("isRunning", true);
            lastJumpTime = Time.time;
            jumpDirection = objectTransform.position + (CheckObstacle(defaultPosition).normalized * jumpRange);
            frontPosition = objectTransform.position;
            middlePosition = GetMidpoint(objectTransform.position, jumpDirection) + (Vector3.up * 8.0f);
            hasDirection = true;
            return NodeState.SUCCESS;
        }
        else if (!isWithinChaseRange && hasDirection)
        {
            animator.SetBool("isWithinRange", isWithinHome);

            counter += Time.deltaTime;

            if (counter >= 2.0f)
            {
                counter = 0;
                hasDirection = false;
            }

            moveCounter = (moveCounter + Time.deltaTime);
            objectTransform.position = CubicBezierCurve(frontPosition, middlePosition, jumpDirection, moveCounter);

            if (Vector3.Distance(objectTransform.position, jumpDirection) <= 1f)
            {
                animator.SetBool("isWithinRange", isWithinHome);
                parent.SetData("isJumping", false);
                animator.SetBool("isRunning", false);
                moveCounter = 0;
                counter = 0;
                Debug.Log("Direction reached");
                hasDirection = false;
            }

            return NodeState.SUCCESS;
        }

        if (Vector3.Distance(objectTransform.position, targetTransform.position) >= attackRange || (hasDirection && isWithinChaseRange))
        {
            if (isWithinChaseRange && Mathf.Abs(lastJumpTime - Time.time) >= jumpRate && !hasDirection)
            {
                animator.SetBool("isWithinRange", isWithinChaseRange);
                parent.SetData("isJumping", true);

                // animator.SetBool("isWithinRange", true);
                animator.SetBool("isRunning", true);
                lastJumpTime = Time.time;
                jumpDirection = objectTransform.position + (CheckObstacle(targetTransform.position).normalized * jumpRange);
                frontPosition = objectTransform.position;
                middlePosition = GetMidpoint(objectTransform.position, jumpDirection) + (Vector3.up * 8.0f);
                hasDirection = true;
                return NodeState.SUCCESS;
            }
            else if (hasDirection && isWithinChaseRange)
            {
                animator.SetBool("isWithinRange", isWithinChaseRange);

                counter += Time.deltaTime;

                if (counter >= 2.0f)
                {
                    counter = 0;
                    hasDirection = false;
                }

                moveCounter = (moveCounter + Time.deltaTime);
                ///  Debug.Log("Move counter: " + moveCounter);
                objectTransform.position = CubicBezierCurve(frontPosition, middlePosition, jumpDirection, moveCounter);

                if (objectTransform.position == jumpDirection)
                {
                    parent.SetData("isJumping", false);
                    animator.SetBool("isRunning", false);
                    moveCounter = 0;
                    counter = 0;
                    Debug.Log("Direction reached");
                    hasDirection = false;
                }

                return NodeState.SUCCESS;
            }
        }

        return base.Evaluate();
    }

    Vector3 GetMidpoint(Vector3 p1, Vector3 p2)
    {
        return new Vector3((p1.x + p2.x) / 2, (p1.y + p2.y) / 2, (p1.z + p2.z) / 2);
    }

    Vector3 CheckObstacle(Vector3 target)
    {
        objectTransform.LookAt(target);
        
        Vector3 destination = Vector3.zero;
        Vector3[] checkAheadList = new Vector3[] {objectTransform.forward, (objectTransform.forward - (objectTransform.right * raycastInBetweenDistance)), (objectTransform.forward + (objectTransform.right * raycastInBetweenDistance)) };
        for (int i = 0; i < 3; i++)
        {
            Physics.Raycast(new Ray(objectTransform.position, checkAheadList[i].normalized), out RaycastHit hit, lookRange, obstacleMask);
          
            if (hit.collider != null)
            {
                continue;
            }
            else
            { 
                destination = checkAheadList[i];
               // objectTransform.LookAt(destination);
                break;
            }
        }
        return destination;
    }

    Vector3 CubicBezierCurve(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 p1 = Vector3.Lerp(a, b, t);
        Vector3 p2 = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(p1, p2, t);
    }
    /*
      Check obstacle ahead, use raycast 3-5 times before jumping ahead. 
      First check where the player is. If raycast is */

}
