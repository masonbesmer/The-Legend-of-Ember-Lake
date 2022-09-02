using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
public class TaskChase : Node
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

    public TaskChase(NavMeshAgent navAgent,Rigidbody rigidbody, Transform targetTransform, Transform objectTransform, float chaseRange, float returnRange)
    {
        this.targetTransform = targetTransform;
        this.objectTransform = objectTransform;
        
        defaultPosition = objectTransform.position;
        animator = this.objectTransform.GetComponent<Animator>();
        this.rigidbody = rigidbody;
        this.navAgent = navAgent;
        this.returnRange = returnRange;
        this.chaseRange = chaseRange;
    }
    public override NodeState Evaluate()
    {
        /*    if (navAgent.hasPath)
                navAgent.acceleration = (navAgent.remainingDistance <= chaseRange) ? deceleration : acceleration;
    */
        if (ExtensionMethodsBT.GetDistance(ExtensionMethodsBT.GetXZVector(defaultPosition), ExtensionMethodsBT.GetXZVector(targetTransform.position)) <= chaseRange)
        {
            Debug.Log("Chasing player");
            wasChasing = true;
            animator.SetBool("isRunning", true);
            navAgent.SetDestination(targetTransform.position);
            return NodeState.RUNNING;
        }

        return NodeState.FAILURE;
    }
}
