using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CameraUIManager : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject camerasPanel;          // Panel que contiene las imágenes
    [SerializeField] private Transform player;                 // Referencia al jugador
    [SerializeField] private List<RawImage> cameraSlots;       // 4 RawImages dentro del panel
    [SerializeField] private int renderTextureSize = 256;      // Resolución de las RenderTextures

    [System.Serializable]
    public class CameraData
    {
        public Camera cameraObject;       // Cámara en el mundo
        public RenderTexture renderTexture; // Textura que captura la cámara
    }

    [Header("Cámaras registradas")]
    public List<CameraData> allCameras = new List<CameraData>();

    void Update()
    {
        // Cerrar panel con Escape
        if (camerasPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            HidePanel();
        }
    }

    public void Show4()
    {
        if (player == null)
        {
            Debug.LogWarning("No se asignó el jugador al CameraUIManager.");
            return;
        }

        if (allCameras.Count == 0)
        {
            Debug.LogWarning("No hay cámaras registradas en CameraUIManager.");
            return;
        }

        // Ordenar las cámaras por distancia al jugador
        var sorted = allCameras
            .OrderBy(c => Vector3.Distance(player.position, c.cameraObject.transform.position))
            .Take(4)
            .ToList();

        camerasPanel.SetActive(true);

        // Asignar texturas a los RawImages y asegurarnos de que la RenderTexture esté activa
        for (int i = 0; i < cameraSlots.Count; i++)
        {
            if (i < sorted.Count && sorted[i].cameraObject != null)
            {
                Camera cam = sorted[i].cameraObject;

                // Si la cámara no tiene RenderTexture asignada, crear una
                if (sorted[i].renderTexture == null)
                {
                    RenderTexture rt = new RenderTexture(renderTextureSize, renderTextureSize, 16);
                    cam.targetTexture = rt;
                    sorted[i].renderTexture = rt;
                }

                cameraSlots[i].texture = sorted[i].renderTexture;
                cameraSlots[i].gameObject.SetActive(true);
            }
            else
            {
                cameraSlots[i].texture = null;
                cameraSlots[i].gameObject.SetActive(false);
            }
        }

        Debug.Log("Mostrando las 4 cámaras más cercanas con video en tiempo real.");
    }

    public void HidePanel()
    {
        camerasPanel.SetActive(false);
    }

    public void RegisterCamera(Camera camObject)
    {
        if (allCameras.Exists(c => c.cameraObject == camObject))
            return;

        allCameras.Add(new CameraData { cameraObject = camObject, renderTexture = null });
    }
}
