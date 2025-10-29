using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour
{
    public bool encendida;      // Estado de la linterna (encendida o apagada)
    public Light lightSpot;     // Referencia directa a la luz (asignala desde el Inspector)

    void Start()
    {
        if (lightSpot == null)
        {
            lightSpot = GetComponentInChildren<Light>();
        }

        encendida = lightSpot != null && lightSpot.enabled;
    }

    public void Turn()
    {
        if (lightSpot == null) return;

        // Alternar encendido/apagado
        lightSpot.enabled = !lightSpot.enabled;
        encendida = lightSpot.enabled;
    }

    public void TurnOff()
    {
        if (lightSpot == null) return;

        // Alternar encendido/apagado
        lightSpot.enabled = false;
        encendida = false;
    }
}
