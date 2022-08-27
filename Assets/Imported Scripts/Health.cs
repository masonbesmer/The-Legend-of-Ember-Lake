using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Health : MonoBehaviour, IHealth
{
    [SerializeField] protected float maxHealth;

    protected float currentHealth;

    public Action<float> onHurtAction;
    public Action onDeathAction;

    protected bool isDead;

    private void Awake()
    {
        currentHealth = maxHealth;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
 
    }

    public virtual void Heal(float heal)
    {
        if(currentHealth < maxHealth)
        {
            currentHealth += heal;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }
    }
    public virtual void TakeDamage(int damage)
    {
        if (damage < currentHealth)
        {
            currentHealth -= damage;
            onHurtAction?.Invoke(currentHealth / maxHealth);
        }
        else
        {
            Debug.Log("Dead");
            isDead = true;
            onDeathAction?.Invoke();
          //  gameObject.SetActive(false);
            //return;
        }
    }
}
