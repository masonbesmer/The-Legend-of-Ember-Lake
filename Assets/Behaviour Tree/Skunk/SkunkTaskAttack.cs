using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkunkTaskAttack : Node
{
    private Transform objectTransform;
    private Transform targetTransform;

    private Animator animator;
    private float attackRange;
    private NavMeshAgent navAgent;

    private bool isAttacking;
    private float attackedTime = 9999f;
    private float attackOffset = 2f;
    private bool canAttack = false;
    public SkunkTaskAttack(NavMeshAgent navAgent, Transform targetTransform, Transform objectTransform, float attackRange)
    {
        Debug.Log("Initialized");
        //  this.attackedTime = 9999f;
        // this.canAttack = false;
        this.isAttacking = false;
        this.navAgent = navAgent;
        this.objectTransform = objectTransform;
        this.targetTransform = targetTransform;
        this.attackRange = attackRange;
        animator = objectTransform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {

        if (ExtensionMethodsBT.GetDistance(ExtensionMethodsBT.GetXZVector(objectTransform.position), ExtensionMethodsBT.GetXZVector(targetTransform.position)) > attackRange)
        {
            navAgent.isStopped = false;
            //   animator.SetBool("isAttacking", false);
            return NodeState.FAILURE;
        }

        // Debug.Log("Attacking is being run");

        if (attackedTime >= attackOffset)
        {
            attackedTime = 0;
            canAttack = true;
        }
        else
        {
            attackedTime += Time.deltaTime;
        }

        if (canAttack)
        {
            canAttack = false;
            objectTransform.LookAt(targetTransform.position);
            animator.SetTrigger("isAttacking");
            navAgent.isStopped = true;
        }

        return NodeState.RUNNING;
    }
}
