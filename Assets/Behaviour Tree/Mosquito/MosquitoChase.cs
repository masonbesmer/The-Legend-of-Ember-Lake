using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
public class MosquitoChase : Node
{
    private Transform objectTransform;
    private Transform targetTransform;

    private readonly Vector3 defaultPosition;
    private Animator animator;

    private float chaseRange;
    private Terrain terrain;
    private float flightHeight;
    private bool isInDefaultPosition;
    private bool isWithinHome = false;
    private float moveSpeed;
    private float attackRange;
    public MosquitoChase(Transform objectTransform, Transform targetTransform, Terrain terrain, float chaseRange, float attackRange, float moveSpeed, float flightHeight)
    {
        this.attackRange = attackRange;
        this.moveSpeed = moveSpeed;
        this.flightHeight = flightHeight;
        this.targetTransform = targetTransform;
        this.objectTransform = objectTransform;
        this.terrain = terrain;
        defaultPosition = objectTransform.position;
        animator = this.objectTransform.GetComponent<Animator>();
        this.chaseRange = chaseRange;
    }
    public override NodeState Evaluate()
    {
        bool isWithinChaseRange = ExtensionMethodsBT.GetDistance(ExtensionMethodsBT.GetXZVector(defaultPosition), ExtensionMethodsBT.GetXZVector(targetTransform.transform.position)) <= chaseRange;
        bool isWithinAttackRange = ExtensionMethodsBT.GetDistance(ExtensionMethodsBT.GetXZVector(objectTransform.position), ExtensionMethodsBT.GetXZVector(targetTransform.transform.position)) <= attackRange;

        if (!isWithinChaseRange)
             isWithinHome = ExtensionMethodsBT.GetDistance(ExtensionMethodsBT.GetXZVector(defaultPosition), ExtensionMethodsBT.GetXZVector(objectTransform.transform.position)) >= 3.0f;

        if (isWithinChaseRange && !isWithinAttackRange)
        {
            objectTransform.LookAt(targetTransform);
            isInDefaultPosition = false;
            objectTransform.position = Vector3.MoveTowards(new Vector3(objectTransform.position.x, objectTransform.position.y, objectTransform.position.z), targetTransform.position, Time.deltaTime * moveSpeed);
            return NodeState.RUNNING;
        }
        else if (isWithinHome && !isWithinChaseRange)
        {
            objectTransform.LookAt(defaultPosition);
            objectTransform.position = Vector3.MoveTowards(new Vector3(objectTransform.position.x, objectTransform.position.y, objectTransform.position.z), defaultPosition, Time.deltaTime * moveSpeed);
            return NodeState.RUNNING;
        }

        return base.Evaluate();
    }
}
