using UnityEngine;

public class EsperarSonido : NodoBT
{
    private Zombie zombie;
    private float tiempoEspera;
    private float temporizador;

    public EsperarSonido(Zombie z, float tiempo = 2f)
    {
        zombie = z;
        tiempoEspera = tiempo;
        temporizador = 0f;
    }

    public override bool Ejecutar()
    {
        // Aumentar el tiempo de espera
        temporizador += Time.deltaTime;

        if (temporizador >= tiempoEspera)
        {
            // Resetear
            temporizador = 0f;
            zombie.VolverAPatrullar(); // ✅ vuelve a patrulla
            return false; // Termina el nodo
        }

        return true; // Sigue esperando
    }
}
