using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundPlayer : MonoBehaviour
{
    [NonSerialized] public static SoundPlayer Instance;

    [SerializeField] private AudioClip bgmForScene;
    [SerializeField] private AudioMixer audioMixer;

    public static string SFX = "SFX";
    public static string BGM = "BGM";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        PlayBGM(bgmForScene, true);
    }

    public void SetLevel(float sliderValue)
    {
        audioMixer.SetFloat("Master_Volume", Mathf.Log10(sliderValue) * 20);
    }

    public void SetBGMLevel(float sliderValue)
    {
        audioMixer.SetFloat("BGM_Volume", Mathf.Log10(sliderValue) * 20);
    }

    public AudioSource PlayBGM(
        AudioClip clip,
        bool loop = false,
        float spatialBlend = 0,
        float volume = 1
    )
    {
        if (clip == null) return null;
        return PlayOnGO(clip, Camera.main.gameObject, BGM, loop, spatialBlend, volume);
    }

    public AudioSource PlayOnGO(
        AudioClip clip,
        GameObject GO,
        string audioMixerGroup,
        bool loop = false,
        float spatialBlend = 0,
        float volume = 1
    )
    {
        if (clip == null || GO == null) return null;

        AudioSource source = GO.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = loop;
        source.spatialBlend = spatialBlend;
        source.volume = volume;

        source.outputAudioMixerGroup = audioMixer.FindMatchingGroups(audioMixerGroup)[0];

        source.Play();
        if (!loop)
        {
            StartCoroutine(DestroySource(clip.length, source));
        }

        return source;
    }

    public AudioSource PlayRandomSoundOnGO(
        List<AudioClip> clips,
        GameObject GO,
        string audioMixerGroup,
        bool loop = false,
        float spatialBlend = 0,
        float volume = 1
    ) {
        if (clips.Count == 0) return null;

        int soundToPlay = UnityEngine.Random.Range(0, clips.Count);
        AudioClip toolSoundToPlay = clips[soundToPlay];

        return PlayOnGO(toolSoundToPlay, GO, audioMixerGroup, loop, spatialBlend, volume);
    }

    IEnumerator DestroySource(float time, AudioSource source)
    {
        yield return new WaitForSeconds(time);
        Destroy(source);
    }
}