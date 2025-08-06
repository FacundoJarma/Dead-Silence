using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    private NavMeshAgent agente;
    private bool patrullando = false;

    public GameObject jugador;
    public Transform[] puntosPatrulla;     // Puntos posibles de patrulla
    public LayerMask capaObstaculos;       // Obstáculos que bloquean la vista
    public LayerMask capaJugador;          // Capa del jugador

    public float distanciaVision = 10f;
    float tiempoEspera = 1f;        // Tiempo de pausa entre puntos

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();

        if (jugador == null)
        {
            Debug.LogWarning("No se asignó el jugador al zombie.");
        }

        if (puntosPatrulla.Length > 0)
        {
            StartCoroutine(Patrullar());
        }
    }

    void Update()
    {
        if (PuedeVerAlJugador())
        {
            StopAllCoroutines(); // Cancela patrullaje si ve al jugador
            agente.SetDestination(jugador.transform.position);
        }
        else if (!patrullando && puntosPatrulla.Length > 0)
        {
            StartCoroutine(Patrullar());
        }
    }

    IEnumerator Patrullar()
    {
        patrullando = true;

        while (!PuedeVerAlJugador())
        {
            int indice = Random.Range(0, puntosPatrulla.Length);
            agente.SetDestination(puntosPatrulla[indice].position);

            // Esperar a llegar al destino
            while (agente.pathPending || agente.remainingDistance > 0.5f)
            {
                if (PuedeVerAlJugador()) yield break;
                yield return null;
            }

            // Esperar unos segundos en el punto
            float timer = 0f;
            while (timer < tiempoEspera)
            {
                if (PuedeVerAlJugador()) yield break;
                timer += Time.deltaTime;
                yield return null;
            }
        }

        patrullando = false;
    }

    bool PuedeVerAlJugador()
    {
        if (jugador == null) return false;

        Vector3 origen = transform.position + Vector3.up;
        Vector3 direccion = (jugador.transform.position - origen).normalized;

        if (Physics.Raycast(origen, direccion, out RaycastHit hit, distanciaVision, capaJugador | capaObstaculos))
        {
            if (((1 << hit.collider.gameObject.layer) & capaJugador) != 0)
            {
                return true;
            }
        }

        return false;
    }
}
