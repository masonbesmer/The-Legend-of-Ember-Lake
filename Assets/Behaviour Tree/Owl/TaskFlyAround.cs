using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskFlyAround : Node
{
    Transform objectTransform, targetTransform;
   // Vector3 defaultPosition;
   // private bool hasKey;
    private float attackRange;
    private bool chase;
    private float chaseTime;
    private float lastChaseTime;

    private Animator animator;
    public TaskFlyAround(Transform objectTransform, Transform targetTransform, float attackRange)
    {
     ///   defaultPosition = objectTransform.position;
        chase = false;
        lastChaseTime = Time.time;
        chaseTime = 10.0f;
        this.attackRange = attackRange;
        //  this.defaultPosition = objectTransform.position;
       // hasKey = false;
        this.objectTransform = objectTransform;
        this.targetTransform = targetTransform;
        this.animator = objectTransform.GetComponent<Animator>();
        //   this.attackRange = attackRange;
    }
    public override NodeState Evaluate()
    {
        //if (chase) return NodeState.SUCCESS;
        if (parent.HasKey("chase")){
            chase = (bool)parent.GetData("chase");
        }

        if(Mathf.Abs(lastChaseTime - Time.time) >= chaseTime && !chase)
        {
            Debug.Log("Last chase time: " + lastChaseTime + " | " + Time.time);
            lastChaseTime = Time.time;
            parent.SetData("chase", true);
            chase = true;
        }

        if (!chase)
        {
            if (parent.HasKey("defaultPosition"))
            {
                animator.SetBool("fly", true);
                objectTransform.RotateAround((Vector3)parent.GetData("defaultPosition"), Vector3.up, 50 * Time.deltaTime);
                objectTransform.LookAt(objectTransform.forward);
                Debug.Log("Flying around.");
            }
           // return NodeState.RUNNING;
        }
        Debug.Log("Chase status : " + chase);
        return chase ? NodeState.SUCCESS : NodeState.FAILURE;
      //  return NodeState.SUCCESS;
    }
}
