using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class Sword_Skill : Skill
{
    public SwordType swordType = SwordType.Regular;

    [Header("Bounce info")]
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;
    [SerializeField] private UI_SkillTreeSlot bounceUnlockButton;

    [Header("Pierce info")]
    [SerializeField] private int pierceAmount;
    [SerializeField] private int pierceSpeed;
    [SerializeField] private float pierceGravity;
    [SerializeField] private UI_SkillTreeSlot pierceUnlockButton;

    [Header("Skill info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchDir;
    [SerializeField] private float swordGravity;
    [SerializeField] private UI_SkillTreeSlot swordUnlockButton;
    public bool swordUnlock { get; private set; }

    [Header("Throw info")]
    [SerializeField] private GameObject dotsPrefab;
    [SerializeField] private Transform dotsParent;
    [SerializeField] private int numOfDots;
    [SerializeField] private float spaceBewtenDots;

    [Header("Spin info")]
    [SerializeField] private float maxTravelDistance = 7f;
    [SerializeField] private float spinDuration = 2f;
    [SerializeField] private float spinGravity = 1f;
    [SerializeField] private float hitCooldown;
    [SerializeField] private UI_SkillTreeSlot spinUnlockButton;

    [Header("Freeze Time")]
    [SerializeField] private float freezeTime;
    

    [SerializeField] private Vector2 finalDir;
    [SerializeField] private float yDir = 0;

    [Header("Passive skills")]
    [SerializeField] private UI_SkillTreeSlot timeStopUnlockButton;
    [SerializeField] private UI_SkillTreeSlot vulnerabilityUnlockButton;
    public bool timeStopUnlock { get; private set; }
    public bool vulnerabilityUnlock { get; private set; }
    private GameObject[] dots;

    

    protected override void Start()
    {
        base.Start();
        GenereatDots();
        
        
        
        swordUnlockButton.GetComponent<Button>().onClick.AddListener(SwordUnlocked);
        bounceUnlockButton.GetComponent<Button>().onClick.AddListener(BounceUnlocked);
        pierceUnlockButton.GetComponent<Button>().onClick.AddListener(PierceUnlocked);
        spinUnlockButton.GetComponent<Button>().onClick.AddListener(SpinUnlocked);
        timeStopUnlockButton.GetComponent<Button>().onClick.AddListener(TimeStopUnlocked);
        vulnerabilityUnlockButton.GetComponent<Button>().onClick.AddListener(VulnerabilityUnlocked);
    }

    protected override void CheckUnlock()
    {
        SwordUnlocked();
        BounceUnlocked();
        PierceUnlocked();
        SpinUnlocked();
        TimeStopUnlocked();
        VulnerabilityUnlocked();
    }
    private void SetupGravity()
    {
        if (swordType == SwordType.Bounce)
        {
            swordGravity = bounceGravity;
            launchDir = new Vector2(30, 35);
        }
        else if(swordType== SwordType.Pierce)
        {
            swordGravity = pierceGravity;
            launchDir = new Vector2(pierceSpeed, 0);
        }
        else if(swordType== SwordType.Spin)
        {
            swordGravity = spinGravity;
            launchDir = new Vector2(30, 35);
        }
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKey(KeyCode.LeftControl))
        {
            SetupGravity();
            yDir += Time.deltaTime * 4f;
            for(int i = 0; i < numOfDots; i++)
            {
                dots[i].transform.position = DotsPosition(yDir, i * spaceBewtenDots);
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            
            finalDir = new Vector2(AimDirection(yDir).normalized.x * launchDir.x, AimDirection(yDir).normalized.y * launchDir.y);
            yDir = 0;
        }
    }

    #region Unlock region

    public void TimeStopUnlocked()
    {
        if (timeStopUnlockButton.unLocked)
            timeStopUnlock = true;
    }
    public void VulnerabilityUnlocked()
    {
        if(vulnerabilityUnlockButton.unLocked)
            vulnerabilityUnlock = true;
    }

    public void SwordUnlocked()
    {
        if(swordUnlockButton.unLocked)
        {
            swordType = SwordType.Regular;
            swordUnlock = true;
        }
    }

    private void BounceUnlocked()
    {
        if(bounceUnlockButton.unLocked)
            swordType = SwordType.Bounce;
    }

    public void PierceUnlocked()
    {
        if (pierceUnlockButton.unLocked)
            swordType = SwordType.Pierce;
    }
    public void SpinUnlocked()
    {
        if (spinUnlockButton.unLocked)
            swordType = SwordType.Spin;
    }

    #endregion

    public void CreateSword(Player _player)
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, player.transform.rotation);
        Sword_Controller sword_Controller = newSword.GetComponent<Sword_Controller>();
        
        if(swordType == SwordType.Bounce)
            sword_Controller.SetupBounce(true, bounceAmount);
        else if(swordType == SwordType.Pierce)
            sword_Controller.SetupPierce(true, pierceAmount);
        else if(swordType == SwordType.Spin)
            sword_Controller.SetupSpin(true, maxTravelDistance, spinDuration, hitCooldown);


        sword_Controller.SetupSword(finalDir, swordGravity, _player, freezeTime);
        player.AssignSword(newSword);
        DotsActive(false);
    }

    public Vector2 AimDirection(float yDirection)
    {
        Vector2 playerPos = player.transform.position;
        Vector2 goalPos = new Vector2(playerPos.x + 5 * player.faceDir, playerPos.y + yDirection);
        Vector2 direction = goalPos - playerPos;
        return direction;
    }

    public void DotsActive(bool _active)
    {
        for(int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_active);
        }
    }

    private void GenereatDots()
    {
        dots = new GameObject[numOfDots];
        for(int i = 0; i < numOfDots; i++)
        {
            dots[i] = Instantiate(dotsPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float yDir, float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection(yDir).normalized.x * launchDir.x,
            AimDirection(yDir).normalized.y * launchDir.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t);
        return position;
    }
}
