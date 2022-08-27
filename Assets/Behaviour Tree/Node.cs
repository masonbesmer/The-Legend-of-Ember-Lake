using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public enum NodeState
    {
        RUNNING, SUCCESS, FAILURE
    }
    public class Node
    {
        protected NodeState nodeState;

        public Node parent;
        protected List<Node> childrenList;


        private Dictionary<string, object> nodeDataContainer;
        public Node()
        {
            parent = null;
            childrenList = new List<Node>();
            nodeDataContainer = new Dictionary<string, object>();
        }

        public Node(List<Node> childredList)
        {
            nodeDataContainer = new Dictionary<string, object>();
            childrenList = new List<Node>();
            AddChildren(childredList);
        }

        private void AddChild(Node child)
        {
            child.parent = this;
            childrenList.Add(child);
        }

        private void AddChildren(List<Node> children)
        {
            foreach(Node node in children)
            {
                node.parent = this;
                childrenList.Add(node);
            }
        }

        public virtual NodeState Evaluate() => NodeState.FAILURE;

        public object GetData(string key)
        {
            object value = null;
            if (nodeDataContainer.TryGetValue(key, out value)) return value;
            return parent.GetData(key);
        }
        public void SetData(string key, object value) => nodeDataContainer[key] = value;

        public bool ClearData(string key)
        {
            if(nodeDataContainer.ContainsKey(key))
            {
                nodeDataContainer.Remove(key);
                return true;
            } else if(parent != null)
            {
                return parent.ClearData(key);
            }
            else
            {
                return false;
            }

 /*           while(parent != null)
            {
                if (parent.ClearData(key)) return true;

                parent = parent.parent;
            }

            return false;*/
        }
    }
}
