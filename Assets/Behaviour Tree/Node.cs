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
        /*    foreach(Node child  in childrenList)
            {
                if(child.GetData)
            }
            */
            if (nodeDataContainer.TryGetValue(key, out value)) return value;
          


            if(parent != null)
            {
                return parent.GetData(key);
                //if (nodeDataContainer.TryGetValue(key, out value)) return value;
            }

            return null;
       /*     Node parentNode = parent;
            while(parentNode != null)
            {
                value = parentNode.GetData(key);
                if (value != null) return value;
                parentNode = parent;
            }
            return null;*/
        }
        public void SetData(string key, object value)
        {
            nodeDataContainer[key] = value;
            
            Node node = parent;

            while(node != null)
            {
                node.SetData(key, value);
                node = node.parent;
            }
        }

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

        public bool HasKey(string key)
        {
           return nodeDataContainer.ContainsKey(key);
        }

        public bool HasData(string key) => nodeDataContainer.ContainsKey(key);
    }
}
