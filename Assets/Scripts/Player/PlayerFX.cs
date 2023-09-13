using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerFX : CharacterFX
{
    [Header("Screen Shack FX")]
    private CinemachineImpulseSource screenShack;
    [SerializeField] private float shackMultiplier;
    [SerializeField] private Vector3 shackPower;

    [Header("After image FX")]
    [SerializeField] private float afterImgCooldown;
    [SerializeField] private float afterImgTimer;
    [SerializeField] private GameObject afterImagePreafab;
    [SerializeField] private float colorLooseRate;
    [Space]
    

    [Space]
    [SerializeField] private ParticleSystem dustFx;

    protected override void Start()
    {
        base.Start();
        screenShack = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        afterImgTimer -= Time.deltaTime;
    }


    public void CreateAfterImage()
    {
        if (afterImgTimer < 0)
        {
            afterImgTimer = afterImgCooldown;
            GameObject newAfterImg = Instantiate(afterImagePreafab, transform.position, transform.rotation);
            newAfterImg.GetComponent<AfterImageFX>().SetupAfterImage(sr.sprite, colorLooseRate);
        }
    }

    public void ScreenShack()
    {
        screenShack.GenerateImpulse(new Vector3(shackPower.x * player.faceDir, shackPower.y) * shackMultiplier);
    }

    public void playDustFx()
    {
        if (dustFx != null)
            dustFx.Play();
    }
}
