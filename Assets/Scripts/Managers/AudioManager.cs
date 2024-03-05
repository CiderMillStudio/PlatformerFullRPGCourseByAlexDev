using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private float sfxMaximumDistance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] backgroundMusic;

    public bool playBackgroundMusic;

    private int backgroundMusicIndex;

    private bool canPlaySFX = false;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
            instance = this;

        Invoke("EnableSFX", 0.2f);
    }

    private void Update()
    {
        if (!playBackgroundMusic)
            StopAllBackgroundMusic();
        else
        {
            if (!backgroundMusic[backgroundMusicIndex].isPlaying)
                PlayBackgroundMusic(backgroundMusicIndex);
        }
    }



    public void PlaySFX(int _sfxIndex, Transform _source) //pass null for the _source if you DON'T want a distance check
    {
        //if (sfx[_sfxIndex].isPlaying)
            //return;

        if (!canPlaySFX)
        {
            return;
        }

        if (_source != null)
        {
            if (Vector2.Distance(_source.position, PlayerManager.instance.player.transform.position) > sfxMaximumDistance)
            {
                return;
            }
        }
        
        sfx[_sfxIndex].pitch = Random.Range(0.85f, 1.15f);
        
        if (_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].Play();
           
        }
    }

    public void StopSFX(int _index) => sfx[_index].Stop();

    public void PlayBackgroundMusic(int _BgmIndex)
    {
        backgroundMusicIndex = _BgmIndex;

        StopAllBackgroundMusic();

        backgroundMusic[_BgmIndex].Play();
    }

    public void StopAllBackgroundMusic()
    {
        for (int i = 0; i < backgroundMusic.Length; i++)
        {
            backgroundMusic[i].Stop();
        }

        //playBackgroundMusic = false; //get rid of this!!
    }

    public void EnableSFX()
    {
        canPlaySFX = true;
        PlaySFX(114, null); //this is the ambient noise loop
    }

    public void FadeOutSfxVolume(int _sfxIndex, float _rate)
    {
        //StopAllCoroutines();
        StartCoroutine(FadeOutVolumeCoroutine(sfx[_sfxIndex], _rate));
    }

    public void FadeInSfxVolume(int _sfxIndex, float _rate)
    {
        StopAllCoroutines();
        PlaySFX(_sfxIndex, null);
        StartCoroutine(FadeInVolumeCoroutine(sfx[_sfxIndex], _rate));
    }

    public void FadeOutBgmVolume(int _bgmIndex, float _rate)
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutVolumeCoroutine(backgroundMusic[_bgmIndex], _rate));
    }

    public void FadeInBgmVolume(int _bgmIndex, float _rate)
    {
        StopAllCoroutines();
        PlayBackgroundMusic(_bgmIndex);
        StartCoroutine(FadeInVolumeCoroutine(backgroundMusic[_bgmIndex], _rate));
    }

    private IEnumerator FadeOutVolumeCoroutine(AudioSource _audio, float _rate)
    { 
        float defaultVolume = _audio.volume;

        while (_audio.volume > 0.1f)
        {
            _audio.volume -= 0.05f;

            float newRate = 1 / _rate;

            yield return new WaitForSeconds(newRate);

            if (_audio.volume < 0.15f)
                break;
            

        }

        if (_audio.volume <= 0.16f)
        {
            _audio.Stop();
            foreach (AudioSource source in backgroundMusic)
            {
                if (_audio == source)
                {
                    playBackgroundMusic = false;
                }
            }
            _audio.volume = defaultVolume;
                
        }
    }

    private IEnumerator FadeInVolumeCoroutine(AudioSource _audio, float _rate)
    {
        float defaultVolume = 0.5f;
        _audio.volume = 0;

        while (_audio.volume <= defaultVolume)
        {
            _audio.volume += 0.05f;
            float newRate = 1 / _rate;
            yield return new WaitForSeconds(newRate);
            
            if (_audio.volume >= 0.5f)
            {
                break;
            }

        }
    }




}
