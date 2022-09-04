using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;


public class FrogAtttackTask : Node
{
    Transform objectTransform, targetTransform;
    float attackRange, attackRate;

    Animator animator;

    private Vector3 defaultPosition;
    private List<SpitProperties> spitObjects;
    private GameObject spitPrefab;
    private SpitProperties spit;

    private bool hasSpit;
    private float lastAttackedTime;
    private float moveCounter;
    AnimationEvent eventAnim;
  
    public FrogAtttackTask(Transform objectTransform, Transform targetTransform, Animator animator, GameObject spitPrefab, float attackRange, float attackRate)
    {
        
        moveCounter = 0;
        lastAttackedTime = 999f;
        this.spitPrefab = spitPrefab;
        spitObjects = new List<SpitProperties>();
        this.objectTransform = objectTransform;
        this.targetTransform = targetTransform;
        this.attackRange = attackRange;
        this.attackRate = attackRate;
        this.animator = animator;

        defaultPosition = this.objectTransform.position;
    }

    public override NodeState Evaluate()
    {
        bool isWithinAttackRange = ExtensionMethodsBT.GetDistance(ExtensionMethodsBT.GetXZVector(objectTransform.position), ExtensionMethodsBT.GetXZVector(targetTransform.transform.position)) <= attackRange;

        if (isWithinAttackRange && !hasSpit && parent.HasKey("isJumping") && !(bool)parent.GetData("isJumping") && (Mathf.Abs(lastAttackedTime - Time.time) >= attackRate))
        {
            lastAttackedTime = Time.time;
            Debug.Log("Attacking player");
            animator.SetBool("isRunning", false);
            animator.SetBool("isIdling", false);
            animator.SetBool("isAttacking", true);
            objectTransform.LookAt(targetTransform);
            hasSpit = true;
                
            return NodeState.SUCCESS;
        }
        else if((hasSpit || this.spit.spit != null) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime * 29 >= 12)
        {
            if (hasSpit)
            {
                hasSpit = false;
                SpitProperties spitProp = new SpitProperties()
                {
                    spit = GameObject.Instantiate(spitPrefab, objectTransform.position + new Vector3(0.129999995f, 4.07000017f, 1.89999998f), Quaternion.identity),
                    frontPosition = objectTransform.position + new Vector3(0.129999995f, 4.07000017f, 1.89999998f),
                    middlePosition = GetMidpoint(objectTransform.position, targetTransform.position) + Vector3.up * 8.0f,
                    backPosition = targetTransform.position,
                    speedCounter = 0
                    
                };

                moveCounter = 0;
                //  spitObjects.Insert(spitObjects.Count, spitProp);
                this.spit = spitProp;
            }

            SpitProperties child = this.spit;
           // Debug.Log(animator.GetCurrentAnimatorStateInfo(0).length + " | " + animator.);
            moveCounter = (moveCounter + Time.deltaTime);
           // child.speedCounter = (child.speedCounter + Time.deltaTime);
            Debug.Log("Running spit to destination. " + child.speedCounter);
            child.spit.transform.position = CubicBezierCurve(child.frontPosition, child.middlePosition, child.backPosition, moveCounter);

            if (child.spit.transform.position == child.backPosition)
            {
                hasSpit = false;
              //  child.spit = null;
                child.speedCounter = 0;
                this.spit.spit = null;
                GameObject.Destroy(child.spit.gameObject,.5f);
            }
            return NodeState.RUNNING;
        }
        else if(!hasSpit && spit.spit == null)
        {
            Debug.Log("Not attacking");
            animator.SetBool("isAttacking", false);
        }


        return NodeState.FAILURE;
    }

    struct SpitProperties
    {
       public GameObject spit;
       public  Vector3 frontPosition, middlePosition, backPosition;
       public float speedCounter;
    }

    public void Spit()
    {
        if (spitObjects.Count > 0)
        {
            for (int i = 0; i < spitObjects.Count; i++)
            {
                SpitProperties child = spitObjects[i];
                child.speedCounter += Time.deltaTime;
                if(i == 0)
                {
                    Debug.Log("Counter : " + child.speedCounter);
                }
              //  child.speedCounter +=  .01f;
               // child.speedCounter = Mathf.Max(child.speedCounter, 1);
              //  Debug.Log(child.frontPosition + " | " + child.middlePosition + " | " + child.backPosition + " Speed counter : " + child.speedCounter);
                child.spit.transform.position = CubicBezierCurve(child.frontPosition, child.middlePosition, child.backPosition, child.speedCounter);
              
                if(child.speedCounter == 1f)
                {
                    Debug.Log("Destination reached!");
                    child.speedCounter = 0;
                    spitObjects.RemoveAt(i);
                    GameObject.Destroy(child.spit.gameObject);
                }
            }
        }
    }

    Vector3 GetMidpoint(Vector3 p1, Vector3 p2)
    {
        return new Vector3((p1.x + p2.x) / 2, (p1.y + p2.y) / 2, (p1.z + p2.z) / 2);
    }


    Vector3 CubicBezierCurve(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 p1 = Vector3.Lerp(a, b, t);
        Vector3 p2 = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(p1, p2, t);
    }
}
