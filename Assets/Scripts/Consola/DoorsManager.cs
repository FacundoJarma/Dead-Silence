using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // 👈 Necesario para TMP_InputField

[System.Serializable]
public class DoorEntry
{
    public string code;
    public GameObject door;
}

public class DoorsManager : MonoBehaviour
{
    [Header("Puertas registradas")]
    public List<DoorEntry> doorEntries = new List<DoorEntry>();

    private Dictionary<string, GameObject> doors = new Dictionary<string, GameObject>();

    [Header("Configuración de apertura")]
    public float openAngle = -90f;
    public float openSpeed = 2f;

    [Header("Referencia UI")]
    public TMP_InputField codeInput; // 👈 Campo donde el jugador escribe el código

    void Start()
    {
        // Cargar las puertas del inspector al diccionario
        foreach (DoorEntry entry in doorEntries)
        {
            if (!doors.ContainsKey(entry.code) && entry.door != null)
            {
                doors.Add(entry.code, entry.door);
            }
        }
    }

    // 👇 Esta función se puede vincular a un botón desde el Inspector
    public void TryOpenDoorFromInput()
    {
        if (codeInput == null)
        {
            Debug.LogWarning("No se asignó el campo de texto (InputField o TMP_InputField).");
            return;
        }

        string code = codeInput.text.Trim(); // Tomamos el texto ingresado

        if (string.IsNullOrEmpty(code))
        {
            FindObjectOfType<AlertManager>().DisplayDangerAlert("Ingrese un código primero.");
            return;
        }

        OpenWithCode(code); // Intentar abrir la puerta con ese código
    }

    public void OpenWithCode(string code)
    {
        if (doors.ContainsKey(code))
        {
            GameObject door = doors[code];
            FindObjectOfType<AlertManager>().DisplaySuccessAlert("Puerta abierta.");
            StartCoroutine(RotateDoor(door.transform));
        }
        else
        {
            FindObjectOfType<AlertManager>().DisplayDangerAlert($"No se encontró ninguna puerta con el código '{code}'.");
        }
    }

    private IEnumerator RotateDoor(Transform doorTransform)
    {
        Quaternion startRotation = doorTransform.rotation;
        Quaternion targetRotation = Quaternion.Euler(
            doorTransform.eulerAngles.x,
            doorTransform.eulerAngles.y + openAngle,
            doorTransform.eulerAngles.z
        );

        float elapsed = 0f;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * openSpeed;
            doorTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed);
            yield return null;
        }
    }
}
