using Unity.VisualScripting;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public int currentHealth;
    
    [Header("Major Stats")]

    public Stat strength; //1 pt increase damage by 1 and crit.power by 1%
    public Stat agility; //1 pt increase evasion by 1% and crit.chance by 1%
    public Stat intelligence; //1 pt increase magic damage by 1 and magic resistance by 1%.. or 3%? (just FYI We won't be making a bunch of spells)
    public Stat vitality; //1 pt increase health by 3 or 5 points?

    [Header("Offensive Stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;   //default value 150 (%)



    [Header("Defensive Stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic Stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;

    public bool isIgnited; //does damage over time
    public bool isChilled; //reduce armor by 20%~
    public bool isShocked; //reduce accuracy by 20%~

    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    private float igniteDamageCooldown = 0.3f;
    private float igniteDamageTimer;
    private int igniteDamage;


    public System.Action onHealthChanged;



    protected virtual void Start()
    {
        currentHealth = GetMaxHealthValue();
        Debug.Log("character stats called");
        critPower.SetDefaultValue(150);

    }

    protected virtual void Update()
    {
        igniteDamageTimer -= Time.deltaTime;

        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;


        if (ignitedTimer < 0)
            isIgnited = false;
        
        if (igniteDamageTimer < 0 && isIgnited)
        {
            Debug.Log("Take burn damage " + igniteDamage);

            DecreaseHealthBy(igniteDamage);
            
            if (currentHealth <= 0)
            {
                Die();
            }

            igniteDamageTimer = igniteDamageCooldown;
        }

        if (chilledTimer < 0 )
            isChilled = false;

        if (shockedTimer < 0 )
            isShocked = false;


    }



    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
            //Debug.Log("total crit damage is: " + totalDamage);
        }

        _targetStats.TakeDamage(totalDamage); //right now, we're commenting this out to test magical damage.
        DoMagicalDamage(_targetStats);

    }

    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();

        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);
        _targetStats.TakeDamage(totalMagicalDamage);

        //This is where we'll write code for Ailments (next video!)
        //The way this works is the ailment with "the most damage attributed to it's category" is the ailment that gets applied
        //e.g. if fire, shock, and chill damage is dealt by a sword, but "fire damage" happens to be the highest
        //damage type dealt by the sword attack, the "isIgnited" ailment will be applied, but not the other ailments.

        if(Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0) //avoids infinite while loop.
        {
            return;
        }

        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < 0.5f && _fireDamage > 0) //Random.value gives you a random float value between 0 and 1
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("Applied Ignite");
                return;
            }
            if (Random.value < 0.5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("Applied Chill");
                return;
            }
            if (Random.value < 0.5f && _lightningDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("Applied Shock");
                return;
            }
        }

        if(canApplyIgnite)
        {
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * 0.20f));
        }

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;

    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        if (isIgnited || isChilled || isShocked)
        {
            return;
        }

        if (_ignite)
        {
            isIgnited = _ignite;
            ignitedTimer = 3f;
        }

        if (_chill)
        {
            isChilled = _chill;
            chilledTimer = 3f;
        }

        if (_shock)
        {
            isShocked = _shock;
            shockedTimer = 3f;
        }
    }
    



    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealthBy(_damage);

        Debug.Log("TakeDamage(): " +_damage);

        

        if (currentHealth <= 0) 
        {
            Die();
        }
    }

    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHealth -= _damage;

        if (onHealthChanged != null)
            onHealthChanged();
    }

    protected virtual void Die()
    {

    }

    private int CheckTargetArmor(CharacterStats _targetStats, int _totalDamage)
    {
        if (_targetStats.isChilled)
        {
            _totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * 0.8f);
            Debug.Log("Total damage of Chilled Entity: " + _totalDamage);
        }
        else
            _totalDamage -= _targetStats.armor.GetValue();

        _totalDamage = Mathf.Clamp(_totalDamage, 0, int.MaxValue); //this lines prevent target being healed if armor is too high
        Debug.Log("Total damage after clamp : " + _totalDamage);
        return _totalDamage;
    }
    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
        {
            totalEvasion += 20;
        }

        if (Random.Range(1, 100) < totalEvasion)
        {
            return true;
        }

        return false;
    }

    private bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0,100) <= totalCriticalChance)
        {
            return true;
        }    

        return false;
    }

    private int CalculateCriticalDamage(int _totalDamage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;

        float critDamage = _totalDamage * totalCritPower;
        //Debug.Log("crit damage before round up: " + critDamage);

        return Mathf.RoundToInt(critDamage);
    }

    public int GetMaxHealthValue()
    {
        return (maxHealth.GetValue() + (5 * vitality.GetValue()));
    }    
}


//notes on slider: (attached a Slider UI object to an Enemy Skeleton)
//1) deleted the "Handle Slide Area" gameobject inside the Slider gameobject.
//2) dragged UI_HealthBar png file to "Source Image" component of the Image component of the 'Background'
//gameobject of the slider.
//3) change Canvas Render mode to WorldSpace
//4) change sorting layer to Player, because we always want to see these healthbars of enemy and player.
//5) Change canvas scale to 0.005 for x, y, and z. Set Rect Transform Pos X and Pos Y to 0, 0.
//6) Rename Canvas gameobject to "Enemy_Status_UI", this is where we'll show health and ailments.
//7) Resize it to your liking using the Rect Tool (press 'T' on keyboard)
//8) in Slider, in the "Rect Transform" component, use the centering tool (while pressing alt AND shift) to
//choose the bottom right corner option. Now, the HealthBar png image will take up the whole rect transform, 
//as to how big you make it.
//9) in the "FillArea" game object, open "Fill" and use the Rect Transform ('T') tool to make the Fill color 
//occupy the whole Slider box.
//10) change Fill "Image" color to Red, or whatever color you'd like (we're making it red because we're making a health bar).
//Also, delete the UI_Sprite that autopopulates the "Source Image" section of the Fill "Image" component
//11) if the UI_HealthBar is invisible because it's being blocked by the Fill of the Fill Area gameobject,
//just drag the "Background" child of the slider BELOW the "Fill Area" child in the heirarchy, now the bar frame should be visible.
//12) Make sure the slider value is set to 1.0
//13) Check the Slider game obeject, and in the "slider" component, ensure that Fill Rect is set to "Fill (Rect Transform)"
//14) but there's a problem!! when the skeleton reverses direction (turns around), the whole EnemySkeleton (and all its children,
//including the UI slider) flips directions!!!!) We can solve this with something called "EVENTS!" idk what this is...
//Talk about Events, and Script Execution Orders!
