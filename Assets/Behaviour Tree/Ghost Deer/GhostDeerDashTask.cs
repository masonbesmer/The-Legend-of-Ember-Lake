using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostDeerDashTask : BehaviourTree.Node
{

    public GhostDeerDashTask()
    {
       
    }

    public override NodeState Evaluate()
    {
        

        return NodeState.RUNNING;
    }
}
