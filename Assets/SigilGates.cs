using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SigilGates : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] bool hasStarSigil, hasSunSigil, hasMoonSigil;
    private bool starGateOpen, sunGateOpen, moonGateOpen;
    Animation animator;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag != null) {

            string tag = col.gameObject.tag;

            if (tag.Contains("Gate")) {
                animator = col.gameObject.GetComponent<Animation>();
                if (tag.Contains("Star")) {
                    if (player.GetComponent<SigilGates>().hasStarSigil) {
                        animator.Play("Open");
                        player.GetComponent<SigilGates>().starGateOpen = true;
                    }
                }
                if (tag.Contains("Sun")) {
                    if (player.GetComponent<SigilGates>().hasSunSigil) {
                        animator.Play("Open");
                        player.GetComponent<SigilGates>().sunGateOpen = true;
                    }
                }
                if (tag.Contains("Moon")) {
                    if (player.GetComponent<SigilGates>().hasMoonSigil) {
                        animator.Play("Open");
                        player.GetComponent<SigilGates>().moonGateOpen = true;
                    }
                }
            }

            else if (tag.Contains("Sigil")) {
                if (tag.Contains("Star")) {
                    player.GetComponent<SigilGates>().hasStarSigil = true;
                    GameObject.Destroy();
                }
                if (tag.Contains("Sun")) {
                    player.GetComponent<SigilGates>().hasSunSigil = true;
                    GameObject.Destroy();
                }
                if (tag.Contains("Moon")) {
                    player.GetComponent<SigilGates>().hasMoonSigil = true;
                    GameObject.Destroy();
                }
            }
        }
        else return;
    }
}
