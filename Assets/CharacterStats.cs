using Unity.VisualScripting;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{


    [SerializeField] private int currentHealth;

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






    protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue();
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
            currentHealth -= igniteDamage;
            
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
            ignitedTimer = 100f;
        }

        if (_chill)
        {
            isChilled = _chill;
            chilledTimer = 100f;
        }

        if (_shock)
        {
            isShocked = _shock;
            shockedTimer = 100f;
        }
    }
    



    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;

        Debug.Log("TakeDamage(): " +_damage);

        

        if (currentHealth <= 0) 
        {
            Die();
        }
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
}
