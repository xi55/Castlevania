using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotkeyPrefab;
    [SerializeField] private List<KeyCode> hotkeys = new List<KeyCode>();

    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    private bool canGrow = true;
    private bool canShrink;
    private bool playerCanDisapear = true;

    private bool canAttack;
    private int attackAmount = 4;
    private float cloneAttackCooldown = 0.3f;
    private float cloneAttackTimer;
    [SerializeField] private float blackholeTimer;

    [SerializeField] private List<Transform> target = new List<Transform>();
    private List<GameObject> createHotkey = new List<GameObject>();
    private bool canCreateHotkey = true;

    public bool playerCanExitState { get; private set; }
    
    public void SetBlackHole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _attackAmount, float _cloneAttackCooldown, float _blackholeDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        attackAmount = _attackAmount;
        cloneAttackCooldown = _cloneAttackCooldown;
        blackholeTimer = _blackholeDuration;
        if (SkillManager.instance.cloneSkill.crystalInseadOfClone)
            playerCanDisapear = false;
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        if(blackholeTimer < 0)
        {
            blackholeTimer = Mathf.Infinity;
            if (target.Count > 0)
                ReleaseCloneAttack();
            else
                FnishBlackHole();

        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();
        if (canGrow && !canShrink)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x < 0)
                Destroy(gameObject);
        }
    }

    private void ReleaseCloneAttack()
    {
        if (target.Count <= 0)
        {
            return;
        }
        DestroyHotkey();
        canAttack = true;
        canCreateHotkey = false;
        if (playerCanDisapear) 
        {
            playerCanDisapear = false;
            PlayerManager.instance.player.fx.MakeTransprent(true);
        }
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && canAttack && attackAmount > 0)
        {
            cloneAttackTimer = cloneAttackCooldown;

            int randomIndex = Random.Range(0, target.Count);
            float xOffset;

            if (Random.Range(0, 100) > 50)
                xOffset = 2;
            else
                xOffset = -2;

            if(SkillManager.instance.cloneSkill.crystalInseadOfClone)
            {
                SkillManager.instance.crystalSkill.CreateCrystal();
                SkillManager.instance.crystalSkill.ChooseRandomTarget();
            }
            else
                SkillManager.instance.cloneSkill.CreateClone(target[randomIndex], new Vector3(xOffset, 0));

            attackAmount--;
            if (attackAmount <= 0)
            {
                Invoke("FnishBlackHole", 1f);
            }
        }
    }

    private void FnishBlackHole()
    {
        DestroyHotkey();
        playerCanExitState = true;
        //PlayerManager.instance.player.ExitBlackHoleState();
        canShrink = true;
        canAttack = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            GetHotKey(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(false);
        }
    }

    private void DestroyHotkey()
    {
        if(createHotkey.Count <= 0) return;
        for(int i = 0; i < createHotkey.Count; i++)
            Destroy(createHotkey[i]);
    }

    private void GetHotKey(Collider2D collision)
    {
        if (!canCreateHotkey) return;
        GameObject newHotKey = Instantiate(hotkeyPrefab, collision.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        createHotkey.Add(newHotKey);
        KeyCode chooseKey = hotkeys[Random.Range(0, hotkeys.Count)];
        hotkeys.Remove(chooseKey);
        newHotKey.GetComponent<BlackHole_Hotkey_Controller>().SetHotKey(chooseKey, collision.transform, this);
    }

    public void AddEnemtToList(Transform _enemy) => target.Add(_enemy);
}
