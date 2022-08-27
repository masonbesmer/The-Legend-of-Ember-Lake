using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;


public class TaskIdle : Node
{
    private Transform nodeTransform;
    private Animator animator;
    private int id;

    public TaskIdle(Transform nodeTransform, int id)
    {
        this.nodeTransform = nodeTransform;
        animator = nodeTransform.GetComponent<Animator>();
        this.id = id;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override NodeState Evaluate()
    {
        Debug.Log("Idling around" + id);
        nodeState = NodeState.SUCCESS;
        return base.Evaluate();
    }
}

