using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{  
    // Only one shop should exist at any given time
    public static ShopManager Instance;

    // List of all shop items (scriptable objects)
    public List<ShopItem> ShopItems = new List<ShopItem>();

    public float ShopUpdateInterval = 60f; // Update interval in seconds

    public int MaxShopItems = 5;


    private int m_randomSeed = 0; // Seed is updated every x minutes
    private float m_randomTimer = 0; // Timer to update the seed

    // Active shop items
    private List<ShopItem> m_activeShopItems = new List<ShopItem>();
    // Active shop objects
    private List<GameObject> m_activeShopItemObjects = new List<GameObject>();


    // Don't destroy this object when loading a new scene
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateShopItems();
    }

    // Update is called once per frame
    void Update()
    {
        // Update the random seed every x seconds
        m_randomTimer += Time.deltaTime;
        if (m_randomTimer > ShopUpdateInterval)
        {
            print("Updating random seed");
            UpdateShopItems();

            m_randomTimer = 0;
            m_randomSeed = Random.Range(0, 0xFFFF);
        }

        
    }


    /// # Generate Shop Items
    ///
    /// Generate a list of shop items to be displayed in the shop,
    /// based on the current seed.
    void GenerateShopItems()
    {
        // Using the seed, generate a random list of shop items
        Random.InitState(m_randomSeed);

        // Duplicate the list of shop items
        List<ShopItem> pickableShopItems = new List<ShopItem>(ShopItems);

        m_activeShopItems.Clear();

        // pick random items from the list of shop items, up to the max number of items
        for (int i = 0; i < MaxShopItems; i++)
        {   
            // If there are no more items in the list, break out of the loop
            if (pickableShopItems.Count == 0)
            {
                break;
            }

            // Get a random index from the list of shop items
            int randomIndex = Random.Range(0, pickableShopItems.Count);
            // Add the shop item to the list of active shop items
            m_activeShopItems.Add(pickableShopItems[randomIndex]);

            // Remove the shop item from the list of pickable shop items
            pickableShopItems.RemoveAt(randomIndex);
        }

        print("Active shop items: " + m_activeShopItems.Count);
    }

    /// # Spawn Shop Items
    ///
    /// Spawns the currently active shop items in the shop.
    void SpawnShopItems()
    {
        // Only spawn if the current scene is the shope scene (in build)
#if UNITY_PLAYER
        if (SceneManager.GetActiveScene().name != "Shop"){
            return;
        }
#endif

        // Delete all active shop item objects
        foreach (GameObject shopItemObject in m_activeShopItemObjects)
        {
            Destroy(shopItemObject);
        }
        m_activeShopItemObjects.Clear();

        // Spawn new shop item objects
        foreach (ShopItem shopItem in m_activeShopItems)
        {
            print("Spawning shop item: " + shopItem.itemName);
            // Instantiate the shop item prefab
            GameObject shopItemObject = Instantiate(shopItem.itemPrefab);
            // Add the shop item object to the list of active shop item objects
            m_activeShopItemObjects.Add(shopItemObject);
        }
    }

    /// # Update Shop Items
    ///
    /// Update the shop items, then spawns the shop items.
    public void UpdateShopItems()
    {
        // Generate new shop items
        GenerateShopItems();
        // Spawn new shop item objects
        SpawnShopItems();
    }
}
