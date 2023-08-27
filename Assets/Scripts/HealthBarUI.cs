using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    private Character character;
    private CharacterStarts starts;
    private Transform trans;
    private Slider slider;
    private void Start()
    {
        trans = GetComponent<RectTransform>();
        character = GetComponentInParent<Character>();
        slider = GetComponentInChildren<Slider>();
        starts = GetComponentInParent<CharacterStarts>();

        UpdateHealthUI();

        character.onFlipped += FlipUI;
        starts.OnHealthChanged += UpdateHealthUI;
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = starts.GetMaxHealthValue();
        slider.value = starts.currentHealth;
    }

    private void FlipUI() => trans.Rotate(0, 180, 0);

    private void OnDisable()
    {
        character.onFlipped -= FlipUI;
        starts.OnHealthChanged -= UpdateHealthUI;
    }
}
