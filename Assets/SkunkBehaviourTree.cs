using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using Tree = BehaviourTree.Tree;
using UnityEngine.AI;

public class SkunkBehaviourTree : Tree
{
    [SerializeField] Transform objectTransform;
    [SerializeField] Transform targetTransform;
    [SerializeField] ParticleSystem fartPX;
    [SerializeField] float chaseRange;
    [SerializeField] float returnRange;
    [SerializeField] float attackRange;

    
    public float speed;
    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new SkunkTaskAttack(objectTransform.GetComponent<NavMeshAgent>(), targetTransform,objectTransform,attackRange),
            new TaskChase(objectTransform.GetComponent<NavMeshAgent>(), objectTransform.GetComponent<Rigidbody>(),targetTransform,objectTransform,chaseRange,returnRange),
            new TaskIdle(targetTransform,1)
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

    public void SetTarget(Transform playerTransform) => targetTransform = playerTransform;

    public void Fart()
    {
        // ParticleSystem fart = Instantiate(fartPX);
        fartPX.Play();
       //fart.transform.position = transform.position;
      // fart.transform.LookAt(targetTransform.position);
    }

}
