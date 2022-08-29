using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
public class TaskFlyUp : Node
{
    float attackRange;
    private Transform objectTransform;
    private Transform targetTransform;
    private Vector3 defaultPosition;
    private bool isUp;

    public TaskFlyUp(Transform objectTransform, Transform targetTransform, float attackRange)
    {
        this.defaultPosition = objectTransform.position;
        this.objectTransform = objectTransform;
        this.targetTransform = targetTransform;
        this.attackRange = attackRange;
        isUp = false;
    }
    public override NodeState Evaluate()
    {
        bool isWithinAttackRange = ExtensionMethodsBT.GetDistance(ExtensionMethodsBT.GetXZVector(objectTransform.position), ExtensionMethodsBT.GetXZVector(targetTransform.position)) <= attackRange;
        if(!isWithinAttackRange) return NodeState.FAILURE;
    
        if (isWithinAttackRange && (parent.parent.HasKey("isUp") && parent.parent.HasData("isUp"))){
            isUp = (bool)GetData("isUp");
        }
        
        if (!isUp && isWithinAttackRange){
            Debug.Log("Flying up and up");
            Vector3 flyingPosition = defaultPosition + Vector3.up * 8.0f;
            objectTransform.position = Vector3.MoveTowards(objectTransform.position,flyingPosition, Time.deltaTime * 10f);

            if(objectTransform.position == flyingPosition)
            {
                isUp = true;
                parent.parent.SetData("isUp", true);
                Debug.Log("Successful");
                return NodeState.SUCCESS;
            }
        }

        return isUp ? NodeState.RUNNING : NodeState.FAILURE;
    }
}
