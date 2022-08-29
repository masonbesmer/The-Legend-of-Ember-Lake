using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
public class CheckDockPositionNode : Node
{
    Transform objectTransform, targetTransform;
    private bool hasKey;
    private float attackRange;
    public CheckDockPositionNode(Transform objectTransform, Transform targetTransform, float attackRange)
    {
        this.attackRange = attackRange;
        //  this.defaultPosition = objectTransform.position;
        hasKey = false;
        this.objectTransform = objectTransform;
        this.targetTransform = targetTransform;
     //   this.attackRange = attackRange;
    }
    public override NodeState Evaluate()
    {
        //  hasEntered = false;

        //Debug.Log("Checking docking position");
        if (!parent.HasKey("dock"))
        {
            hasKey = true;
            Debug.Log("No key");
            Physics.Raycast(new Ray(objectTransform.position, Vector3.down), out RaycastHit hitInfo, 10f);
            if (hitInfo.collider != null)
            {
                Debug.Log("Data has been set");
                parent.SetData("dock", hitInfo.point);
                Debug.Log((Vector3)GetData("dock"));
                //   dockPosition = hitInfo.point;
            }
        }

        return hasKey && (ExtensionMethodsBT.GetDistance(ExtensionMethodsBT.GetXZVector(objectTransform.position), ExtensionMethodsBT.GetXZVector(targetTransform.position)) >= attackRange)? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
