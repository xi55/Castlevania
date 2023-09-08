using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
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
    lightingDamage
}
public class CharacterStarts : MonoBehaviour
{
    private CharacterFX fx;

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

    [SerializeField] private float ailmentDuration = 4f;

    public bool isIgnited;
    public bool isChilled;
    public bool isShocked;

    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    private float ignitedDamageCooldown = .3f;
    private float ignitedDamageTimer;
    [SerializeField] private int ignitedDamage;

    [SerializeField] private GameObject shockPrefab;
    private int shockDamage;

    public int currentHealth;
    public bool isDead { get; private set; }
    private bool isVulnerable;
    public bool isInvincible { get; private set; }

    public System.Action OnHealthChanged;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        fx = GetComponent<CharacterFX>();
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
        if (chilledTimer < 0)
            isChilled = false;
        if (shockedTimer < 0)
            isShocked = false;
        if(isIgnited)
        {
            ApplyIgniteDamage(ignitedDamage);
        }
    }

    public virtual void IncreasesStat(int _modifer, float _duration, Start _start)
    {
        StartCoroutine(StatModCoroutine(_modifer, _duration, _start));
    }
    IEnumerator StatModCoroutine(int _modifer, float _duration, Start _start)
    {
        _start.AddModifiers(_modifer);
        yield return new WaitForSeconds(_duration);
        _start.RemoveModifiers(_modifer);
    }

    public void MakeVulnerableFor(float _duration) => StartCoroutine(VulnerableForCorutine(_duration));

    private IEnumerator VulnerableForCorutine(float _duration)
    {
        isVulnerable = true;
        yield return new WaitForSeconds(_duration);
        isVulnerable = false;
    }

    public virtual void IncreaseHealth(int healAmount)
    {
        currentHealth += healAmount;
        if(currentHealth > maxHealth.GetValue())
            currentHealth = maxHealth.GetValue();

            if(OnHealthChanged != null)
                 OnHealthChanged();
    }

    private void ApplyIgniteDamage(int ignitedDmg)
    {
        
        if (ignitedDamageTimer < 0)
        {
            DecreaseHealthBy(ignitedDmg);
            if (currentHealth < 0 && !isDead)
                Die();
            ignitedDamageTimer = ignitedDamageCooldown;
        }
    }

    public virtual void DoDamage(CharacterStarts _target)
    {
        bool isCritical = false;
        //Debug.Log(_target.name);
        if (TargetCanAvoidAttack(_target))
            return;
        
        _target.GetComponent<Character>().SetupKonckBackDir(transform);

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
        {
            totalDamage = ClaculateCriticalDamage(totalDamage);
            isCritical = true;
        }

        fx.CreateHitFx(_target.transform, isCritical);

        totalDamage = CheckTargetArmor(_target, totalDamage);

        _target.TakeDamage(totalDamage);
    }

    public virtual void DoMagicDamage(CharacterStarts _target)
    {

        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();
        Debug.Log("fireDamage: " + _fireDamage + ", iceDamage: " + _iceDamage + ", lightingDamage:" + _lightingDamage);
        int totalMagicDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();
        totalMagicDamage = CheckTargetResistance(_target, totalMagicDamage);
        _target.TakeDamage(totalMagicDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
            return;

        AttemptyToApplyAilments(_target, _fireDamage, _iceDamage, _lightingDamage);

    }

    private void AttemptyToApplyAilments(CharacterStarts _target, int _fireDamage, int _iceDamage, int _lightingDamage)
    {
        bool canApplyIgnited = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChilled = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
        bool canApplyShocked = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;

        while (!canApplyChilled && !canApplyIgnited && !canApplyShocked)
        {
            if (UnityEngine.Random.value < 0.33f && _fireDamage > 0)
            {
                canApplyIgnited = true;
                _target.ApplyAilments(canApplyIgnited, canApplyChilled, canApplyShocked, _fireDamage);
                return;
            }
            if (UnityEngine.Random.value < 0.33f && _iceDamage > 0)
            {
                canApplyChilled = true;
                _target.ApplyAilments(canApplyIgnited, canApplyChilled, canApplyShocked, _iceDamage);
                return;
            }
            if (UnityEngine.Random.value < 0.33f && _lightingDamage > 0)
            {
                canApplyShocked = true;
                _target.ApplyAilments(canApplyIgnited, canApplyChilled, canApplyShocked, _lightingDamage);
                return;
            }
        }

        if (canApplyIgnited)
        {
            _target.SetupIngitedDamage(Mathf.RoundToInt(_fireDamage * .2f));
        }

        if (canApplyShocked)
        {
            _target.SetipShockedDamage(Mathf.RoundToInt(_lightingDamage * .2f));
        }
        print("1: " + canApplyIgnited + ", " + canApplyChilled + ", " + canApplyShocked);
        _target.ApplyAilments(canApplyIgnited, canApplyChilled, canApplyShocked, shockDamage);
    }

    public void SetupIngitedDamage(int _damage) => ignitedDamage = _damage;
    public void SetipShockedDamage(int _damage) => shockDamage = _damage;
    private int CheckTargetResistance(CharacterStarts _target, int totalMagicDamage)
    {
        totalMagicDamage -= _target.magicResistance.GetValue() + (_target.intelligence.GetValue() * 3);
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        return totalMagicDamage;
    }

    public void ApplyAilments(bool _isIgnited, bool _isChilled, bool _isShocked, int enemyhockDamage)
    {
        bool canApplyIgnite = !isChilled && !isIgnited && !isShocked;
        bool canApplyChill = !isChilled && !isIgnited && !isShocked;
        bool canApplyShock = !isChilled && !isIgnited;
        print("2: " + canApplyIgnite + ", " + canApplyChill + ", " + canApplyShock);
        if (_isIgnited && canApplyIgnite)
        {
            isIgnited = _isIgnited;
            ignitedTimer = ailmentDuration;
            fx.IgniteFXfor(ailmentDuration);
        }
        if(_isChilled && canApplyChill)
        {
            isChilled = _isChilled;
            chilledTimer = ailmentDuration;
            fx.chillFXfor(ailmentDuration);
            GetComponent<Character>().SlowCharacter(.2f, ailmentDuration);
        }
        if(_isShocked && canApplyShock)
        {
            if(!isShocked)
            {
                ApplyShock(_isShocked);
            }
            else
            {
                if (GetComponent<Player>() != null) return;
                
                HitNearestTargetWithShockStrick(enemyhockDamage);
            }
        }


    }

    public void ApplyShock(bool _isShocked)
    {
        isShocked = _isShocked;
        shockedTimer = ailmentDuration;
        fx.ShockFXfor(ailmentDuration);
    }

    private void HitNearestTargetWithShockStrick(int enemyhockDamage)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 20);
        float minClosesTarget = Mathf.Infinity;
        Transform closeTarget = null;
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1f)
            {
                if (Vector2.Distance(transform.position, hit.transform.position) < minClosesTarget)
                {
                    minClosesTarget = Vector2.Distance(transform.position, hit.transform.position);
                    closeTarget = hit.transform;
                }
            }
            if (closeTarget == null)
                closeTarget = transform;
        }


        if (closeTarget != null)
        {
            GameObject newShockStrick = Instantiate(shockPrefab, transform.position, Quaternion.identity);
            newShockStrick.GetComponent<Thunder_Controller>().SetupThunder(enemyhockDamage, closeTarget.GetComponent<CharacterStarts>());
        }
    }

    protected bool CanCrit()
    {
        int totalCritChance = critChance.GetValue() + agility.GetValue();
        if (UnityEngine.Random.Range(0, 100) < totalCritChance)
        {
            return true;
        }
        return false;
    }

    protected int CheckTargetArmor(CharacterStarts _target, int totalDamage)
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
        if (isInvincible) return;

        DecreaseHealthBy(_damage);
        
        GetComponent<Character>().DamageEffect();
        fx.StartCoroutine(fx.FlashFX());
        if (currentHealth < 0 && !isDead)
            Die();
    }

    protected virtual void DecreaseHealthBy(int _damage)
    {
        if (isVulnerable)
            _damage = Mathf.RoundToInt(_damage * 1.5f);
        Debug.Log(gameObject.name + " was dameged. value is " + _damage.ToString());
        currentHealth -= _damage;
        if(OnHealthChanged != null)
            OnHealthChanged();
    }

    protected int ClaculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;
        float CritDamage = totalCritPower * _damage;

        return Mathf.RoundToInt(CritDamage);
    }

    protected virtual void Die()
    {
        isDead = true;
    }

    public void KillCharacter()
    {
        if (!isDead)
            Die();
    }

    public void MakeInvincible(bool _invincible) => isInvincible = _invincible;

    public virtual void OnEvasion()
    {
        Debug.Log(transform.name + ": Avoid Attack");
    }

    protected bool TargetCanAvoidAttack(CharacterStarts _target)
    {
        int totalEvasion = _target.evasion.GetValue() + _target.agility.GetValue();
        if(isShocked)
            totalEvasion += 20;

        if (UnityEngine.Random.Range(0, 100) < totalEvasion)
        {
            _target.OnEvasion();
            return true;
        }
        return false;
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue();
    }

    public Start GetStat(StatType statType)
    {
        switch (statType)
        {
            case StatType.intelligence: return intelligence;
            case StatType.vitality: return vitality;
            case StatType.damage: return damage;
            case StatType.critChance: return critChance;
            case StatType.critPower: return critPower;
            case StatType.strength: return strength;
            case StatType.agility: return agility;
            case StatType.health: return maxHealth;
            case StatType.armor: return armor;
            case StatType.evasion: return evasion;
            case StatType.magicResistance: return magicResistance;
            case StatType.fireDamage: return fireDamage;
            case StatType.iceDamage: return iceDamage;
            case StatType.lightingDamage: return lightingDamage;
        }

        return null;
    }
}


