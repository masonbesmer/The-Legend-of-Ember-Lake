using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public abstract class Tree : MonoBehaviour
    {
        [SerializeField] protected Transform objectTransform;
        [SerializeField] protected Transform targetTransform;

        private Node root = null;
        void Start()
        {
            root = SetupTree();
          //  Destroy(gameObject, 3);
        }

        // Update is called once per frame
        void Update()
        {
            if(root != null)
            {
                root.Evaluate();
            }
        }

        protected abstract Node SetupTree();
        public void SetTarget(GameObject target) => targetTransform = target.transform;
    }


}
