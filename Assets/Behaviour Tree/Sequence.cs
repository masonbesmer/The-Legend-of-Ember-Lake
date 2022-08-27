using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviourTree
{
    public class Sequence : Node
    {
        public Sequence() : base(){ }
        public Sequence(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            bool isRunning = false;
           
            foreach(Node child in childrenList)
            {
                switch (child.Evaluate())
                {
                    case NodeState.RUNNING:
                        nodeState = NodeState.RUNNING;
                        isRunning = true;
                        return nodeState;
                      //  break;
                    case NodeState.SUCCESS:
                        nodeState = NodeState.SUCCESS;
                        return nodeState;
                      //  break;
                    case NodeState.FAILURE:
                        nodeState = NodeState.FAILURE;
                        return nodeState;
                        //break;
                }
            }

            nodeState = isRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return nodeState;
        }
    }
}
