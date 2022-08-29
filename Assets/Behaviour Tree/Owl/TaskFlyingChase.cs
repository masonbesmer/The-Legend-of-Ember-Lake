using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class TaskFlyingChase : Node
{
    private float timeDelay;
    private bool isChasing;

    public TaskFlyingChase()
    {

        timeDelay = 3.0f;
        isChasing = true;
    }
    public override NodeState Evaluate()
    {
        timeDelay -= Time.deltaTime;

        if(timeDelay <= 0)
        {
            Debug.Log("Delaying time for chase");
            timeDelay = 3.0f;
            parent.SetData("chase", false);
            isChasing = false;
        }

        return isChasing ?  NodeState.RUNNING : NodeState.SUCCESS;
    }
}
