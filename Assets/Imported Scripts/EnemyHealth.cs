using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : Health
{
    [SerializeField] GameObject[] healthObject;
    [SerializeField] GameObject target;
    NavMeshAgent agent;

    int healthCounter = 0;
    // healthMax;
    float miniMaxHealth;
    float miniCurrentHealth;

    Animator healthAnimator;

    Animator moveAnimator;
    bool isAttacking;
    // Start is called before the first frame update
    void Start()
    {
        moveAnimator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
       miniMaxHealth = maxHealth / healthObject.Length;
       miniCurrentHealth = miniMaxHealth;
        Debug.Log("Mini max healht" + miniMaxHealth);
       healthAnimator = healthObject[healthCounter].GetComponent<Animator>();
     //  animator = GetComponent<Animator>();
      // animator.Play("takeDamage", 0,);
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.transform.position);


        if (Vector3.Distance(transform.position, target.transform.position) <= agent.stoppingDistance)
        {
            moveAnimator.SetBool("attack", true);
        }
        else
        {
            moveAnimator.SetBool("attack", false);
        }

    }

    public void DamagePlayer()
    {
        target.GetComponent<PlayerHealth>().TakeDamage(2);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        miniCurrentHealth -= damage;
     

        if ((miniMaxHealth - miniCurrentHealth) >= miniMaxHealth)
        {
            
            healthObject[healthCounter].SetActive(false);
            
            if (healthCounter + 1 < healthObject.Length)
            {
                healthCounter++;

                healthAnimator = healthObject[healthCounter].GetComponent<Animator>();

                miniCurrentHealth = miniMaxHealth;
            }

        }

        if (isDead) Destroy(gameObject);

        if (healthAnimator != null)
        {
            Debug.Log("Enemy damage taken" + (miniMaxHealth - miniCurrentHealth) / miniMaxHealth);
            healthAnimator.Play("takeDamage", 0, (miniMaxHealth - miniCurrentHealth) / miniMaxHealth);
        }

    }
}
