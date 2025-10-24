using UnityEngine;
using System.Collections;

public class RandomSoundPlayer : MonoBehaviour
{
    [Header("Clips a reproducir aleatoriamente")]
    public AudioClip[] soundClips;

    [Header("Rango de tiempo entre sonidos (segundos)")]
    public float minDelay = 5f;
    public float maxDelay = 15f;

    [Header("Volumen")]
    public float volume = 0.5f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 0f; // Sonido 2D
        StartCoroutine(PlaySoundsRandomly());
    }

    IEnumerator PlaySoundsRandomly()
    {
        while (true)
        {
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);

            if (soundClips.Length > 0)
            {
                AudioClip randomClip = soundClips[Random.Range(0, soundClips.Length)];
                audioSource.PlayOneShot(randomClip, volume);
            }
        }
    }
}
