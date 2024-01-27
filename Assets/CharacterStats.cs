using UnityEngine;

public class CharacterStats : MonoBehaviour
{

   

    [Header("Major Stats")]

    public Stat strength; //1 pt increase damage by 1 and crit.power by 1%
    public Stat agility; //1 pt increase evasion by 1% and crit.chance by 1%
    public Stat intelligence; //1 pt increase magic damage by 1 and magic resistance by 1%.. or 3%? (just FYI We won't be making a bunch of spells)
    public Stat vitality; //1 pt increase health by 3 or 5 points?


    [Header("Defensive Stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;

    
    public Stat damage;
    


    [SerializeField] private int currentHealth;




    protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue();

    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        _targetStats.TakeDamage(totalDamage);
    }


    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;

        

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
        _totalDamage -= _targetStats.armor.GetValue();
        _totalDamage = Mathf.Clamp(_totalDamage, 0, int.MaxValue); //this lines prevent player being healed if armor is too high
        return _totalDamage;
    }
    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (Random.Range(1, 100) < totalEvasion)
        {
            return true;
        }

        return false;
    }
}
