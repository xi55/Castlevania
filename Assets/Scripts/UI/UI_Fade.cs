using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Fade : MonoBehaviour
{
    [SerializeField] private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void FadeIm() => animator.SetTrigger("fadeIn");
    public void FadeOut() => animator.SetTrigger("fadeOut");
}
