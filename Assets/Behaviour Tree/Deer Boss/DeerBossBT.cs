using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;
public class DeerBossBT : BehaviourTree.Tree
    
{
    [SerializeField] GameObject ghostDeerPrefab;
    [SerializeField] float chaseRange;
    [SerializeField] float returnRange;
    [SerializeField] float attackRange;
    [SerializeField] float stampedeMax;
    [SerializeField] float attackRate;
    [SerializeField] float ghostDeercount;

    public float speed;
    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new DeerBossStampedeTask(ghostDeerPrefab,targetTransform,terrain, objectTransform,attackRange,attackRate,stampedeMax, ghostDeercount),
            new DeerBossDashTask(objectTransform.GetComponent<NavMeshAgent>(), terrain,targetTransform,objectTransform,attackRange)
           // new TaskAttack(objectTransform.GetComponent<NavMeshAgent>(), targetTransform,objectTransform,attackRange),
           // new TaskChase(objectTransform.GetComponent<NavMeshAgent>(), objectTransform.GetComponent<Rigidbody>(),targetTransform,objectTransform,chaseRange,returnRange),
           // new TaskIdle(objectTransform.GetComponent<NavMeshAgent>(),objectTransform,targetTransform)
        });

        return root;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, returnRange);
    }
}
