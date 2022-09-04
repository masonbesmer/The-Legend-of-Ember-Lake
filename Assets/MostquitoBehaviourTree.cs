using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using Tree = BehaviourTree.Tree;
public class MostquitoBehaviourTree : Tree
{
    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip hurtSound;
    [SerializeField] AudioClip idleSound;
    
    [SerializeField] float chaseRange;
    [SerializeField] float returnRange;
    [SerializeField] float attackRange;
    [SerializeField] float attackRate;
    [SerializeField] float flightHeight;
    [SerializeField] float moveSpeed;
    [SerializeField] int attackDamage;
    public float speed;
    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new MosquitoAttack(targetTransform,objectTransform,objectTransform.GetComponent<Animator>(), attackRange,attackRate,attackDamage),
            new MosquitoChase(objectTransform,targetTransform,terrain,chaseRange, attackRange, moveSpeed,flightHeight)
        });

        return root;
    }

    float counter;
    private void LateUpdate()
    {
        counter += Time.deltaTime;
        GetComponent<Animator>().Play("mosquito-idle", 0,counter);
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
    public void PlayIdleSound()
    {
        audioSource.clip = idleSound;
        audioSource.Play();
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
