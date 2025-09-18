public class EsperarSonido : NodoBT
{
    private Zombie zombie;

    public EsperarSonido(Zombie z)
    {
        zombie = z;
    }

    public override bool Ejecutar()
    {
        return zombie.EsperarEnSonido();
    }
}
