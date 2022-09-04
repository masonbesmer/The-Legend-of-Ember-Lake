using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
//using Tree = BehaviourTree.Tree;
public class MosquitoAttack : Node
{
    private Transform objectTransform;
    private Transform targetTransform;
    private Vector3 defaultPosition;

    private Animator animator;
    private float attackRange;

    private int attackDamage;

    private float lastAttackedTime, attackRate;

    private PlayerHealth playerHealth;

    public MosquitoAttack( Transform targetTransform, Transform objectTransform, Animator animator, float attackRange, float attackRate, int attackDamage)
    {
        lastAttackedTime = 999f;

        this.animator = animator;
        this.objectTransform = objectTransform;
        this.targetTransform = targetTransform;
        this.attackRange = attackRange;
        this.attackRate = attackRate;

        defaultPosition = objectTransform.position;
        animator = objectTransform.GetComponent<Animator>();
        playerHealth = targetTransform.GetComponent<PlayerHealth>();
        this.attackDamage = attackDamage;
    }

    public override NodeState Evaluate()
    {
        if ((Mathf.Abs(lastAttackedTime - Time.time) >= attackRate) && ExtensionMethodsBT.GetDistance(ExtensionMethodsBT.GetXZVector(objectTransform.position), ExtensionMethodsBT.GetXZVector(targetTransform.position)) <= attackRange)
        {

            playerHealth.TakeDamage(attackDamage);
            objectTransform.LookAt(targetTransform);
            animator.SetTrigger("isAttacking");
            lastAttackedTime = Time.time;
            Debug.Log("Attacking...");
            return NodeState.RUNNING;
        }

        return NodeState.FAILURE;
    }

}