using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class TaskFlyingChase : Node
{
    private float timeDelay;
    private bool isChasing;
    private bool hasStarted;
    private Vector3 frontPosition, middlePosition, backPosition;
    private Transform objectTransform, targetTransform;

    private float attackRange;
    private float counter;
    private Animator animator;
/*    public TaskFlyingChase()
    {
    }*/

    public TaskFlyingChase(Transform objectTransform, Transform targetTransform, float attackRange)
    {
        counter = 0;
        hasStarted = false;
        timeDelay = 3.0f;
        isChasing = false;
        this.attackRange = attackRange;
        backPosition = Vector3.positiveInfinity;
        //  this.defaultPosition = objectTransform.position;
        // hasKey = false;
        this.objectTransform = objectTransform;
        this.targetTransform = targetTransform;
        animator = objectTransform.GetComponent<Animator>();
        //   this.attackRange = attackRange;
    }
    public override NodeState Evaluate()
    {
        if (parent.HasKey("chase") && (bool)parent.GetData("chase"))
        {
            if (!isChasing)
            {
                isChasing = true;
                hasStarted = true;
                frontPosition = objectTransform.position;
                middlePosition = targetTransform.position - Vector3.down;
                Vector3 direction = ExtensionMethodsBT.GetXZDirection(objectTransform.position, targetTransform.position);
                objectTransform.LookAt(targetTransform);
                backPosition = targetTransform.position + (direction * attackRange) + (Vector3.up * 8.0f);
                //  GameObject.Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), backPosition, Quaternion.identity);

            }
            else
            {
                Debug.Log("Chasing");

                animator.SetBool("fly", true);
                objectTransform.LookAt(targetTransform);
                counter = (counter + Time.deltaTime);
                objectTransform.position = CubicBezierCurve(frontPosition, middlePosition, backPosition, counter);
                objectTransform.LookAt(objectTransform.forward);

                if (objectTransform.position == backPosition)
                {
                    counter = 0;
                    Debug.Log("destination reach");
                    hasStarted = false;
                    parent.SetData("chase", false);
                    isChasing = false;
                    //  GameObject.Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), Vector3.back, Quaternion.identity);
                }
            }

        }

        //parent.SetData("chase", false);
        return NodeState.SUCCESS;
    }

    Vector3 CubicBezierCurve(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 p1 = Vector3.Lerp(a, b, t);
        Vector3 p2 = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(p1, p2, t);
    }
}
