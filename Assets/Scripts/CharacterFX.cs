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

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originFX = sr.material;
    }

    public IEnumerator FlashFX()
    {
        sr.material = FX;
        yield return new WaitForSeconds(FXtime);
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
}
