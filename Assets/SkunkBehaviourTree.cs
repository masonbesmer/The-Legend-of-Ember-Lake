using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using Tree = BehaviourTree.Tree;
using UnityEngine.AI;

public class SkunkBehaviourTree : Tree
{
    [SerializeField] ParticleSystem fartPX;
    [SerializeField] float chaseRange;
    [SerializeField] float returnRange;
    [SerializeField] float attackRange;
    [SerializeField] int skunkDamage;
    [SerializeField] LayerMask playerMask;

    
    public float speed;
    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new SkunkTaskAttack(objectTransform.GetComponent<NavMeshAgent>(), targetTransform,objectTransform,attackRange, skunkDamage, playerMask),
            new TaskChase(objectTransform.GetComponent<NavMeshAgent>(), objectTransform.GetComponent<Rigidbody>(),targetTransform,objectTransform,chaseRange,returnRange),
            new TaskIdle(objectTransform.GetComponent<NavMeshAgent>(),objectTransform,targetTransform)
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
        
        Physics.SphereCast(new Ray(transform.position, transform.forward), 2f, out RaycastHit hitInfo, 10f, playerMask);

        if (hitInfo.collider != null)
        {
            hitInfo.collider.GetComponent<IHealth>().TakeDamage(skunkDamage);
        }
        //fart.transform.position = transform.position;
        // fart.transform.LookAt(targetTransform.position);
    }

}
