using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskFlyAround : Node
{
    Transform objectTransform, targetTransform;
   // private bool hasKey;
    private float attackRange;
    private bool chase;
    private float chaseTime;
    private float lastChaseTime;
    public TaskFlyAround(Transform objectTransform, Transform targetTransform, float attackRange)
    {
        chase = false;
        lastChaseTime = 0;
        chaseTime = 5.0f;
        this.attackRange = attackRange;
        //  this.defaultPosition = objectTransform.position;
       // hasKey = false;
        this.objectTransform = objectTransform;
        this.targetTransform = targetTransform;
        //   this.attackRange = attackRange;
    }
    public override NodeState Evaluate()
    {
        if (parent.HasData("chase"))
        {
            chase = (bool)parent.GetData("chase");
            Debug.Log("Parent chase data: " + chase);
        }

        if(Mathf.Abs(lastChaseTime - Time.time) >= chaseTime && !chase)
        {
            Debug.Log("Last chase time: " + lastChaseTime);
            lastChaseTime = Time.time;
            parent.SetData("chase", true);
            chase = true;
        }


        if (!chase)
        {
            objectTransform.RotateAround(targetTransform.position, Vector3.up, 50 * Time.deltaTime);
            Debug.Log("Flying around.");
        }

        return chase ? NodeState.SUCCESS : NodeState.RUNNING;
    }
}
