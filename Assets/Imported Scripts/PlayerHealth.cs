using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealth : Health
{
    // Start is called before the first frame update
    public int heartSegments;

    public Action<float> OnInitializeHealthAction;
    public Action<float, float> OnHitAction;
    public Action<float, float, float> OnHealAction;

    int healthCounter = 0;
    // healthMax;
    float miniMaxHealth;
    float miniCurrentHealth;

    Canvas3DManager dManager;

    //Animator healthAnimator;

    private void Awake()
    {
        dManager = FindObjectOfType<Canvas3DManager>();
        dManager.OnMiniCurrentHealthUpdateAction += UpdateMiniHealth;
    }
    void Start()
    {
        OnInitializeHealthAction?.Invoke((maxHealth / heartSegments));
        miniMaxHealth = maxHealth / (maxHealth / heartSegments);
        miniCurrentHealth = miniMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
/*        if (Input.GetMouseButtonDown(1))
        {
            Heal(8);
        }

        if (Input.GetMouseButtonDown(0))
        {
            TakeDamage(1);
        }*/
    }

    void UpdateMiniHealth(float health)
    {
        miniCurrentHealth = health;
    }

    public override void Heal(float heal)
    {
        base.Heal(heal);
        OnHealAction?.Invoke(miniCurrentHealth, miniMaxHealth, heal);
    }

    public override void TakeDamage(int damage)
    {
        Debug.Log("Taking damage: " + damage);
        //  Debug.Log("Damage taken: " + damage + "Current health : " + miniCurrentHealth + " Max Health" + miniMaxHealth);

        base.TakeDamage(damage);

        miniCurrentHealth -= damage;

        OnHitAction?.Invoke(miniCurrentHealth, miniMaxHealth);

        if ((miniMaxHealth - miniCurrentHealth) >= miniMaxHealth)
        {
            miniCurrentHealth = miniMaxHealth;
        }
    }

    private void OnDestroy()
    {
        dManager.OnMiniCurrentHealthUpdateAction -= UpdateMiniHealth;
    }

    public int GetMaxHealth() => (int)maxHealth;
}
