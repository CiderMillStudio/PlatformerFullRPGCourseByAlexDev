using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public DashSkill dash { get; private set; }
    public CloneSkill clone { get; private set; }

    public SwordSkill swordThrow { get; private set; }

    public BlackholeSkill blackhole { get; private set; }


    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
        
    }

    private void Start()
    {
        dash = GetComponent<DashSkill>();
        clone = GetComponent<CloneSkill>();
        swordThrow = GetComponent<SwordSkill>();
        blackhole = GetComponent<BlackholeSkill>();
    }



}
