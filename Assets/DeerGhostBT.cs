using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
public class DeerGhostBT : MonoBehaviour
{
    private Vector3 destinationVector;
    private float speed = 1.0f;
    private bool canRush;
    private Terrain terrain;
    private void Awake()
    {
        destinationVector = transform.position + transform.forward * 25.0f;
    }
    private void Start()
    {
        
    }

    private void Update()
    {

        if(canRush) transform.position = Vector3.MoveTowards(new Vector3(transform.position.x,0,transform.position.z), destinationVector, Time.deltaTime * speed) + new Vector3(0, .5f + terrain.SampleHeight(transform.position), 0);
        bool isWithinGoal = ExtensionMethodsBT.GetDistance(ExtensionMethodsBT.GetXZVector(transform.position), ExtensionMethodsBT.GetXZVector(destinationVector)) <= 4.0f;

        if (isWithinGoal)
        {
            Destroy(gameObject);
        }
    }

    public void SetDeer(float speed, Terrain terrain)
    {
        this.terrain = terrain;
        this.speed = speed;
        canRush = true;
    }

}
