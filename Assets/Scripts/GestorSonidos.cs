using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestorSonidos : MonoBehaviour
{
    public static GestorSonidos instancia;
    private List<Zombie> zombiesRegistrados = new List<Zombie>();

    void Awake()
    {
        if (instancia == null) instancia = this;
        else Destroy(gameObject);
    }

    public void RegistrarZombie(Zombie z)
    {
        if (!zombiesRegistrados.Contains(z))
            zombiesRegistrados.Add(z);
    }

    public void EmitirSonido(Vector3 posicion, float radio)
    {
        foreach (Zombie z in zombiesRegistrados)
        {
            if (z != null)
            {
                float distancia = Vector3.Distance(z.transform.position, posicion);
                if (distancia <= radio)
                {
                    z.IrAHaciaSonido(posicion);
                }
            }
        }
    }
}
