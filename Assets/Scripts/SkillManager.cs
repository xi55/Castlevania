using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public Dash_Skill dashSkill { get; private set; }
    public Clone_Skill cloneSkill { get; private set; }
    public Sword_Skill swordSkill { get; private set; }
    public Crystal_Skill crystalSkill { get; private set; }

    public BlackHole_Skill blackHoleSkill { get; private set; }
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(instance.gameObject);
    }
    private void Start()
    {
        dashSkill = GetComponent<Dash_Skill>();
        cloneSkill = GetComponent<Clone_Skill>();
        swordSkill = GetComponent<Sword_Skill>();
        blackHoleSkill = GetComponent<BlackHole_Skill>();
        crystalSkill = GetComponent<Crystal_Skill>();
    }
}
