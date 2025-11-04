using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    private NavMeshAgent agente;
    private NodoBT arbol;

    public float[] alturas = { 1.5f, 3f };

    [Header("Jugador")]
    public GameObject jugador;
    private HealthManager healthManager;

    private bool persiguiendoJugador = false;
    public float distanciaPerdidaJugador = 15f;
    [SerializeField] float distaciaVision = 20f;

    [Header("Ataque")]
    public int daño = 10;
    public float tiempoEntreAtaques = 1.2f;
    public float rangoAtaque = 1.6f;
    private bool puedeAtacar = true;
    private bool atacando = false;

    [Header("Patrulla")]
    public Transform[] puntosPatrulla;

    [Header("Sonido de comportamiento")]
    public AudioSource audioSource;   // 🎵 Asignar un AudioSource en el inspector
    public float minTiempoSonido = 5f;
    public float maxTiempoSonido = 10f;

    [Header("Sonido de eventos")]
    private Vector3 ultimoSonido;
    public bool haySonido = false;
    public float tiempoEsperaSonido = 2f;
    private float temporizadorSonido = 0f;

    [Header("Debug")]
    public bool mostrarRayos = true;

    private Vector3 ultimaPosicion;
    private float tiempoAtascado = 0f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
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

        // 🔁 Iniciar la rutina de sonidos aleatorios
        if (audioSource != null)
            StartCoroutine(ReproducirSonidosAleatorios());
    }

    void Update()
    {
        // Actualizar animación
        if (animator != null && agente != null)
        {
            float velocidadActual = agente.velocity.magnitude;
            animator.SetFloat("Speed", velocidadActual);
        }

        if (jugador == null)
        {
            arbol.Ejecutar();
            return;
        }

        if (persiguiendoJugador)
        {
            float distancia = Vector3.Distance(transform.position, jugador.transform.position);

            if (healthManager != null && (healthManager.currentHealth <= 0 || distancia > distanciaPerdidaJugador))
            {
                persiguiendoJugador = false;
                VolverAPatrullar();
                return;
            }

            if (distancia <= rangoAtaque)
            {
                agente.isStopped = true;
                agente.velocity = Vector3.zero;
                if (puedeAtacar && !atacando)
                    StartCoroutine(Atacar());
            }
            else
            {
                agente.isStopped = false;
                agente.SetDestination(jugador.transform.position);
            }
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

        // --- Debug Rayos ---
        if (mostrarRayos && jugador != null)
        {
            Vector3 direccion = (jugador.transform.position - transform.position).normalized * 10f;
            Vector3 origen = transform.position + Vector3.up * 1.5f;
            Debug.DrawRay(origen, direccion, Color.red);
        }
    }

    // 🔊 Corrutina para emitir gruñidos aleatorios
    private IEnumerator ReproducirSonidosAleatorios()
    {
        while (true)
        {
            float espera = Random.Range(minTiempoSonido, maxTiempoSonido);
            yield return new WaitForSeconds(espera);

            if (audioSource != null)
            {

                audioSource.Play();
            }
        }
    }

    private void RecalcularRuta()
    {
        if (agente.hasPath)
            agente.SetDestination(agente.destination);
    }

    private IEnumerator Atacar()
    {
        animator.SetInteger("AttackType", Random.Range(1, 3));
        atacando = true;
        puedeAtacar = false;

        Vector3 direccion = (jugador.transform.position - transform.position).normalized;
        direccion.y = 0;
        transform.rotation = Quaternion.LookRotation(direccion);

        if (healthManager != null)
            healthManager.TakeDamage(daño);

        yield return new WaitForSeconds(tiempoEntreAtaques);

        puedeAtacar = true;
        atacando = false;
        animator.SetInteger("AttackType", 0);
    }

    // -------- FUNCIONES PARA NODOS --------

    public bool EstaEnDestinoDelSonido()
    {
        if (agente == null || !haySonido)
            return false;
        return !agente.pathPending && agente.remainingDistance <= agente.stoppingDistance + 0.3f;
    }

    public bool PuedeVerAlJugador()
    {
        if (jugador == null) return false;

        Vector3 dir = (jugador.transform.position - transform.position).normalized;
        float angulo = Vector3.Angle(transform.forward, dir);

        

        if (angulo < 90f)
        {
            foreach (float altura in alturas)
            {
                Vector3 origenRayo = transform.position + Vector3.up * altura;
                Debug.DrawRay(origenRayo, dir * distaciaVision, Color.red);

                if (Physics.Raycast(origenRayo, dir, out RaycastHit hit, distaciaVision))
                {
                    if (hit.collider.CompareTag("Player"))
                        return true;
                }
            }
        }
        return false;
    }

    public void PerseguirJugador()
    {
        if (jugador == null) return;
        persiguiendoJugador = true;
        agente.isStopped = false;
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
