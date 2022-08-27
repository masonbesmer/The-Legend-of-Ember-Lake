using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAbility : MonoBehaviour
{

    //[SerializeField] Animator wand;
    [SerializeField] LayerMask enemyMask;
    // Start is called before the first frame update
    public Action<SkillType> onAbilityChange;
    public Action<SkillType> onAbilityReceived;
    int abilityCount;
    List<SkillType> abilityList;
    private void Awake()
    {
        abilityList = new List<SkillType>();
        abilityList.Add(SkillType.DEFAULT);
        abilityCount = 0;
        AbilityReceived(SkillType.SPREAD);
        AbilityReceived(SkillType.EXPLODE);
       // AbilityReceived(SkillType.SPREAD);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChangeAbility();

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Attacking");
          //  wand.SetTrigger("attack");
            //   Ray ray = new Ray(transform.position,);
            RaycastHit hitInfo;

            Physics.SphereCast(transform.position, 1f, transform.forward,out hitInfo,5f);
            //var hitInfo = Physics.OverlapSphere(transform.forward * 1.5f, 5f,enemyMask);
           // Physics.ray
            if (hitInfo.collider != null)
            {
                EnemyHealth enemy = hitInfo.collider.GetComponent<EnemyHealth>();
                enemy.TakeDamage(1);
            }
            else
            {
                Debug.Log("No cast");
            }
        }
    }

    void AbilityReceived(SkillType skillType)
    {
       // onAbilityChange?.Invoke(skillType);
        onAbilityReceived?.Invoke(skillType);
        abilityList.Add(skillType);
        abilityCount++;
    }

    void ChangeAbility()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && abilityCount >= 0)
        {
            onAbilityChange?.Invoke(abilityList[0]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && abilityCount >= 1)
        {
            onAbilityChange?.Invoke(abilityList[1]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && abilityCount >= 2)
        {
            onAbilityChange?.Invoke(abilityList[2]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && abilityCount >= 3)
        {
            onAbilityChange?.Invoke(abilityList[3]);
        }
    }

}
