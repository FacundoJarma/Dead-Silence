using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Zombie : MonoBehaviour
{
    private NavMeshAgent agente;
    private bool patrullando = false;

    public GameObject jugador;
    public Transform[] puntosPatrulla;     // Puntos posibles de patrulla
    public LayerMask capaObstaculos;       // Obstáculos que bloquean la vista
    public LayerMask capaJugador;          // Capa del jugador

    public float distanciaVision = 10f;
    public float tiempoEspera = 0.5f;      // Tiempo de pausa entre puntos

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
        if (GestorSonidos.instancia != null)
        {
            GestorSonidos.instancia.RegistrarZombie(this);
        }
    }

    void Update()
    {
        if (PuedeVerAlJugador())
        {
            if (patrullando)
            {
                StopAllCoroutines();   // Detiene patrullaje si empieza a perseguir
                patrullando = false;
            }

            if (jugador != null)
            {
                agente.SetDestination(jugador.transform.position);
            }
        }
        else if (!patrullando && puntosPatrulla.Length > 0)
        {
            StartCoroutine(Patrullar());
        }
    }

    IEnumerator Patrullar()
    {
        patrullando = true;

        while (true)
        {
            if (PuedeVerAlJugador()) break;

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

        Vector3 origen = transform.position + Vector3.up; // Para que el raycast no toque el suelo
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
    public void IrAHaciaSonido(Vector3 posicion)
    {
        StopAllCoroutines(); // Cancela patrullaje/persecución anterior
        agente.SetDestination(posicion);
    }


}
