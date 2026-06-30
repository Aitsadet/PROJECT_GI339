using System.Collections;
using UnityEngine;

public class ZoneAudioManager : MonoBehaviour
{
    public AudioSource zoneAudioSource;
    public float fadeSpeed = 2f;
    public float targetVolume = 0.5f;

    private Coroutine fadeCoroutine;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            if (!zoneAudioSource.isPlaying) zoneAudioSource.Play();
            fadeCoroutine = StartCoroutine(FadeAudio(targetVolume));
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeAudio(0f));
        }
    }

    private IEnumerator FadeAudio(float endVolume)
    {
        while (!Mathf.Approximately(zoneAudioSource.volume, endVolume))
        {
            zoneAudioSource.volume = Mathf.MoveTowards(zoneAudioSource.volume, endVolume, fadeSpeed * Time.deltaTime);
            yield return null;
        }

        if (endVolume == 0f)
        {
            zoneAudioSource.Stop();
        }
    }
}