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
        Node root = new Selector(new List<Node>
        {
            new TaskAttack(objectTransform.GetComponent<NavMeshAgent>(), targetTransform,objectTransform,attackRange),
            new TaskChase(objectTransform.GetComponent<NavMeshAgent>(), objectTransform.GetComponent<Rigidbody>(),targetTransform,objectTransform,chaseRange,returnRange),
            new TaskIdle(targetTransform,1)
        });
    }
}
