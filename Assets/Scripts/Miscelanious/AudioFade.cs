using System.Collections;
using UnityEngine;

public class AudioFade : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] float fadeDuration = 1f;
    [SerializeField] float volume = 0f;

    void Start()
    {
        volume = audioSource.volume;
    }
    public void FadeOut()
    {
        StartCoroutine("FadeOutCoroutine");
    }

    IEnumerator FadeOutCoroutine()
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = volume;
    }


}
