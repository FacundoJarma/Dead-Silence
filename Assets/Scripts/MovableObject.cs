using UnityEngine;

public class MovableObject : MonoBehaviour, IInteractable
{
    public bool isHolding = false;
    public float moveForce = 5f;
    public float maxSpeed = 3f; // límite de velocidad
    private Rigidbody rb;


    bool canHold;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Interact()
    {
        if (canHold)
        {
            isHolding = true;
        }
    }

    void FixedUpdate()
    {
        if (isHolding)
        {
            // Dirección horizontal hacia donde mira la cámara
            Vector3 direccion = Camera.main.transform.forward;
            direccion.y = 0;

            // Si no ha alcanzado la velocidad máxima, aplicamos fuerza
            if (rb.velocity.magnitude < maxSpeed)
            {
                rb.AddForce(direccion.normalized * moveForce, ForceMode.Acceleration);
            }
        }
        else
        {      
            rb.velocity *= 0.5f;      
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isHolding = false;
            canHold = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canHold = true;
        }
    }
}
