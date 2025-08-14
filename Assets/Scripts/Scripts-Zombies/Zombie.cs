using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    private NavMeshAgent agente;
    private bool patrullando = false;

    [Header("Objetivos")]
    public GameObject jugador;
    public Transform[] puntosPatrulla;

    [Header("Detección")]
    public LayerMask capaObstaculos;
    public LayerMask capaJugador;
    public float distanciaVision = 10f;
    public float anguloVision = 90f; // Ángulo del cono de visión
    public float tiempoEspera = 0.5f;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();

        if (jugador == null)
            Debug.LogWarning("No se asignó el jugador al zombie.");

        if (puntosPatrulla.Length > 0)
            StartCoroutine(Patrullar());
    }

    void Update()
    {
        if (PuedeVerAlJugador())
        {
            if (patrullando)
            {
                StopAllCoroutines();
                patrullando = false;
            }

            if (jugador != null)
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

        while (true)
        {
            if (PuedeVerAlJugador()) break;

            int indice = Random.Range(0, puntosPatrulla.Length);
            agente.SetDestination(puntosPatrulla[indice].position);

            while (agente.pathPending || agente.remainingDistance > 0.5f)
            {
                if (PuedeVerAlJugador()) yield break;
                yield return null;
            }

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

        Vector3 direccionJugador = (jugador.transform.position - transform.position).normalized;
        float distanciaAlJugador = Vector3.Distance(transform.position, jugador.transform.position);

        // 1️⃣ Verificar distancia
        if (distanciaAlJugador > distanciaVision) return false;

        // 2️⃣ Verificar ángulo de visión
        float angulo = Vector3.Angle(transform.forward, direccionJugador);
        if (angulo > anguloVision / 2f) return false;

        // 3️⃣ Raycast para verificar que no hay obstáculos
        if (Physics.Raycast(transform.position + Vector3.up, direccionJugador, out RaycastHit hit, distanciaVision, capaJugador | capaObstaculos))
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
        StopAllCoroutines();
        agente.SetDestination(posicion);
    }

    // DEBUG VISUAL
    void OnDrawGizmosSelected()
    {
        // Rango de visión
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaVision);

        // Líneas del cono
        Vector3 derecha = Quaternion.Euler(0, anguloVision / 2, 0) * transform.forward;
        Vector3 izquierda = Quaternion.Euler(0, -anguloVision / 2, 0) * transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, derecha * distanciaVision);
        Gizmos.DrawRay(transform.position, izquierda * distanciaVision);
    }
    public void VolverAPatrullar()
{
    StopAllCoroutines();
    if (puntosPatrulla.Length > 0)
    {
        StartCoroutine(Patrullar());
    }
}
}
