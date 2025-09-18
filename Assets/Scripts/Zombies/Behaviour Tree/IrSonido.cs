public class IrSonido : NodoBT
{
    private Zombie zombie;

    public IrSonido(Zombie z)
    {
        zombie = z;
    }

    public override bool Ejecutar()
    {
        if (zombie.HaySonidoPendiente())
        {
            zombie.IrAlSonido();
            return true; // Sigue buscando el sonido, no termina aquí
        }

        return false; // Si no hay sonido pendiente, termina el nodo
    }
}