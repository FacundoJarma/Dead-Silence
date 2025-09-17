using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundEntry
{
    public string name;        // Ej: "Jump", "Pickup", "Run"
    public AudioClip clip;     // El clip asociado
}

public class PlayerSFX : MonoBehaviour
{
    public AudioSource audioSource;   // El AudioSource que reproducirá los sonidos
    public List<SoundEntry> sounds;   // Lista editable en el inspector

    private Dictionary<string, AudioClip> soundDictionary;

    void Awake()
    {
        // Convertimos la lista en un diccionario para búsquedas rápidas
        soundDictionary = new Dictionary<string, AudioClip>();
        foreach (var s in sounds)
        {
            if (!soundDictionary.ContainsKey(s.name))
                soundDictionary.Add(s.name, s.clip);
        }
    }


    public void PlaySound(string soundName, float volume = 1f)
    {
        if (soundDictionary.TryGetValue(soundName, out AudioClip clip))
        {
            audioSource.PlayOneShot(clip, volume);
        }
        else
        {
            Debug.LogWarning($"[PlayerSFX] No se encontró el sonido con el nombre '{soundName}'");
        }
    }
}
