using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class TaskAttack : Node
{
    private Transform objectTransform;
    private Transform targetTransform;
    private Vector3 defaultPosition;

    private Animator animator;
    private float attackRange;
    private NavMeshAgent navAgent;

    public TaskAttack(NavMeshAgent navAgent, Transform targetTransform, Transform objectTransform, float attackRange)
    {

        this.navAgent = navAgent;
        this.objectTransform = objectTransform;
        this.targetTransform = targetTransform;
        this.attackRange = attackRange;
  
        defaultPosition = objectTransform.position;
        animator = objectTransform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        Debug.Log("Attacking is being run");

        if (ExtensionMethodsBT.GetDistance(ExtensionMethodsBT.GetXZVector(objectTransform.position),ExtensionMethodsBT.GetXZVector(targetTransform.position)) <= attackRange)
        {
            animator.SetBool("isAttacking", true);
            navAgent.isStopped = true;
            return NodeState.RUNNING;
        }
        else
        {
            navAgent.isStopped = false;
            animator.SetBool("isAttacking", false);
            return NodeState.FAILURE;
        }
        // Debug.Log("Idling around" + id);
       // nodeState = NodeState.SUCCESS;
    }
}

