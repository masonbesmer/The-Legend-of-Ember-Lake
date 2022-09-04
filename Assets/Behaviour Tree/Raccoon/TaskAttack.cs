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
    private float attackRate;
    private float lastAttackedTime;
    private PlayerHealth playerHealth;
    private int attackDamage;

    public TaskAttack(NavMeshAgent navAgent, Transform targetTransform, Transform objectTransform, float attackRange, float attackRate, int attackDamage)
    {

        this.lastAttackedTime = 999f;
        this.attackRate = attackRate;
        this.navAgent = navAgent;
        this.objectTransform = objectTransform;
        this.targetTransform = targetTransform;
        this.attackRange = attackRange;
        this.attackDamage = attackDamage;

        playerHealth = targetTransform.GetComponent<PlayerHealth>();
  
        defaultPosition = objectTransform.position;
        animator = objectTransform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        Debug.Log("Attacking is being run");
        bool canAttack = Mathf.Abs(Time.time - lastAttackedTime) >= attackRate;

        if (canAttack && ExtensionMethodsBT.GetDistance(ExtensionMethodsBT.GetXZVector(objectTransform.position),ExtensionMethodsBT.GetXZVector(targetTransform.position)) <= attackRange)
        {

            lastAttackedTime = Time.time;
            animator.SetTrigger("isAttacking");
            playerHealth.TakeDamage(attackDamage);
            navAgent.isStopped = true;
            return NodeState.RUNNING;
        }
        else
        {
            navAgent.isStopped = false;
            return NodeState.FAILURE;
        }
        // Debug.Log("Idling around" + id);
       // nodeState = NodeState.SUCCESS;
    }
}

