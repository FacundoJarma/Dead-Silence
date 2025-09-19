public class EsperarSonido : NodoBT
{
    private Zombie z;

    public EsperarSonido(Zombie zombie)
    {
        z = zombie;
    }

    // Retornamos true = terminado (success), false = aún esperando (running/failure segun tu BT)
    public override bool Ejecutar()
    {
        // Si ya no hay sonido, consideralo terminado (no hay nada que esperar)
        if (!z.HaySonidoPendiente())
        {
            return true;
        }

        // Si aún no llegó al destino, seguimos "en progreso"
        if (!z.EstaEnDestinoDelSonido())
        {
            return false;
        }

        // Estamos en el destino: delegamos el temporizador al método de Zombie
        // EsperarEnSonido devuelve true cuando el tiempo de espera llegó
        bool terminado = z.EsperarEnSonido();

        if (terminado)
        {
            // Aseguramos la limpieza/estado final
            z.TerminarSonido(); // opcional si EsperarEnSonido ya lo hace; es seguro llamarlo
            return true;
        }

        // Todavía esperando
        return false;
    }
}
