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

    SkillType currentSkill;

    bool canAttack;
    private void Awake()
    {
        abilityList = new List<SkillType>();
        
        //abilityCount = 0;
       // abilityList.Add(SkillType.DEFAULT);
     //   AbilityReceived(SkillType.EXPLODE);
        // AbilityReceived(SkillType.SPREAD);
    }
    void Start()
    {
        AbilityReceived(SkillType.DEFAULT);
        AbilityReceived(SkillType.HOMING);
        currentSkill = SkillType.DEFAULT;
       // AbilityReceived(SkillType.EXPLODE);        
    }

    // Update is called once per frame
    void Update()
    {
        ChangeAbility();

        if (Input.GetMouseButtonDown(0))
        {
            canAttack = true;
            Debug.Log("Abiliy active : " + currentSkill);
        }
    }

    private void FixedUpdate()
    {
        if (canAttack)
        {
            canAttack = false;
            CurrentAbility();
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
        if(Input.GetKey(KeyCode.Alpha1) && abilityCount >= 0)
        {
            SkillType changeSkillTo = abilityList[0];
            onAbilityChange?.Invoke(changeSkillTo);
            currentSkill = changeSkillTo;
           // DefaultAttack();
        }
        else if (Input.GetKey(KeyCode.Alpha2) && abilityCount >= 1)
        {
            SkillType changeSkillTo = abilityList[1];
            onAbilityChange?.Invoke(changeSkillTo);
            currentSkill = changeSkillTo;

        }
        else if (Input.GetKey(KeyCode.Alpha3) && abilityCount >= 2)
        {
            SkillType changeSkillTo = abilityList[2];
            onAbilityChange?.Invoke(changeSkillTo);
            currentSkill = changeSkillTo;
        }
        else if (Input.GetKey(KeyCode.Alpha4) && abilityCount >= 3)
        {
            SkillType changeSkillTo = abilityList[3];
            onAbilityChange?.Invoke(changeSkillTo);
            currentSkill = changeSkillTo;
        }

    }

    void CurrentAbility()
    {
        switch (currentSkill)
        {
            case SkillType.DEFAULT:
                DefaultAttack();
                break;
            case SkillType.SPREAD:
                SpreadAttack();
                break;
            case SkillType.EXPLODE:
                ExplodeAttack();
                break;
            case SkillType.HOMING:
                HomingAttack();
                break;
        }
    }

    void DefaultAttack()
    {

       // Physics.SphereCast(transform.position + new Vector3(0, 0.699999988f, -1.89999998f), 2f, transform.forward, out hitInfo, 5f,enemyMask);
        Collider[] hitInfoList = Physics.OverlapSphere(transform.position + new Vector3(0, 2.6f, -4.09f), 7f, enemyMask);

        Debug.DrawRay(transform.position + new Vector3(0, 2.6f, -4.09f), -transform.forward, Color.blue);
        
        if (hitInfoList.Length > 0)
        {
            foreach(Collider child in hitInfoList)
            {
                Debug.Log(child.name);
                if(child != null && child.TryGetComponent(out EnemyHealth enemyHealth)){
                    Debug.Log("Hitting enemy");
                 //   EnemyHealth enemy = hitInfo.collider.GetComponent<EnemyHealth>();
                    enemyHealth.TakeDamage(10);
                } 
            }
        }
        else
        {
            //  Debug.Log("No cast");
        }
    }

    void SpreadAttack()
    {

    }

    void ExplodeAttack()
    {

    }

    void HomingAttack()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        cube.transform.position = transform.position;
        cube.AddComponent(typeof(Rigidbody));

        //cube.GetComponent<SphereCollider>().
        Rigidbody rb = cube.GetComponent<Rigidbody>();
        rb.AddForce(-transform.forward * 800f * Time.deltaTime, ForceMode.Impulse);

        rb.GetComponent<SphereCollider>().isTrigger = true;
        rb.useGravity = false;

        Destroy(cube, 3f);
    }

}
