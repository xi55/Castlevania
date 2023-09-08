using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private float colorLooseRate;
    
    public void SetupAfterImage(Sprite _sprite, float _looseRate)
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = _sprite;
        colorLooseRate = _looseRate;
    }

    void Update()
    {
        if(sr == null) return;
        float alpha = sr.color.a - colorLooseRate * Time.deltaTime;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

        if(sr.color.a <= 0)
            Destroy(gameObject);
    }
}
