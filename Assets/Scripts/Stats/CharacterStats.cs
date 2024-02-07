using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    #region List of Stats (el grande encyclopedia!)
    public List<Stat> listOfStats;

    #region Major Stats
    [Header("Major Stats")]

    public Stat strength; //1 pt increase damage by 1 and crit.power by 1%
    public Stat agility; //1 pt increase evasion by 1% and crit.chance by 1%
    public Stat intelligence; //1 pt increase magic damage by 1 and magic resistance by 1%.. or 3%? (just FYI We won't be making a bunch of spells)
    public Stat vitality; //1 pt increase health by 3 or 5 points?

    #endregion

    #region Offensive Stats
    [Header("Offensive Stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;   //default value 150 (%)

    #endregion

    #region Defensive Stats
    [Header("Defensive Stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    #endregion

    #region Magic Stats
    [Header("Magic Stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;


    [SerializeField] private float ailmentsDuration = 4f;

    public bool isIgnited; //does damage over time
    public bool isChilled; //reduce armor by 20%~
    public bool isShocked; //reduce accuracy by 20%~

    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    private float igniteDamageCooldown = 0.3f;
    private float igniteDamageTimer;
    private int igniteDamage;
    [SerializeField] private GameObject shockThunderstrikePrefab;
    private int shockDamage; //amount of damage done when thunder strikes closest enemy.

    #endregion;

    #endregion

    public int currentHealth;

    private EntityFX fx;

    public System.Action onHealthChanged;

    public bool isDead { get; private set; }


    private void Awake()
    {
        listOfStats = new List<Stat>();
        listOfStats.Add(strength);
        listOfStats.Add(agility);
        listOfStats.Add(intelligence);
        listOfStats.Add(vitality);

        listOfStats.Add(damage);
        listOfStats.Add(critChance);
        listOfStats.Add(critPower);

        listOfStats.Add(maxHealth);
        listOfStats.Add(armor);
        listOfStats.Add(evasion);
        listOfStats.Add(magicResistance);

        listOfStats.Add(fireDamage);
        listOfStats.Add(iceDamage);
        listOfStats.Add(lightningDamage);

    }
    protected virtual void Start()
    {
        fx = GetComponentInChildren<EntityFX>();
        currentHealth = GetMaxHealthValue();
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


        if (chilledTimer < 0)
            isChilled = false;

        if (shockedTimer < 0)
            isShocked = false;

        if (isIgnited)
            ApplyIgniteDamage();

    }

    public virtual void IncreaseStatBy(int _modifier, float _duration, Stat _statToModify)
    {
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statToModify));
    }

    private IEnumerator StatModCoroutine(int _modifier, float _duration, Stat _statToModify)
    {
        _statToModify.AddModifier(_modifier);

        yield return new WaitForSeconds(_duration);

        _statToModify.RemoveModifier(_modifier);
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

        _targetStats.TakeDamage(totalDamage);


        DoMagicalDamage(_targetStats); //Remove this line if you don't want to apply magic hit on primary attack.

        //if current weapon has fire effect, do fire magical damage, otherwise DON'T!
        //DoMagicalDamage(_targetStats);

    }

    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealthBy(_damage);

        GetComponent<Entity>().DamageImpact(); //We deleted DamageEffect() EVERYWHERE else, except for here!
        fx.StartCoroutine("FlashFX");

        //Debug.Log("TakeDamage(): " +_damage);



        if (currentHealth <= 0 && !isDead)
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

    public void IncreaseHealthBy(int _healAmount)
    {
        currentHealth += _healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth.GetValue());

        if (onHealthChanged != null)
            onHealthChanged();
    }



    protected virtual void Die()
    {
        isDead = true;
    }

    #region Magical Damage and Ailments
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

        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0) //avoids infinite while loop.
        {
            return;
        }

        AttemptToApplyAilments(_targetStats, _fireDamage, _iceDamage, _lightningDamage);
    }

    private void AttemptToApplyAilments(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightningDamage)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < 0.35f && _fireDamage > 0) //Random.value gives you a random float value between 0 and 1
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("Applied Ignite");
                break;
            }
            if (Random.value < 0.5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("Applied Chill");
                break;
            }
            if (Random.value < 0.65f && _lightningDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("Applied Shock");
                break;
            }
        }


        if (canApplyIgnite)
        {
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * 0.20f));
        }

        if (canApplyShock)
            _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightningDamage * 0.20f));

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;
    public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;


    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {

        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;


        /*        if (isIgnited || isChilled || isShocked)
                {
                    return;
                }*/

        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = ailmentsDuration;
            fx.IgniteFxFor(ailmentsDuration);
        }

        if (_chill && canApplyChill)
        {
            isChilled = _chill;
            chilledTimer = ailmentsDuration;

            float slowPercentage = 0.4f; //represents 40% decrease in moveSpeed (set up in player.cs or enemy.cs or
                                         //specificEnemy.cs)

            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuration);
            fx.ChillFxFor(ailmentsDuration);
        }

        if (_shock && canApplyShock)
        {

            if (!isShocked)
            {
                ApplyShock(_shock);
            }

            else
            {
                if (GetComponent<Player>()) // != null) // if an enemy hits a shocked PLAYER, no thunderbolt should be made.
                    return;

                HitNearestTargetWithNewShockStrike();

            }

        }
    }

    private void ApplyIgniteDamage()
    {
        if (igniteDamageTimer < 0)
        {

            DecreaseHealthBy(igniteDamage);


            if (currentHealth <= 0 && !isDead)
            {
                Die();
            }

            igniteDamageTimer = igniteDamageCooldown;
        }
    }
    public void ApplyShock(bool _shock)
    {
        if (isShocked)
            return;

        isShocked = _shock;
        shockedTimer = ailmentsDuration;

        fx.ShockFxFor(ailmentsDuration);
    }

    private void HitNearestTargetWithNewShockStrike()
    {
        //first, find nearest enemy target.

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 100);
        float closestDistance = Mathf.Infinity;

        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance && hit.transform != this.transform)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }

                if (!closestEnemy)
                    closestEnemy = transform;
            }
        }


        //next, instantiate and setup shock strike

        if (closestEnemy != null)
        {

            GameObject newShockStrike = Instantiate(shockThunderstrikePrefab, transform.position, Quaternion.identity);
            ShockStrikeController newShockStrikeScript = newShockStrike.GetComponent<ShockStrikeController>();
            newShockStrikeScript.Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>()); //PASSING INFO TO THUNDERBOLT!
        }
    }
    #endregion

    #region Stat Calculations
    private int CheckTargetArmor(CharacterStats _targetStats, int _totalDamage)
    {
        if (_targetStats.isChilled)
        {
            _totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * 0.8f);
            //Debug.Log("Total damage of Chilled Entity: " + _totalDamage);
        }
        else
            _totalDamage -= _targetStats.armor.GetValue();

        _totalDamage = Mathf.Clamp(_totalDamage, 0, int.MaxValue); //this lines prevent target being healed if armor is too high
        //Debug.Log("Total damage after clamp : " + _totalDamage);
        return _totalDamage;
    }

    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
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

        if (Random.Range(0, 100) <= totalCriticalChance)
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


    #endregion
}