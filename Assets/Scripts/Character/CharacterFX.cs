using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class CharacterFX : MonoBehaviour
{
    protected SpriteRenderer sr;
    protected Player player;

    [Header("Pop Up Text")]
    [SerializeField] private GameObject popUpTextPrefab;

    [Header("Flashing info")]
    [SerializeField] private Material FX;
    [SerializeField] private float FXtime;
    private Material originFX;

    [Header("Ailment Colors")]
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] ignteColor;
    [SerializeField] private Color[] shockColor;

    [Header("Ailment particles")]
    [SerializeField] private ParticleSystem igniteFx;
    [SerializeField] private ParticleSystem chillFx;
    [SerializeField] private ParticleSystem shockFx;

    [Header("Hit FX")]
    [SerializeField] private GameObject hitFxPrefab;
    [SerializeField] private GameObject hitCritFxPrefab;

    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originFX = sr.material;
        player = PlayerManager.instance.player;
    }


    public void CreatePopUpText(string _text)
    {
        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(2, 3);
        Vector3 posOffset = new Vector3(randomX, randomY, 0);

        GameObject newPopUpText = Instantiate(popUpTextPrefab, transform.position + posOffset, Quaternion.identity);

        newPopUpText.GetComponent<TextMeshPro>().text = _text;
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
        igniteFx.Stop();
        chillFx.Stop();
        shockFx.Stop();
    }


    public void chillFXfor(float _second)
    {
        chillFx.Play();
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
        igniteFx.Play();
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
        shockFx.Play();
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

    public void CreateHitFx(Transform _target, bool _critical)
    {
        float zRotation = Random.Range(-90, 90);
        float xPosition = Random.Range(-.5f, .5f);
        float yPosition = Random.Range(-.5f, .5f);

        Vector3 hitFxRotation = new Vector3(0, 0, zRotation);

        GameObject hitPrefab = hitFxPrefab;
        if (_critical)
        {
            hitPrefab = hitCritFxPrefab;
            float yRotation = 0;
            zRotation = Random.Range(-45, 45);
            if (GetComponent<Character>().faceDir == -1)
                yRotation = 180;
            hitFxRotation = new Vector3(0, yRotation, zRotation);
        }

        GameObject newHitFX = Instantiate(hitPrefab, _target.position, Quaternion.identity);
        newHitFX.transform.Rotate(hitFxRotation);

        Destroy(newHitFX, .5f);
    }



}
