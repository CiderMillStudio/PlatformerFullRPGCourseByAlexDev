using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGamePanel : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image swordThrowImage;
    [SerializeField] private Image blackholeImage;
    
    [Space]
    [Header("Flask Image Settings")]
    [SerializeField] private Image flaskImage; //this is the DARK color image (filled image)
    [SerializeField] private Image flaskOverlayImage; //this is the WHITE color (simple image)
    [SerializeField] private Sprite defaultFlaskImage;
    [SerializeField] private Color fullFlaskImageColor;


    private SkillManager skills;

    [Header("Souls Currency Info")]
    [SerializeField] private TextMeshProUGUI currentSouls;
    [SerializeField] private float soulsAmount;
    [SerializeField] private float increaseRate = 500;



    private void Start()
    {
        if (playerStats != null)
        {
            playerStats.onHealthChanged += UpdateHealthUI;
        }

        Debug.Log("UI_InGamePanel");
        skills = SkillManager.instance;

        
        
    }

    private void Update()
    {
        if (soulsAmount < PlayerManager.instance.GetCurrentCurrency())       
            soulsAmount += Time.deltaTime * increaseRate;       
        else
            soulsAmount = PlayerManager.instance.GetCurrentCurrency();

        currentSouls.text = Mathf.RoundToInt(soulsAmount).ToString("#,#"); //THIS IS SO COOL HOLY MOLEY!!!!!! LADKFJKLASDFALJFSDALKFSDLFJDS
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && skills.dash.dashUnlocked)
        {
            SetCooldownOf(dashImage);
        }

        if (Input.GetKeyDown(KeyCode.Q) && skills.parry.parryUnlocked)
        {
            SetCooldownOf(parryImage);
        }

        if (Input.GetKeyDown(KeyCode.F) && skills.crystal.crystalUnlocked)
        {
            SetCooldownOf(crystalImage);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && skills.swordThrow.swordUnlocked) 
        {
            SetCooldownOf(swordThrowImage);
        }

        if (Input.GetKeyDown(KeyCode.R) && skills.blackhole.blackholeUnlocked)
        {
            SetCooldownOf(blackholeImage);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && Inventory.instance.GetEquipment(EquipmentType.Flask) != null)
        {
            SetCooldownOf(flaskImage);
        }

        CheckCooldownOf(dashImage, skills.dash.cooldown);
        CheckCooldownOf(parryImage, skills.parry.cooldown);
        CheckCooldownOf(crystalImage, skills.crystal.cooldown);
        CheckCooldownOf(swordThrowImage, skills.swordThrow.cooldown);
        CheckCooldownOf(blackholeImage, skills.blackhole.cooldown);
        CheckCooldownOf(flaskImage, CheckForFlaskCooldown());
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHealth;
    }

    private void SetCooldownOf(Image _image)
    {
        if (_image.fillAmount <= 0)
        {
            _image.fillAmount = 1;
        }
    }

    private void CheckCooldownOf(Image _image, float _cooldown)
    {
        if (_image.fillAmount > 0)
        { 
            _image.fillAmount -= Time.deltaTime / _cooldown; 
        }
    }

    public void SetFlaskImage(ItemDataEquipment _flaskItemDataEquipment)
    {

        if (_flaskItemDataEquipment != null)
        {
            flaskImage.sprite = _flaskItemDataEquipment.icon;
            flaskOverlayImage.sprite = _flaskItemDataEquipment.icon;
            flaskImage.color = fullFlaskImageColor;
        }
        
        else
        {
            flaskImage.sprite = defaultFlaskImage;
            flaskOverlayImage.sprite = defaultFlaskImage;
        }

        
    }

    private float CheckForFlaskCooldown()
    {
        ItemDataEquipment currentFlask = Inventory.instance.GetEquipment(EquipmentType.Flask);

        if (currentFlask == null)
        {
            return float.MaxValue;
        }

        return currentFlask.itemCooldown;
    }


}
