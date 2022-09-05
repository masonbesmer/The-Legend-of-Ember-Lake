using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
public class DeerBossStampedeTask : Node
{
    private Transform objectTransform;
    private Transform targetTransform;
    private Vector3 defaultPosition;

    private Animator animator;
    private float attackRange;
    private NavMeshAgent navAgent;
    private Vector3 dashDirection;
    private bool canDash;
    private Terrain terrain;

    private float lastAttackedTime = 999f;
    private float attackRate = 3;
    private bool isInStampede;
    private bool canStampede;

    private GameObject ghostDeerPrefab;
    private float ghostDeerCount;

    private float stampedeModeOffset = .5f;
    private float lastStampedeTime;
    private float stampedeCount;
    private float stampedeMax;
    private bool stampedeOver;
    private float deerSpeed;
    private float ghostDeerDistance;


    public DeerBossStampedeTask(GameObject ghostDeerPrefab, Transform targetTransform,Terrain terrain,  Transform objectTransform, float attackRange, float attackRate, float deerSpeed, float ghostDeerDistance, float xstampedeCount, float gdCount)
    {
        stampedeOver = false;
        isInStampede = false;
       
        stampedeCount = 0;
        stampedeMax = 2;

        this.ghostDeerDistance = ghostDeerDistance;
        this.deerSpeed = deerSpeed;
        this.attackRate = attackRate;
        this.stampedeMax = xstampedeCount;
        this.terrain = terrain;
        this.objectTransform = objectTransform;
        this.targetTransform = targetTransform;
        this.attackRange = attackRange;
        this.ghostDeerPrefab = ghostDeerPrefab;
        this.ghostDeerCount = gdCount;
        animator = objectTransform.GetComponent<Animator>();
        lastStampedeTime = 999f;
    }

    public override NodeState Evaluate()
    {

        bool isWithinChaseRange = ExtensionMethodsBT.GetDistance(ExtensionMethodsBT.GetXZVector(defaultPosition), ExtensionMethodsBT.GetXZVector(objectTransform.position)) <= attackRange;

        if (((Mathf.Abs(lastAttackedTime - Time.time) >= attackRate) || isInStampede) && isWithinChaseRange)
        {
            stampedeOver = false;
            lastAttackedTime = Time.time;
           // stampedeOver = 
            if (!isInStampede)
            {
                animator.SetTrigger("isStampede");
                isInStampede = true;
                lastStampedeTime = Time.time;
                canStampede = true;
                stampedeCount++;
            }
            else if ((Mathf.Abs(Time.time - lastStampedeTime)) >= stampedeModeOffset)
            {
                Debug.Log("Stampeding twice");
                stampedeCount++;
                canStampede = true;
            }

            if (canStampede)
            {
                canStampede = false;

                for (int i = 0; i < ghostDeerCount; i++)
                {
                    GameObject deer = GameObject.Instantiate(ghostDeerPrefab, objectTransform.position + objectTransform.right * Random.Range(-ghostDeerDistance, ghostDeerDistance), objectTransform.rotation);
                    deer.GetComponent<DeerGhostBT>().SetDeer(deerSpeed, terrain);
                }

                if (stampedeCount >= stampedeMax)
                {
                    stampedeOver = true;
                    canStampede = false;
                    stampedeCount = 0;
                    isInStampede = false;
                }
            }

            return NodeState.RUNNING;
        }
        else
        {
            return base.Evaluate();
        }
    }
}
