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
            
        });

        return root;
    }
}
