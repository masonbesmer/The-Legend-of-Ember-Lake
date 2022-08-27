using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum NonSkillItemTypes{ACORNS,HEART,ARMOR};
public class Pickupable : MonoBehaviour
{
    public NonSkillItemTypes nonSkillItemTypes;

    public static Action<NonSkillItemTypes> onPickupAction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            onPickupAction?.Invoke(nonSkillItemTypes);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            onPickupAction?.Invoke(nonSkillItemTypes);
            Destroy(gameObject);
        }
    }

}
