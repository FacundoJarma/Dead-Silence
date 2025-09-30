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

    [Header("Patrulla General")]
    public Transform[] puntosPatrulla;

    // --- Sonido ---
    private Vector3 ultimoSonido;
    public bool haySonido = false;
    public float tiempoEsperaSonido = 2f;
    private float temporizadorSonido = 0f;

    // --- Anti-bug / Anti-atascos ---
    private Vector3 ultimaPosicion;
    private float tiempoAtascado = 0f;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();

        // Configuración para evitar bugs
        agente.avoidancePriority = Random.Range(20, 80); 
        agente.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        agente.autoRepath = true;
        agente.autoBraking = false;

        // Armamos el árbol:
        arbol = new Selector(
                new Sequence(
                    new VerJugador(this),
                    new PerseguirJugador(this)
                ),
                new Sequence(
                    new HaySonido(this),
                    new IrSonido(this), // Se mueve al sonido
                    new EsperarSonido(this) // Espera en el sonido
                ),
                new Patrullar(this) // Patrullaje general
            );
    }

    void Update()
    {
        arbol.Ejecutar(); 

        // --- Anti-atascos ---
        if (Vector3.Distance(transform.position, ultimaPosicion) < 0.05f)
        {
            tiempoAtascado += Time.deltaTime;
            if (tiempoAtascado > 1.5f) // Si no se movió en 1.5s
            {
                RecalcularRuta();
                tiempoAtascado = 0f;
            }
        }
        else
        {
            tiempoAtascado = 0f;
        }

        ultimaPosicion = transform.position;

        // Dibujo del rayo de visión
        if (jugador != null)
        {
            Vector3 direccion = (jugador.transform.position - transform.position).normalized * 10f;
            Vector3 origenRayo = transform.position + Vector3.up * 3f;
            Debug.DrawRay(origenRayo, direccion, Color.red);
        }
    }

    private void RecalcularRuta()
    {
        if (agente.hasPath)
        {
            agente.SetDestination(agente.destination); // recalcula el camino actual
        }
    }

    // --------- FUNCIONES PARA LOS NODOS -----------
    public bool PuedeVerAlJugador()
    {
        if (jugador == null) return false;

        Vector3 direccion = (jugador.transform.position - transform.position).normalized;
        float angulo = Vector3.Angle(transform.forward, direccion);

        if (angulo < 90f) // visión en cono
        {
            // Alturas desde las que se lanzarán los rayos
            float[] alturas = { 1.5f, 3f };

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
        agente.SetDestination(jugador.transform.position);
    }

    public bool HaySonidoPendiente()
    {
        Debug.Log("HaySonidoPendiente: " + haySonido);

        return haySonido;
    }

    public void IrAlSonido()
    {
        Debug.Log("IrAlSonido");
        agente.SetDestination(ultimoSonido);
    }

    public bool EstaEnDestinoDelSonido()
    {
        if (!haySonido) return false;

        if (agente.pathPending) return false;
        bool arrived = (!agente.hasPath && agente.remainingDistance <= agente.stoppingDistance + 0.5f)
            || (agente.hasPath && agente.remainingDistance <= agente.stoppingDistance + 0.5f);
        Debug.Log("Esta en sonido: " + arrived);

        return (!agente.hasPath && agente.remainingDistance <= agente.stoppingDistance + 0.5f)
            || (agente.hasPath && agente.remainingDistance <= agente.stoppingDistance + 0.5f);
    }

    public bool EsperarEnSonido()
    {
        temporizadorSonido += Time.deltaTime;

        if (temporizadorSonido >= tiempoEsperaSonido)
        {
            haySonido = false;
            temporizadorSonido = 0f;
            
            return true; 

        }

        return false;
    }

    public void TerminarSonido()
    {
        haySonido = false;
        temporizadorSonido = 0f;

        if (agente != null)
        {
            agente.ResetPath();

        }
    }


    public void IrAHaciaSonido(Vector3 pos)
    {
        // Offset aleatorio para que no se amontonen todos
        Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
        ultimoSonido = pos + offset;
        haySonido = true;

        if (agente != null)
        {
            agente.SetDestination(ultimoSonido);
        }
    }

    public void Patrullar()
    {
        if (puntosPatrulla.Length == 0) return;

        bool necesitaNuevoDestino = false;

        if (!agente.hasPath && !agente.pathPending)
            necesitaNuevoDestino = true;
        else if (!agente.pathPending && agente.remainingDistance <= agente.stoppingDistance + 0.1f)
            necesitaNuevoDestino = true;

        if (necesitaNuevoDestino)
        {
            int indice = Random.Range(0, puntosPatrulla.Length);
            agente.SetDestination(puntosPatrulla[indice].position);
        }

    }

    void OnDrawGizmos()
    {
        if (agente != null && agente.hasPath)
        {
            Vector3[] corners = agente.path.corners;
            Gizmos.color = Color.green;

            for (int i = 0; i < corners.Length - 1; i++)
            {
                Gizmos.DrawLine(corners[i], corners[i + 1]);
                Gizmos.DrawSphere(corners[i], 0.1f);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.name);
    }
}
