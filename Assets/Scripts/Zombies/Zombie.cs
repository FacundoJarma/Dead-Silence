using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    private NavMeshAgent agente;
    private NodoBT arbol;

    [Header("Jugador")]
    public GameObject jugador;
    private HealthManager healthManager;

    private bool persiguiendoJugador = false;
    public float distanciaPerdidaJugador = 15f;

    [Header("Ataque")]
    public int daño = 10;                     // Daño entero (coincide con HealthManager)
    public float tiempoEntreAtaques = 1.2f;   // Tiempo entre golpes
    private float temporizadorAtaque = 0f;

    [Header("Patrulla")]
    public Transform[] puntosPatrulla;

    [Header("Sonido")]
    private Vector3 ultimoSonido;
    public bool haySonido = false;
    public float tiempoEsperaSonido = 2f;
    private float temporizadorSonido = 0f;

    [Header("Debug")]
    public bool mostrarRayos = true;

    private Vector3 ultimaPosicion;
    private float tiempoAtascado = 0f;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        agente.autoRepath = true;
        agente.autoBraking = false;
        agente.avoidancePriority = Random.Range(30, 80);

        if (jugador != null)
            healthManager = jugador.GetComponent<HealthManager>();

        // --- Árbol de comportamiento ---
        arbol = new Selector(
            new Sequence(new VerJugador(this), new PerseguirJugador(this)),
            new Sequence(new HaySonido(this), new IrSonido(this), new EsperarSonido(this)),
            new Patrullar(this)
        );
    }

    void Update()
    {
        temporizadorAtaque -= Time.deltaTime;

        // 🧠 Si está persiguiendo al jugador
        if (persiguiendoJugador && jugador != null)
        {
            float distancia = Vector3.Distance(transform.position, jugador.transform.position);

            // Si el jugador muere o se aleja mucho → volver a patrullar
            if (healthManager != null && (healthManager.currentHealth <= 0 || distancia > distanciaPerdidaJugador))
            {
                persiguiendoJugador = false;
                VolverAPatrullar();
                return;
            }

            agente.SetDestination(jugador.transform.position);
        }
        else
        {
            arbol.Ejecutar();
        }

        // --- Anti-atascos ---
        if (Vector3.Distance(transform.position, ultimaPosicion) < 0.05f)
        {
            tiempoAtascado += Time.deltaTime;
            if (tiempoAtascado > 1.5f)
            {
                RecalcularRuta();
                tiempoAtascado = 0f;
            }
        }
        else tiempoAtascado = 0f;

        ultimaPosicion = transform.position;

        // --- Raycast de visión ---
        if (mostrarRayos && jugador != null)
        {
            Vector3 direccion = (jugador.transform.position - transform.position).normalized * 10f;
            Vector3 origen = transform.position + Vector3.up * 1.5f;
            Debug.DrawRay(origen, direccion, Color.red);
        }
    }

    private void RecalcularRuta()
    {
        if (agente.hasPath)
            agente.SetDestination(agente.destination);
    }

    // -------- FUNCIONES PARA NODOS --------

    public bool EstaEnDestinoDelSonido()
    {
    // Si el zombie no tiene agente o no hay sonido pendiente, retorna falso
    if (agente == null || !haySonido)
        return false;

    // Retorna true si ya llegó al punto del sonido
    return !agente.pathPending && agente.remainingDistance <= agente.stoppingDistance + 0.3f;
    }


    public bool PuedeVerAlJugador()
    {
        if (jugador == null) return false;

        Vector3 dir = (jugador.transform.position - transform.position).normalized;
        float angulo = Vector3.Angle(transform.forward, dir);

        float[] alturas = { 1.5f, 3f };

        Vector3 direccion = (jugador.transform.position - transform.position).normalized;

        if (angulo < 90f) // visión en cono
        {
            foreach (float altura in alturas)
            {
                Vector3 origenRayo = transform.position + Vector3.up * altura;

                // Dibujo para depuración
                Debug.DrawRay(origenRayo, direccion * 10f, Color.red);

                if (Physics.Raycast(origenRayo, direccion, out RaycastHit hit, 10f))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void PerseguirJugador()
    {
        if (jugador != null)
            agente.SetDestination(jugador.transform.position);
    }

    public bool HaySonidoPendiente() => haySonido;

    public void IrAlSonido()
    {
        agente.SetDestination(ultimoSonido);
    }

    public bool EsperarEnSonido()
    {
        temporizadorSonido += Time.deltaTime;

        if (temporizadorSonido >= tiempoEsperaSonido)
        {
            haySonido = false;
            temporizadorSonido = 0f;
            VolverAPatrullar();
            return true;
        }

        return false;
    }

    public void TerminarSonido()
    {
        haySonido = false;
        temporizadorSonido = 0f;
        if (agente != null)
            agente.ResetPath();
    }

    public void IrAHaciaSonido(Vector3 pos)
    {
        Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
        ultimoSonido = pos + offset;
        haySonido = true;

        if (agente != null)
            agente.SetDestination(ultimoSonido);
    }

    public void Patrullar()
    {
        if (puntosPatrulla.Length == 0) return;

        bool nuevoDestino = false;

        if (!agente.hasPath && !agente.pathPending)
            nuevoDestino = true;
        else if (!agente.pathPending && agente.remainingDistance <= agente.stoppingDistance + 0.1f)
            nuevoDestino = true;

        if (nuevoDestino)
        {
            int i = Random.Range(0, puntosPatrulla.Length);
            agente.SetDestination(puntosPatrulla[i].position);
        }
    }

    public void VolverAPatrullar()
    {
        persiguiendoJugador = false;
        haySonido = false;

        if (agente != null)
        {
            agente.ResetPath();
            Patrullar();
        }
    }

    // -------- ATAQUE AL PLAYER --------
    private void OnCollisionStay(Collision collision)
    {
        if (!persiguiendoJugador) return;

        if (collision.collider.CompareTag("Player") && temporizadorAtaque <= 0f)
        {
            if (healthManager == null)
                healthManager = collision.collider.GetComponent<HealthManager>();

            if (healthManager != null && healthManager.currentHealth > 0)
            {
                healthManager.TakeDamage(daño);
                temporizadorAtaque = tiempoEntreAtaques;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (agente != null && agente.hasPath)
        {
            Vector3[] puntos = agente.path.corners;
            Gizmos.color = Color.green;

            for (int i = 0; i < puntos.Length - 1; i++)
            {
                Gizmos.DrawLine(puntos[i], puntos[i + 1]);
                Gizmos.DrawSphere(puntos[i], 0.1f);
            }
        }
    }
}
