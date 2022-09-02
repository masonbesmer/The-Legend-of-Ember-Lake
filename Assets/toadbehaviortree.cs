using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using Tree = BehaviourTree.Tree;

public class toadbehaviortree : Tree
{
    // Start is called before the first frame update
    protected override Node SetupTree()
    {
        throw new NotImplementedException();
        /*Node root = new Selector(new List<Node>
        {
            new TaskAttack(objectTransform.GetComponent<UnityEngine.AI.NavMeshAgent>(), targetTransform,objectTransform,attackRange),
            new TaskChase(objectTransform.GetComponent<UnityEngine.AI.NavMeshAgent>(), objectTransform.GetComponent<Rigidbody>(),targetTransform,objectTransform,chaseRange,returnRange),
            new TaskIdle(targetTransform,1)
        });*/
    }
}
