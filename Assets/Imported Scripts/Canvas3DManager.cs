using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Canvas3DManager : MonoBehaviour
{

    /*    const string MUSIC_VOLUME = "musicVolume";
        const string SFX_VOLUME = "sfxVolume";
        //[SerializeField] PlayerHealth playerHealth;

        [Header("Settings")]
        [SerializeField] AudioMixer audioMixer;
        [SerializeField] Slider sfxSlider;
        [SerializeField] Slider musicSlider;

        [SerializeField] float defaultMusicVolume = 0;
        [SerializeField] float defaultSFXVolume = 0;


        [SerializeField] Image healthBar;*/

    [Header("Acorn")]
    [SerializeField] TextMeshProUGUI acornCountText;
    [SerializeField] int acornCount;

    PlayerHealth playerHealth;
    PlayerAbility playerAbility;

    [Header("Skill Sprites")]
    [SerializeField] GameObject defaultGameObject;
    [SerializeField] GameObject explodeGameObject;
    [SerializeField] GameObject homingGameObject;
    [SerializeField] GameObject spreadGameObject;

    [SerializeField] Material redMaterial, grayMaterial;

    // [SerializeField] GameObject skillUIHolder;
    //  [SerializeField] GameObject skillUIPrefab;

    /*    [SerializeField] GameObject heartUIHolder;
        [SerializeField] GameObject heartPrefab;

        [SerializeField] GameObject armorUIHolder;
        [SerializeField] GameObject armorPrefab;
    */
    [Header("Properties")]
    [SerializeField] ItemProperties skillUIProperty;

    [SerializeField] ItemProperties heartUIProperty;

    [SerializeField] ItemProperties armorUIProperty;

    private Dictionary<SkillType, GameObject> skillUIList;
    private GameObject currentActiveSkillObject;
    //private List<GameObject> skillUIList;

    int skillCount;

    int healthCounter = 0;
    Animator healthAnimator;
    GameObject[] healthObjects;


    public Action<float> OnMiniCurrentHealthUpdateAction;
    private void Awake()
    {
        skillCount = 0;

        skillUIList = new Dictionary<SkillType, GameObject>();

        playerHealth = FindObjectOfType<PlayerHealth>();
        playerAbility = playerHealth.gameObject.GetComponent<PlayerAbility>();
        SubscribedActions();
        InitializeHeart();

        acornCountText.text = acornCount.ToString();

        /*        audioMixer.SetFloat(MUSIC_VOLUME, defaultMusicVolume);
                audioMixer.SetFloat(SFX_VOLUME, defaultSFXVolume);

                sfxSlider.value = defaultSFXVolume;
                musicSlider.value = defaultMusicVolume;

                sfxSlider.onValueChanged.AddListener(val => audioMixer.SetFloat(SFX_VOLUME, val));
                musicSlider.onValueChanged.AddListener(val => audioMixer.SetFloat(MUSIC_VOLUME, val));
        */

        AddSkill(SkillType.DEFAULT);
        ChangeActiveSkill(SkillType.DEFAULT);

        playerAbility.onAbilityReceived += AddSkill;
        playerAbility.onAbilityChange += ChangeActiveSkill;

        /*      AddSkill(SkillType.SPREAD);
              ChangeActiveSkill(SkillType.SPREAD);*/
    }

    void InitializeHeart()
    {
        int maxHearts = playerHealth.GetMaxHealth() / 6;
        healthObjects = new GameObject[maxHearts];

        for (int i = 0; i < maxHearts; i++)
        {
            healthObjects[i] = Instantiate(heartUIProperty.prefab, heartUIProperty.holder.transform);
        }

        healthAnimator = healthObjects[healthCounter].GetComponent<Animator>();
    }


    void OnPlayerHeal(float miniCurrentHealth, float miniMaxHealth, float healAmount)
    {

        if (miniCurrentHealth < miniMaxHealth)
        {
            Debug.Log("Healing first");
            if (healAmount >= miniMaxHealth - miniCurrentHealth)
            {
                Debug.Log("Heal remaining parts of a heart to full");
                healAmount -= miniMaxHealth - miniCurrentHealth;
                miniCurrentHealth = miniMaxHealth;
                healthAnimator = healthObjects[healthCounter].GetComponent<Animator>();
                healthAnimator.Play("playerHit", 0, 0);
            }
            else
            {
                Debug.Log("Small heal");
                miniCurrentHealth += healAmount;
                healAmount = 0;
                healthAnimator = healthObjects[healthCounter].GetComponent<Animator>();
                healthAnimator.Play("playerHit", 0, (miniMaxHealth - miniCurrentHealth) / miniMaxHealth);
            }
        }

        float healRemaining = healAmount;


        if (healRemaining > 0)
        {
            Debug.Log("Remaining heal" + healRemaining);

            int fullHeart = ((int)(healRemaining / miniMaxHealth));

            if (fullHeart > 0)
            {
                for (int i = 0; i < fullHeart; i++)
                {
                    if (healthCounter - 1 >= 0)
                    {
                        Debug.Log("Health counter " + healthCounter);
                        healthCounter--;
                        miniCurrentHealth = miniMaxHealth;
                        healthAnimator = healthObjects[healthCounter].GetComponent<Animator>();
                        healthAnimator.Play("playerHit", 0, 0);
                    }
                }
            }
            else
            {

                if (healthCounter - 1 >= 0)
                {
                    Debug.Log("Remaining heal in counter");
                    healthCounter--;
                    miniCurrentHealth = healRemaining;
                    healthAnimator = healthObjects[healthCounter].GetComponent<Animator>();
                    healthAnimator.Play("playerHit", 0, (miniMaxHealth - miniCurrentHealth) / miniMaxHealth);
                }
            }

        }

        OnMiniCurrentHealthUpdateAction?.Invoke(miniCurrentHealth);
    }

    void OnPlayerHit(float miniCurrentHealth, float miniMaxHealth)
    {
       // Debug.Log("Hit " + miniCurrentHealth + " | " + miniMaxHealth);

        if ((miniMaxHealth - miniCurrentHealth) >= miniMaxHealth)
        {
            miniCurrentHealth = miniMaxHealth;
            healthAnimator.Play("playerHit", 0, 1);
            // healthObjects[healthCounter].SetActive(false);

            if (healthCounter + 1 < healthObjects.Length)
            {
                healthCounter++;
                healthAnimator = healthObjects[healthCounter].GetComponent<Animator>();
                // healthAnimator.Play("playerHit", 0, 0);
            }
        }

        healthAnimator.Play("playerHit", 0, (miniMaxHealth - miniCurrentHealth) / miniMaxHealth);
    }

    void AddSkill(SkillType skillType)
    {
        // GameObject skillUIObject = Instantiate(, skillUIProperty.holder.transform);
        if (GetGameObject(skillType, out GameObject skillGameObject))
        {
            skillUIList.Add(skillType, Instantiate(skillGameObject, skillUIProperty.holder.transform));
        }
    }

    bool GetGameObject(SkillType skillType, out GameObject skillSprite)
    {
        skillSprite = null;

        switch (skillType)
        {
            case SkillType.DEFAULT:
                skillSprite = defaultGameObject;
                break;
            case SkillType.SPREAD:
                skillSprite = spreadGameObject;
                break;
            case SkillType.EXPLODE:
                skillSprite = explodeGameObject;
                break;
            case SkillType.HOMING:
                skillSprite = homingGameObject;
                break;
            default:
                break;
        }
        return skillSprite != null;
    }

    void ChangeActiveSkill(SkillType skillType)
    {

        if (currentActiveSkillObject != null) currentActiveSkillObject.GetComponent<MeshRenderer>().material = grayMaterial;

        currentActiveSkillObject = skillUIList[skillType];

        if (currentActiveSkillObject.TryGetComponent(out MeshRenderer meshRenderer))
        {
            meshRenderer.material = redMaterial;
        }

    }

    private void OnDestroy()
    {
        playerHealth.OnHealAction -= OnPlayerHeal;
        playerAbility.onAbilityReceived -= AddSkill;
        playerAbility.onAbilityChange -= ChangeActiveSkill;
     //   playerHealth.onHurtAction -= OnPlayerHitAction;
        playerHealth.onDeathAction -= OnPlayerDeathAction;
        Pickupable.onPickupAction -= OnItemPickup;
    }

    void SubscribedActions()
    {

        Pickupable.onPickupAction += OnItemPickup;
        playerHealth.OnHealAction += OnPlayerHeal;
        playerHealth.OnHitAction += OnPlayerHit;
      //  playerHealth.onHurtAction += OnPlayerHitAction;
        playerHealth.onDeathAction += OnPlayerDeathAction;

    }

    void OnPlayerDeathAction()
    {

    }

    void OnItemPickup(NonSkillItemTypes nonSkillItemTypes)
    {
        switch (nonSkillItemTypes)
        {
            case NonSkillItemTypes.ACORNS:
                acornCount++;
                acornCountText.text = acornCount.ToString();
                break;
            case NonSkillItemTypes.HEART:
                Instantiate(heartUIProperty.prefab, heartUIProperty.holder.transform);
                break;
            case NonSkillItemTypes.ARMOR:
                Instantiate(armorUIProperty.prefab, armorUIProperty.holder.transform);
                break;
        }
    }
}
