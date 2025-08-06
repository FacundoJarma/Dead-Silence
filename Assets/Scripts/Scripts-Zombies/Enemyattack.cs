using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemysatt : MonoBehaviour
{
    public int daño = 10; // Cantidad de daño que hace el zombie

    void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que tocó tiene el componente HealthManager
        HealthManager salud = other.GetComponent<HealthManager>();

        if (salud != null)
        {
            salud.TakeDamage(daño);
            Debug.Log("Zombie hizo daño al jugador.");
        }
    }
}
