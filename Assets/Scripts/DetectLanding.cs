using UnityEngine;
using System.Collections.Generic;

public class DetectLanding : MonoBehaviour
{
    [Header("Configuración de atracción")]
    public float radioAtraccion = 20f;
    public int maxZombies = 4;

    [Header("Efectos opcionales")]
    public AudioClip sonidoImpacto;
    private AudioSource audioSource;

    private bool hasLanded = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasLanded) return;

        if (collision.gameObject.CompareTag("Ground"))
        {
            hasLanded = true;

            Vector3 posicionImpacto = transform.position;
            Debug.Log($"El objeto {gameObject.name} cayó en: {posicionImpacto}");

            // 🔊 Reproducir sonido si hay uno asignado
            if (sonidoImpacto != null)
            {
                audioSource.PlayOneShot(sonidoImpacto);
            }

            // 🧟‍♂️ Buscar zombies cercanos
            AtraerZombies(posicionImpacto);
        }
    }

    void AtraerZombies(Vector3 posicion)
    {
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
        List<GameObject> zombiesEnRango = new List<GameObject>();

        foreach (GameObject z in zombies)
        {
            Debug.Log(z.name);
            float distancia = Vector3.Distance(z.transform.position, posicion);
            if (distancia <= radioAtraccion)
            {
                zombiesEnRango.Add(z);
            }
        }

        // Ordenar por cercanía
        zombiesEnRango.Sort((a, b) =>
            Vector3.Distance(a.transform.position, posicion)
            .CompareTo(Vector3.Distance(b.transform.position, posicion))
        );

        // Llamar a los zombies más cercanos
        for (int i = 0; i < Mathf.Min(maxZombies, zombiesEnRango.Count); i++)
        {
            GameObject zombie = zombiesEnRango[i];
            Zombie scriptZombie = zombie.GetComponent<Zombie>();

            if (scriptZombie != null)
            {
                scriptZombie.IrAHaciaSonido(posicion);
            }
        }
    }
}
