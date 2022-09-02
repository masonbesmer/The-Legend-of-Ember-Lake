using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class TaskIdle : Node
{
    private Transform objectTransform, targetTransform;
    private Vector3 defaultPosition;
    private Animator animator;
    private NavMeshAgent navAgent;
    private int id;

    public TaskIdle(NavMeshAgent agent, Transform objectTransform, Transform targetTransform)
    {
        this.navAgent = agent;
        this.objectTransform = objectTransform;
        this.targetTransform = targetTransform;
        animator = this.objectTransform.GetComponent<Animator>();
        defaultPosition = objectTransform.position;
    //    this.id = id;
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
        if (objectTransform.position != defaultPosition)
        {
            Debug.Log("Returning to player");
            navAgent.SetDestination(defaultPosition);
            navAgent.isStopped = false;
            return NodeState.RUNNING;
        }
        else
        {
            animator.SetBool("isRunning", false);
            return NodeState.FAILURE;
        }
        // Debug.Log("Idling around" + id);
        //  nodeState = NodeState.SUCCESS;
      //  return base.Evaluate();
    }
}

