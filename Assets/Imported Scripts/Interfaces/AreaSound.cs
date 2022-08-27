using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSound : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource audioSource;
    GameObject player;
    bool playerInside;
    SphereCollider sphereCollider;


    [SerializeField] private bool interactable; //Turn this on if you want the player to interact first before playing the sound.
    [SerializeField] private float minimumVolume,maximumVolume;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInside)
        {
            audioSource.volume = Mathf.Max(minimumVolume,Mathf.Min(1 - Vector3.Distance(transform.position, player.transform.position) / sphereCollider.radius,maximumVolume));
        } 
    }

    //This method can be access by the object holding it, usually an NPC, for interaction purposes. 
    public void PlaySound()
    {
        if(player != null)
        {
            playerInside = true;

            if (!audioSource.isPlaying)
                audioSource.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            player = other.gameObject;
           
            if (!interactable)
            {
                playerInside = true;

                if (!audioSource.isPlaying)
                    audioSource.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            player = null;
            playerInside = false;
            audioSource.volume = 0;
          //  audioSource.Play();
        }
    }
}
