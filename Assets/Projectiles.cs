using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectiles : MonoBehaviour
{
    [SerializeField] protected int damage;
    [SerializeField] protected float hp;

    PlayerHealth playerHealth;
    private void Start()
    {
        InitializeBehaviour();
    }

    private void Update()
    {
        MovementBehaviour();   
    }

    public abstract void InitializeBehaviour();
    public abstract void MovementBehaviour();
    public abstract void DeathBehaviour();

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Player found");
            playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damage);
            DeathBehaviour();
        }
    }
}
