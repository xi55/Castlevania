using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] PlayerStarts playerStarts;
    [SerializeField] private Slider slider;

    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image flaskImage;
    [SerializeField] private Image blackholeImage;

    [SerializeField] private TextMeshProUGUI currentSoul;

    private SkillManager skills;
    void Start()
    {
        UpdateHealthUI();
        if (playerStarts != null)
            playerStarts.OnHealthChanged += UpdateHealthUI;

        skills = SkillManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        currentSoul.text = PlayerManager.instance.GetCurrency().ToString();

        if (Input.GetKeyDown(KeyCode.LeftShift) && skills.dashSkill.dashUnlocked)
            SetupCooldownOf(dashImage);
        if (Input.GetKeyDown(KeyCode.C) && skills.parrySkill.parryUnlocked)
            SetupCooldownOf(parryImage);
        if (Input.GetKeyDown(KeyCode.S) && skills.crystalSkill.crystalUnlocked)
            SetupCooldownOf(crystalImage);
        if (Input.GetKeyDown(KeyCode.LeftControl) && skills.swordSkill.swordUnlock)
            SetupCooldownOf(swordImage);
        if (Input.GetKeyDown(KeyCode.F) && skills.blackHoleSkill.blackHoleUnlocked)
            SetupCooldownOf(blackholeImage);
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetupCooldownOf(flaskImage);


        CheckCooldownOf(dashImage, skills.dashSkill.cooldown);
        CheckCooldownOf(parryImage, skills.parrySkill.cooldown);
        CheckCooldownOf(crystalImage, skills.crystalSkill.cooldown);
        CheckCooldownOf(swordImage, skills.swordSkill.cooldown);
        CheckCooldownOf(blackholeImage, skills.blackHoleSkill.cooldown);
        CheckCooldownOf(flaskImage, Inventory.instance.flaskCooldown);
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = playerStarts.GetMaxHealthValue();
        slider.value = playerStarts.currentHealth;
    }

    private void SetupCooldownOf(Image _image)
    {
        if(_image.fillAmount <= 0)
            _image.fillAmount = 1;
    }

    private void CheckCooldownOf(Image _image, float _cooldown)
    {
        if (_image.fillAmount > 0)
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
    }
}
