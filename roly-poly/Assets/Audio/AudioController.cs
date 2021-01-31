using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
public abstract class AudioController : MonoBehaviour
{
    protected AudioSource audioSource;

    private AudioMixer audioMixer;
    private AudioMixerGroup audioMixerGroup;
    private string audioMixerFileName = "SFXMixer";

    [Serializable]
    public class SFX
    {
        [SerializeField]
        private AudioClip audioClip = null;
        [SerializeField]
        [Range(0f, 1f)]
        private float volume = 1f;

        [SerializeField]
        [Range(0f, 1f)]
        private float volumeVariationPercent = 0.05f;

        [SerializeField]
        [Range(0f, 2f)]
        private float pitch = 1f;

        [SerializeField]
        [Range(0f, 0.2f)]
        private float pitchVariation = 0.05f;

        public AudioClip GetAudioClip()
        {
            return audioClip;
        }
        public float GetVolume()
        {
            return volume + UnityEngine.Random.Range(-volume * volumeVariationPercent, volume * volumeVariationPercent);
        }
        public float GetPitch()
        {
            return pitch + UnityEngine.Random.Range(-pitchVariation, pitchVariation);
        }
    }

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        LoadAudioMixer();

        // If the gameobject has an AudioSource (example the player)
        if (audioSource != null)
        {
            audioSource.outputAudioMixerGroup = audioMixerGroup;
        }
    }

    protected void PlaySFX(SFX sfx)
    {
        audioSource.pitch = sfx.GetPitch();
        audioSource.PlayOneShot(sfx.GetAudioClip(), sfx.GetVolume());
    }

    //To play clips when something dies (Won't stop playing if the GameObject is destroyed)
    protected AudioSource PlayLingeringSFX(SFX sfx)
    {
        AudioSource tempSource = new GameObject().AddComponent<AudioSource>() as AudioSource;
        tempSource.outputAudioMixerGroup = audioMixerGroup;
        tempSource.gameObject.name = String.Format("SFX {0}", sfx.GetAudioClip().name);
        tempSource.pitch = sfx.GetPitch();
        tempSource.PlayOneShot(sfx.GetAudioClip(), sfx.GetVolume());

        Destroy(tempSource.gameObject, sfx.GetAudioClip().length);
        return tempSource;
        // tempSource.GetComponent<AudioSource>();
        // AudioSource.PlayClipAtPoint(sfx.GetAudioClip(), transform.position, sfx.GetVolume());
    }

    public void FadeOutSFX(AudioSource audioSource, float fadeTime)
    {
        StartCoroutine(FadeOutSFXRoutine(audioSource, fadeTime));
    }
    public void FadeOutSFXAfterDuration(AudioSource audioSource, float fadeTime, float waitTime)
    {
        StartCoroutine(FadeOutSFXAfterDurationRoutine(audioSource, fadeTime, waitTime));
    }
    protected IEnumerator FadeOutSFXAfterDurationRoutine(AudioSource audioSource, float fadeTime, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        FadeOutSFX(audioSource, fadeTime);
    }
    protected IEnumerator FadeOutSFXRoutine(AudioSource audioSource, float fadeTime)
    {
        if (audioSource != null)
        {
            float startVolume = audioSource.volume;

            while (audioSource != null && audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
                yield return null;
            }
            if (audioSource != null)
                Destroy(audioSource.gameObject);
        }

    }

    #region AudioMixer stuff
    private void LoadAudioMixer()
    {
        audioMixer = Resources.Load<AudioMixer>(audioMixerFileName);
        if (audioMixer == null)
        {
            Debug.LogError("Failed to load audioMixer for AudioController. Check the Resources path...");
        }
        audioMixerGroup = audioMixer.FindMatchingGroups("Master")[0];
    }

    #endregion
}
