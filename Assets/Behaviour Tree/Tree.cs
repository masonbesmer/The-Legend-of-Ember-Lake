using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public abstract class Tree : MonoBehaviour
    {
        [SerializeField] protected Transform objectTransform;
        [SerializeField] protected Transform targetTransform;

        [SerializeField] protected Terrain terrain;
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
        public void SetTarget(GameObject target, Terrain terrain = null)
        {
            targetTransform = target.transform;
            this.terrain = terrain;
        }
    }


}
