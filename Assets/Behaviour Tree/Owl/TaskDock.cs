using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskDock : Node
{

    Transform objectTransform, targetTransform;
    Vector3 dockPosition;
    bool canDock;

    Animator animator;
    public TaskDock(Transform objectTransform, Transform targetTransform)
    {
        canDock = false;
        //  this.defaultPosition = objectTransform.position;
        this.objectTransform = objectTransform;
        this.targetTransform = targetTransform;
        animator = objectTransform.GetComponent<Animator>();
        //   this.attackRange = attackRange;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("Docking (idle)");

        if (canDock)
        {
            Debug.Log("Can fly down");

            if (objectTransform.position == dockPosition)
            {
                animator.SetBool("fly", false);
                parent.parent.SetData("isUp", false);
                // canDock = false;
            }
            else
            {
                objectTransform.position = Vector3.MoveTowards(objectTransform.position, dockPosition, Time.deltaTime * 5.0f);
                return NodeState.RUNNING;
            }
        }
        else if (parent.HasKey("dock") && parent.HasData("dock"))
        {
            animator.SetBool("fly", true);
            Debug.Log("Has dock");
            canDock = true;
            dockPosition = (Vector3)parent.GetData("dock");

        }

        // RaycastHit hit = Physics.Raycast()
        return base.Evaluate();
    }
}
