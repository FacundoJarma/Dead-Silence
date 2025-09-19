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
            return zombie.EstaEnDestinoDelSonido(); 
        }
        return false; // no hay sonido = falla
    }
}