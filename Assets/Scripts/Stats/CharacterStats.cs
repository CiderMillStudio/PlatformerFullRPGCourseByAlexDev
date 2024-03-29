using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public enum StatType //NEW (used to be in BuffEffect.cs)
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChance,
    critPower,
    health,
    armor,
    evasion,
    magicResistance,
    fireDamage,
    iceDamage,
    lightningDamage,
    soulsDropAmount
}
public class CharacterStats : MonoBehaviour
{

    #region List of Stats (el grande encyclopedia!)
    public List<Stat> listOfStats;

    #region Major Stats
    [Header("Major Stats")]

    public Stat strength; //1 pt increase damage by 1 and crit.power by 1%
    public Stat agility; //1% increase evasion by 1% and crit.chance by 1%
    public Stat intelligence; //1 pt increase magic damage by 2 pts and magic resistance by 3 pts? (just FYI We won't be making a bunch of spells)
    public Stat vitality; //1 pt increase health by 5 points

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

    [Header("Souls Stats")]
    public Stat soulsDropAmount;

    #endregion

    public int currentHealth;

    private EntityFX fx;

    public System.Action onHealthChanged;

    private bool isVulnerable;

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
        if (this.GetComponent<EnemyStats>() != null)
            listOfStats.Add(soulsDropAmount);

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



    public virtual void DoPhysicalDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;

        if (this.GetComponent<Enemy>() != null)
        {
            AudioManager.instance.PlaySFX(Random.Range(119, 123), null);
        }

        int totalDamage = damage.GetValue() + strength.GetValue();

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
            AudioManager.instance.PlaySFX(1, _targetStats.transform);
            //Debug.Log("total crit damage is: " + totalDamage);
        }

        _targetStats.TakeDamage(totalDamage, this.transform, true);


        //DoMagicalDamage(_targetStats, this.transform); //Remove this line if you don't want to apply magic hit on primary attack.

        //if current weapon has fire effect, do fire magical damage, otherwise DON'T!
        //DoMagicalDamage(_targetStats);

    }

    public virtual void TakeDamage(int _damage, Transform _damageSource, bool _fromPhyscialAttack)
    {

        DecreaseHealthBy(_damage, _fromPhyscialAttack);

        GetComponent<Entity>().DamageImpact(_damageSource, _damage); //We deleted DamageImpact() EVERYWHERE else, except for here!
        fx.StartCoroutine("FlashFX");

        //Debug.Log("TakeDamage(): " +_damage);



        if (currentHealth <= 0 && !isDead)
        {
            Die(false);
        }
    }

    protected virtual void DecreaseHealthBy(int _damage, bool _fromPhysicalAttack)
    {
        if (_fromPhysicalAttack)
            Debug.Log("Physical Attack Damage: " + _damage);
        else
            Debug.Log("Magic Attack Damage: " + _damage);

        if (isVulnerable)
        {
            _damage = Mathf.RoundToInt(_damage * 1.4f);
        }

        currentHealth -= _damage;

        if (onHealthChanged != null)
            onHealthChanged();
    }


    public void MakeVulnerableFor(float _duration)
    {
        StartCoroutine(VulnerableCoroutine(_duration));
    }

    private IEnumerator VulnerableCoroutine(float _duration)
    {
        isVulnerable = true;
        yield return new WaitForSeconds(_duration);
        isVulnerable = false;
    }

    public void IncreaseHealthBy(int _healAmount)
    {
        if (!(currentHealth == GetMaxHealthValue()))
            AudioManager.instance.PlaySFX(Random.Range(84, 87), null);

        int newHealth = Mathf.RoundToInt(currentHealth + _healAmount);
        currentHealth = Mathf.Clamp(newHealth, 0, GetMaxHealthValue());

        if (onHealthChanged != null)
            onHealthChanged();
    }



    protected virtual void Die(bool _killedByDeadZone)
    {
        isDead = true;
    }

    public void KillEntity(bool _killedByDeadZone)
    {
        Die(_killedByDeadZone);
        
    }

    #region Magical Damage and Ailments
    public virtual void DoMagicalDamage(CharacterStats _targetStats, Transform _magicDamageSource)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue() * 2;

        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);
      
        
        _targetStats.TakeDamage(totalMagicalDamage, _magicDamageSource, false);
        

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

            AudioManager.instance.PlaySFX(94, transform);
        }

        if (_chill && canApplyChill)
        {
            isChilled = _chill;
            chilledTimer = ailmentsDuration;

            float slowPercentage = 0.4f; //represents 40% decrease in moveSpeed (set up in player.cs or enemy.cs or
                                         //specificEnemy.cs)

            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuration);
            fx.ChillFxFor(ailmentsDuration);

            AudioManager.instance.PlaySFX(Random.Range(96, 99), transform);
            AudioManager.instance.PlaySFX(Random.Range(74, 79), transform);
        }

        if (_shock && canApplyShock)
        {

            if (!isShocked)
            {
                ApplyShock(_shock);
                AudioManager.instance.PlaySFX(93, transform);
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

            DecreaseHealthBy(igniteDamage, false);


            if (currentHealth <= 0 && !isDead)
            {
                Die(false);
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
    protected int CheckTargetArmor(CharacterStats _targetStats, int _totalDamage)
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

    public virtual void OnEvasion(Transform _enemyTransform)
    {
        AudioManager.instance.PlaySFX(Random.Range(66, 68), transform);
    }

    protected bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
        {
            totalEvasion += 20;
        }

        if (Random.Range(1, 100) < totalEvasion)
        {
            _targetStats.OnEvasion(transform);
            return true;
        }

        return false;
    }

    protected bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }

        return false;
    }

    protected int CalculateCriticalDamage(int _totalDamage)
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

    public Stat StatOfType(StatType _statType)
    {
        if (_statType == StatType.strength)
            return strength;
        else if (_statType == StatType.agility)
            return agility;
        else if (_statType == StatType.intelligence)
            return intelligence;
        else if (_statType == StatType.vitality)
            return vitality;
        else if (_statType == StatType.damage)
            return damage;
        else if (_statType == StatType.critChance)
            return critChance;
        else if (_statType == StatType.critPower)
            return critPower;
        else if (_statType == StatType.health)
            return maxHealth;
        else if (_statType == StatType.armor)
            return armor;
        else if (_statType == StatType.magicResistance)
            return magicResistance;
        else if (_statType == StatType.evasion)
            return evasion;
        else if (_statType == StatType.fireDamage)
            return fireDamage;
        else if (_statType == StatType.iceDamage)
            return iceDamage;
        else if (_statType == StatType.lightningDamage)
            return lightningDamage;

        else
            return null;
    }
}