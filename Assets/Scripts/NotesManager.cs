using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotesManager : MonoBehaviour
{
    [SerializeField] GameObject noteDisplay;
    [SerializeField] TextMeshProUGUI noteText;


    string[] notes = { "<b>Fecha:</b> 12 de mayo\n\n" +
            "Hoy comenzamos la fase final del <b>Proyecto Re:Vitalis</b>.\n" +
            "Mi equipo y yo creemos que podemos reactivar tejido muerto mediante una combinación de nano células regenerativas y estímulos eléctricos.\n" +
            "Hace unos años era algo impensado por lo que no puedo creer que esto realmente esté sucediendo, años de investigación volcados en un solo proyecto.\n\n" +
            "El director, <b>Dr. Valdez</b>, insiste en acelerar los ensayos humanos. No está entendiendo que aún hay irregularidades en las muestras animales…\n\n" +
            "Pero si todo sale bien, esto cambiará la medicina para siempre, podríamos pensar en un ciclo de vida muchísimo más largo.\n" +
            "Si sale mal… Bueno, no puedo pensar en eso todavía.",

            "<b>Fecha:</b> 17 de mayo\n\n" +
            "<b>Valdez aprobó las pruebas con voluntarios.</b> ¡Voluntarios! Ni siquiera terminamos de estabilizar la fórmula <b>2.3</b>.\n\n" +
            "Me negué a firmar el protocolo, pero no sirvió de nada, él lo hizo de todos modos.\n\n" +
            "Dicen que la compañía necesita resultados ya, que los fondos se están <b>acabando</b>. " +
            "<b><color=#ff4444>¡ACABANDO!?</color></b> No tiene sentido, el Dr. Valdez es un corrupto, redirige los fondos para sus propios intereses.\n\n" +
            "Anoche escuché <i>gritos</i> en el área médica. No sé si fueron reales o parte de mi paranoia.",

             "<b>Fecha:</b> 22 de mayo\n\n" +
            "Tuve que dejar de escribir. Parece que el <b>Dr. Valdez</b> está revisando las pertenencias de los empleados. " +
            "Ya no me importa. Alguien debe decirlo.\n\n" +

            "Hace semanas descubrimos que las células reanimadas no se comportan como esperábamos. " +
            "No regeneran… <b><color=#ff3333>devoran</color></b>. La muestra <b>H-07</b> atacó a un asistente.\n\n" +

            "Se supone que Valdez ordenó incinerar todo, pero no lo hizo. " +
            "La prensa no puede enterarse de que algo falló; si investigan, descubrirán el paradero de los fondos.\n\n" +

            "Quizás me esté volviendo loco, pero <i>hay algo vivo en los ductos de ventilación</i>. " +
            "Farel insiste en que necesito descansar, que debería tomarme unos días.\n\n" +

            "<b><size=110%><color=#ff4444>¡ESO NO TIENE SENTIDO!</color></size></b> " +
            "Yo impulsé este proyecto. Yo cargué con todo.\n" +
            "Si esto se hunde, caerá conmigo.",

               "<b>Fecha:</b> 31 de Julio\n\n" +

                "<b><size=130%><color=#ff0000>YA ES MUY TARDE!</color></size></b>\n\n" +

            "No puedo decir más... si alguien lee esto… <i>no repitan nuestro error</i>.\n" +
            "La vida no debe ser manipulada.\n" +
            "<b><color=#ff4444>Re:Vitalis</color></b> no era un proyecto médico… " +
            "era un intento de jugar a ser dioses.\n\n" +

            "Creo que ahora yo tambi&eacute;m me inyect&eacute;... me cuaa<s><color=#990000>asdhb</color></s>."


    };


    public void openOrClose(int noteid)
    {
        noteText.text = notes[noteid];
        noteDisplay.SetActive(!noteDisplay.active);
    }

}
