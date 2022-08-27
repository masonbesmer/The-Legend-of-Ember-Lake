using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc : MonoBehaviour
{
    // Start is called before the first frame update
    //SphereCollider sphereCollider;


    bool playerAround;
    private AreaSound areaSound;
    private GameObject interactObject; 
    void Start()
    {
        interactObject = transform.GetChild(0).gameObject;
        areaSound = GetComponentInChildren<AreaSound>();
        //sphereCollider.
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerAround)
        {
            areaSound.PlaySound();
            interactObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            playerAround = true;
            interactObject.SetActive(true);
            Debug.Log("Player touched!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerAround=false;
            interactObject.SetActive(false);
            Debug.Log("Player left!");
        }
    }
}
