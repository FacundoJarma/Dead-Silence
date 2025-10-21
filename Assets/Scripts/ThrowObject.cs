using UnityEngine;
using System.Collections.Generic; // Necesario para usar Dictionary

public class ThrowObject : MonoBehaviour
{
    [Header("Prefabs disponibles para lanzar")]
    public GameObject[] throwablePrefabs; // Los arrastrás desde el inspector

    private Dictionary<string, GameObject> throwableDict; // nombre → prefab

    [Header("Punto de lanzamiento")]
    public Transform throwPoint;

    [Header("Fuerza de lanzamiento")]
    public float throwForce = 10f;

    void Start()
    {
        throwableDict = new Dictionary<string, GameObject>();

        foreach (GameObject prefab in throwablePrefabs)
        {
            if (prefab != null && !throwableDict.ContainsKey(prefab.name))
            {
                throwableDict.Add(prefab.name, prefab);
            }
        }
    }

    public void Throw(string objectName)
    {
        if (!throwableDict.ContainsKey(objectName))
        {
            Debug.LogWarning($"No existe un objeto con el nombre: {objectName}");
            return;
        }

        GameObject prefab = throwableDict[objectName];

        GameObject obj = Instantiate(prefab, throwPoint.position, throwPoint.rotation);

        if (obj.GetComponent<DetectLanding>() == null)
            obj.AddComponent<DetectLanding>();

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = Camera.main.transform.forward;
            rb.AddForce(direction.normalized * throwForce, ForceMode.VelocityChange);
        }
    }

}
