using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    private int bgmIndex;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keep the instance across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(CheckAndPlayBGM());
    }

    private IEnumerator CheckAndPlayBGM()
    {
        while (true)
        {
            if (!bgm[bgmIndex].isPlaying)
            {
                PlayRandomBGM();
            }
            yield return new WaitForSeconds(1f); // Check every second
        }
    }

    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    public void PlaySFX(int index)
    {
        if (index < sfx.Length)
        {
            sfx[index].pitch = Random.Range(.85f, 1.1f);
            sfx[index].Play();
        }
    }

    public void StopSFX(int index)
    {
        if (index < sfx.Length)
        {
            sfx[index].Stop();
        }
    }

    public void PlayBGM(int index)
    {
        if (index < bgm.Length)
        {
            StopBGM();
            bgm[index].Play();
        }
    }

    public void StopBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }
}
