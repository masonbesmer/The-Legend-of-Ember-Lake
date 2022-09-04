using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using Tree = BehaviourTree.Tree;
using UnityEngine.AI;
public class RacconBehaviourTree : Tree
{

    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip hurtSound;
    

    [SerializeField] float chaseRange;
    [SerializeField] float returnRange;
    [SerializeField] float attackRange;
    [SerializeField] float attackRate;
    [SerializeField] int attackDamage;

   // private AudioSource audioSource;

    public float speed;
    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new TaskAttack(objectTransform.GetComponent<NavMeshAgent>(), targetTransform,objectTransform,attackRange,attackRate,attackDamage),
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

    public void PlayAttackSound()
    {
        audioSource.clip = attackSound;
        audioSource.Play();
    }

    public void PlayHurtSound()
    {
        audioSource.clip = hurtSound;
        audioSource.Play();
        // audioSource.PlayOneShot(hurtSound);
    }

   // public void PlayHurtSound)

}
