using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class InteractableShopItem : MonoBehaviour
{
    public ShopItem shopItem;

    SphereCollider sphereCollider;

    void Start()
    {
        // Attemp to get the sphere collider.
        if (!TryGetComponent(out sphereCollider))
        {
            // Create a sphere collider if one doesn't exist.
            sphereCollider = gameObject.AddComponent<SphereCollider>();
        }

        sphereCollider.radius = 2f;
        sphereCollider.isTrigger = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player entered shop item");
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player exited shop item");
        }
    }
}
