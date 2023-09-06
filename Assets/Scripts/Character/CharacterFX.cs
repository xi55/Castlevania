using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flashing info")]
    [SerializeField] private Material FX;
    [SerializeField] private float FXtime;
    private Material originFX;

    [Header("Ailment Colors")]
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] ignteColor;
    [SerializeField] private Color[] shockColor;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originFX = sr.material;
    }

    public virtual void MakeTransprent(bool _transprent)
    {
        if (_transprent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }
    public IEnumerator FlashFX()
    {
        sr.material = FX;
        Color currentColor = sr.color;
        sr.color = Color.white;

        yield return new WaitForSeconds(FXtime);

        sr.color = currentColor;
        sr.material = originFX;
    }

    private void RedColorBlink()
    {
        if(sr.color != Color.white)
            sr.color = Color.white;
        else
            sr.color = Color.red;
    }

    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;
    }


    public void chillFXfor(float _second)
    {
        InvokeRepeating("chillColorFX", 0, .3f);
        Invoke("CancelColorChange", _second);
    }

    private void chillColorFX()
    {
        if (sr.color != chillColor[0])
            sr.color = chillColor[0];
        else
            sr.color = chillColor[1];
    }

    public void IgniteFXfor(float _second)
    {
        InvokeRepeating("IgniteColorFX", 0, .3f);
        Invoke("CancelColorChange", _second);
    }

    private void IgniteColorFX()
    {
        if (sr.color != ignteColor[0])
            sr.color = ignteColor[0];
        else
            sr.color = ignteColor[1];
    }

    public void ShockFXfor(float _second)
    {
        InvokeRepeating("shockColorFX", 0, .3f);
        Invoke("CancelColorChange", _second);
    }

    private void shockColorFX()
    {
        if (sr.color != shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }

}
