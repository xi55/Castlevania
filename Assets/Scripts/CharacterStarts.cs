using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStarts : MonoBehaviour
{
    [Header("Major States")]
    public Start strength;
    public Start agility;
    public Start intelligence;
    public Start vitality;

    [Header("Offensive States")]
    public Start damage;
    public Start critChance;
    public Start critPower;


    [Header("Defensive States")]
    public Start maxHealth;
    public Start armor;
    public Start evasion;
    public Start magicResistance;

    [Header("Magic States")]
    public Start fireDamage;
    public Start iceDamage;
    public Start lightingDamage;

    public bool isIgnited;
    public bool isChilled;
    public bool isShocked;

    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    private float ignitedDamageCooldown = .3f;
    private float ignitedDamageTimer;
    private int ignitedDamage;
    
    public int currentHealth;

    public System.Action OnHealthChanged;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHealth = GetMaxHealthValue();
        critPower.SetDefultValue(150);
        //damage.AddModifiers(4);
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        ignitedDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
            isIgnited = false;
        if(chilledTimer < 0)
            isChilled = false;
        if(shockedTimer < 0)
            isShocked = false;

        if(ignitedDamageTimer < 0 && isIgnited)
        {
            Debug.Log("Take burning Damage");
            DecreaseHealthBy(ignitedDamage);
            if (currentHealth < 0)
                Die();
            ignitedDamageTimer = ignitedDamageCooldown;
        }
    }

    public virtual void DoDamage(CharacterStarts _target)
    {
        if (TargetCanAvoidAttack(_target))
            return;
        
        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
            totalDamage = ClaculateCriticalDamage(totalDamage);

        totalDamage = CheckTargetArmor(_target, totalDamage);

        DoMagicDamage(_target);
        //_target.TakeDamage(totalDamage);
        //Damage();
    }

    public virtual void DoMagicDamage(CharacterStarts _target)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        int totalMagicDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();
        totalMagicDamage = CheckTargetResistance(_target, totalMagicDamage);
        _target.TakeDamage(totalMagicDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
            return;

        bool canApplyIgnited = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChilled = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
        bool canApplyShocked = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;

        while(!canApplyChilled && !canApplyIgnited && !canApplyShocked)
        {
            if(UnityEngine.Random.value < 0.33f && _fireDamage > 0)
            {
                canApplyIgnited = true;
                _target.ApplyAilments(canApplyIgnited, canApplyChilled, canApplyShocked);
                return;
            }
            if (UnityEngine.Random.value < 0.33f && _iceDamage > 0)
            {
                canApplyChilled = true;
                _target.ApplyAilments(canApplyIgnited, canApplyChilled, canApplyShocked);
                return;
            }
            if (UnityEngine.Random.value < 0.33f && _lightingDamage > 0)
            {
                canApplyShocked = true;
                _target.ApplyAilments(canApplyIgnited, canApplyChilled, canApplyShocked);
                return;
            }
        }
        if (canApplyIgnited)
            SetupIngitedDamage(Mathf.RoundToInt(_fireDamage * .2f));

        _target.ApplyAilments(canApplyIgnited, canApplyChilled, canApplyShocked);

    }

    public void SetupIngitedDamage(int _damage) => ignitedDamage = _damage;

    private static int CheckTargetResistance(CharacterStarts _target, int totalMagicDamage)
    {
        totalMagicDamage -= _target.magicResistance.GetValue() + (_target.intelligence.GetValue() * 3);
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        return totalMagicDamage;
    }

    public void ApplyAilments(bool _isIgnited, bool _isChilled, bool _isShocked)
    {
        if(isIgnited || isChilled || isShocked) return;

        if(_isIgnited)
        {
            isIgnited = _isIgnited;
            ignitedTimer = 4f;
        }
        if(_isChilled)
        {
            isChilled = _isChilled;
            chilledTimer = 3f;
        }
        if(_isShocked)
        {
            isShocked = _isShocked;
            shockedTimer = 2f;
        }


    }

    private bool CanCrit()
    {
        int totalCritChance = critChance.GetValue() + agility.GetValue();
        if (UnityEngine.Random.Range(0, 100) < totalCritChance)
        {
            return true;
        }
        return false;
    }

    private int CheckTargetArmor(CharacterStarts _target, int totalDamage)
    {
        if (_target.isChilled)
            totalDamage -= Mathf.RoundToInt(_target.armor.GetValue() * .8f);
        else
            totalDamage -= _target.armor.GetValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealthBy(_damage);

        if (currentHealth < 0)
            Die();
    }

    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHealth -= _damage;
        if(OnHealthChanged != null)
            OnHealthChanged();
    }

    private int ClaculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;
        float CritDamage = totalCritPower * _damage;

        return Mathf.RoundToInt(CritDamage);
    }

    protected virtual void Die()
    {
        
    }
    private bool TargetCanAvoidAttack(CharacterStarts _target)
    {
        int totalEvasion = _target.evasion.GetValue() + _target.agility.GetValue();
        if(isShocked)
            totalEvasion += 20;

        if (UnityEngine.Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }
        return false;
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue();
    }

}
