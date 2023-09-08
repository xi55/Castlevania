using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private float sfxMinDistance;

    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    public bool playBGM;
    private int bgmIndex;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance.gameObject);
    }

    private void Update()
    {
        if(!playBGM)
            StopAllBgm();
        else
        {
            if(!bgm[bgmIndex].isPlaying)
                PlayBgm(bgmIndex);
        }
    }

    public void PlaySfx(int _index, Transform _source)
    {
        if (sfx[_index].isPlaying) return;

        if (_source != null && Vector2.Distance(PlayerManager.instance.transform.position, _source.position) > sfxMinDistance) return;

        if(_index < sfx.Length)
        {
            sfx[_index].pitch = Random.Range(0.85f, 1.1f);
            sfx[_index].Play();
        }
    }

    public void StopSfx(int _index) => sfx[_index].Stop();

    public void StopSfxWithTime(int _index) => StartCoroutine(DecreaseVolume(sfx[_index]));

    private IEnumerator DecreaseVolume(AudioSource _source)
    {
        float defultVolume = _source.volume;
        while(_source.volume > .1f)
        {
            _source.volume -= _source.volume * .2f;
            yield return new WaitForSeconds(.25f);
            if(_source.volume <= .1f)
            {
                _source.Stop();
                _source.volume = defultVolume;
                break;
            }
        }
    }

    public void PlayBgm(int _index)
    {
        bgmIndex = _index;
        StopAllBgm();
        bgm[bgmIndex].Play();
    }

    public void PlayerRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        bgm[bgmIndex].Play();
    }

    public void StopAllBgm()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }

}
