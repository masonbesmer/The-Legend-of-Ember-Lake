using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using Tree = BehaviourTree.Tree;
using UnityEngine.AI;

public class FrogBehaviourTree : Tree
{

    [SerializeField] GameObject spitPrefab;
    [SerializeField] float chaseRange;
    [SerializeField] float returnRange;
    [SerializeField] float attackRange;
    [SerializeField] float attackRate;

    [SerializeField] float jumpRange;
    [SerializeField] float jumpRate;
    [SerializeField] float checkAheadRange;
    [SerializeField] float raycastInBetweenDistance;
    [SerializeField] LayerMask obstacleMask;

    private Vector3 frontPosition, middlePosition, backPosition;
    public float speed;
    private float lookAround;
    private bool canSpit;
    private GameObject spitObject;
    //[SerializeField] private Terrain terrain;
    protected override Node SetupTree()
    {
        //terrain.SampleHeight()
      //  Spit();
        Node root = new Selector(new List<Node> { 
          //  new TaskAttack(objectTransform.GetComponent<NavMeshAgent>(), targetTransform,objectTransform,attackRange),
            new FrogAtttackTask(objectTransform,targetTransform,objectTransform.GetComponent<Animator>(), spitPrefab,attackRange,attackRate),
            new FrogChaseTask(targetTransform,objectTransform,chaseRange,jumpRange, jumpRate, checkAheadRange, attackRange, raycastInBetweenDistance,obstacleMask)
            //  new TaskIdle(objectTransform.GetComponent<NavMeshAgent>(),objectTransform,targetTransform)
        });

        return root;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, jumpRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, checkAheadRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * checkAheadRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * checkAheadRange - (transform.right*raycastInBetweenDistance)));
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * checkAheadRange + (transform.right* raycastInBetweenDistance)));
    }

    float moveCounter = 0;
    public void Spit()
    {
        
        if (canSpit)
        {
            Debug.Log("Running");
            moveCounter = (moveCounter + Time.deltaTime);
            spitObject.transform.position = CubicBezierCurve(frontPosition, middlePosition, backPosition, moveCounter);

            if(spitObject.transform.position == backPosition)
            {
                canSpit = false;
                moveCounter = 0;
                Destroy(spitObject);
            }
        }
    }


    public void ActivateSpit()
    {
        canSpit = true;
        spitObject = Instantiate(spitPrefab, transform.position + new Vector3(0.119999997f, 2.53999996f, 3.16000009f), Quaternion.identity);
        frontPosition = transform.position;
        middlePosition = GetMidpoint(transform.position, targetTransform.position);
        backPosition = targetTransform.position;
    }

    public void GetPlayerPosition()
    {
       // playerPosition = targetTransform.position;
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
