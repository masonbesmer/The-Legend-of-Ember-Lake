using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
public class DeerBossDashTask : Node
{
    private Transform objectTransform;
    private Transform targetTransform;
    private Vector3 defaultPosition;

    private Animator animator;
    private float attackRange;
    private NavMeshAgent navAgent;
    private Vector3 dashDirection;
    private bool canDash;
    private Terrain terrain;

    private float lastAttackedTime = 999f;
    private float attackRate = 3.0f;

    public DeerBossDashTask(NavMeshAgent navAgent, Terrain terrain, Transform targetTransform, Transform objectTransform, float attackRange)
    {
        this.terrain = terrain;
        this.navAgent = navAgent;
        this.objectTransform = objectTransform;
        this.targetTransform = targetTransform;
        this.attackRange = attackRange;

        defaultPosition = objectTransform.position + ExtensionMethodsBT.GetXZDirection(objectTransform.position, targetTransform.position).normalized * 10.0f;
      //  var sphere = GameObject.CreatePrimitive(PrimitiveType.Capsule);
       // sphere.transform.position = defaultPosition;
        animator = objectTransform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        bool isWithinChaseRange = ExtensionMethodsBT.GetDistance(ExtensionMethodsBT.GetXZVector(defaultPosition), ExtensionMethodsBT.GetXZVector(objectTransform.position)) <= 3.0f;
        bool canAttack = (Mathf.Abs(Time.time - lastAttackedTime)) >= attackRate;

        animator.SetBool("isRunning", false);
        if (!isWithinChaseRange)
        {
            animator.SetBool("isRunning", true);
            objectTransform.position = Vector3.MoveTowards(objectTransform.position, defaultPosition, Time.deltaTime * 10);
            return NodeState.RUNNING;
        }
        else if(isWithinChaseRange && canAttack)
        {
          //  animator.SetBool("dash");
            lastAttackedTime = Time.time;
            Debug.Log("Is within range and moving");
            objectTransform.LookAt(targetTransform);
            defaultPosition = objectTransform.position + objectTransform.forward *  20.0f;
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }
}
