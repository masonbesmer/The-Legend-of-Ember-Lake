using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectTest : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] LayerMask mask;

    RectTransform cube;
    bool cubeFound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out RaycastHit hit,40,mask) && !cubeFound)
        {
            cubeFound = true;
            cube = hit.collider.gameObject.GetComponent<RectTransform>();
            Debug.Log("Found it" + hit.collider.name);
        }

        if (cube != null)
        {
            cube.localPosition = Mathf.Clamp(cam.ScreenToWorldPoint(Input.mousePosition).x * 15f,-80,130) * Vector3.right;  
        }

        if (Input.GetMouseButtonUp(0))
        {
            cubeFound = false;
            cube = null;
        }

    }
}
