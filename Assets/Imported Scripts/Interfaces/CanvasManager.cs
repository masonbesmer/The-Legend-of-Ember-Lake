using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public enum SkillType {DEFAULT,SPREAD,EXPLODE,HOMING}

[System.Serializable]
public struct ItemProperties
{
    public GameObject holder;
    public GameObject prefab;
}
public class CanvasManager : MonoBehaviour
{
    
    const string MUSIC_VOLUME = "musicVolume";
    const string SFX_VOLUME = "sfxVolume";
    //[SerializeField] PlayerHealth playerHealth;

    [Header("Settings")]
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider musicSlider;

    [SerializeField] float defaultMusicVolume = 0;
    [SerializeField] float defaultSFXVolume = 0;


    [SerializeField] Image healthBar;

    [Header("Acorn")]
    [SerializeField] TextMeshProUGUI acornCountText;
    [SerializeField] int acornCount;

    PlayerHealth playerHealth;
    PlayerAbility playerAbility;

    [Header("Skill Sprites")]
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite explodeSprite;
    [SerializeField] Sprite homingSprite;
    [SerializeField] Sprite spreadSprite;

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

    private void Awake()
    {
        skillCount = 0;

        skillUIList = new Dictionary<SkillType, GameObject>();

        playerHealth = FindObjectOfType<PlayerHealth>();
        playerAbility = playerHealth.gameObject.GetComponent<PlayerAbility>();

        SubscribedActions();

        acornCountText.text = acornCount.ToString();

        audioMixer.SetFloat(MUSIC_VOLUME, defaultMusicVolume);
        audioMixer.SetFloat (SFX_VOLUME, defaultSFXVolume);

        sfxSlider.value = defaultSFXVolume;
        musicSlider.value = defaultMusicVolume;

        sfxSlider.onValueChanged.AddListener(val => audioMixer.SetFloat(SFX_VOLUME,val));
        musicSlider.onValueChanged.AddListener(val => audioMixer.SetFloat(MUSIC_VOLUME, val));

        AddSkill(SkillType.DEFAULT);
        ChangeActiveSkill(SkillType.DEFAULT);
 
        playerAbility.onAbilityReceived += AddSkill;
        playerAbility.onAbilityChange += ChangeActiveSkill;

  /*      AddSkill(SkillType.SPREAD);
        ChangeActiveSkill(SkillType.SPREAD);*/
    }

    void AddSkill(SkillType skillType)
    {
        GameObject skillUIObject = Instantiate(skillUIProperty.prefab,skillUIProperty.holder.transform);

        if(skillUIObject.transform.GetChild(0).TryGetComponent(out Image image))
        {
            if(GetSprite(skillType,out Sprite skillSprite))
            {
                image.sprite = skillSprite;
                skillUIList.Add(skillType,skillUIObject);
            }
        }
    }

    bool GetSprite(SkillType skillType, out Sprite skillSprite)
    {
        skillSprite = null;
        
        switch (skillType)
        {
            case SkillType.DEFAULT:
                skillSprite = defaultSprite;
                break;
            case SkillType.SPREAD:
                skillSprite = spreadSprite;
                break;
            case SkillType.EXPLODE:
                skillSprite = explodeSprite;
                break;
            case SkillType.HOMING:
                skillSprite = homingSprite;
                break;
            default:
                break;
        }
        return skillSprite != null;
    }

    void ChangeActiveSkill(SkillType skillType)
    {
        if(currentActiveSkillObject != null) currentActiveSkillObject.GetComponent<Image>().color = Color.white;
        currentActiveSkillObject = skillUIList[skillType];
        
        if(currentActiveSkillObject.TryGetComponent(out Image image))
        {
            image.color = Color.red;
        }

    }
  
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnDestroy()
    {

        playerAbility.onAbilityReceived -= AddSkill;
        playerAbility.onAbilityChange -= ChangeActiveSkill;
        playerHealth.onHurtAction -= OnPlayerHitAction;
        playerHealth.onDeathAction -= OnPlayerDeathAction;
        Pickupable.onPickupAction -= OnItemPickup;
    }

    void SubscribedActions()
    {
        Pickupable.onPickupAction += OnItemPickup;
        playerHealth.onHurtAction += OnPlayerHitAction;
        playerHealth.onDeathAction += OnPlayerDeathAction;

    }

    void OnPlayerHitAction(float healthPercentage)
    {
        healthBar.fillAmount = healthPercentage;
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
