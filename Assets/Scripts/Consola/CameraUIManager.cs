using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraUIManager : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject camerasPanel;
    [SerializeField] private Transform player;
    [SerializeField] private List<RawImage> cameraSlots;
    [SerializeField] private List<TextMeshProUGUI> nameSlots;
    [SerializeField] private int renderTextureSize = 256;

    [System.Serializable]
    public class CameraData
    {
        public Camera cameraObject;
        public RenderTexture renderTexture;
        public string name;
    }

    [Header("Cámaras registradas (asignalas en el inspector)")]
    public List<CameraData> allCameras = new List<CameraData>();

    void Start()
    {
        foreach (var camData in allCameras)
        {
            if (camData.cameraObject == null) continue;

            // Crear RenderTexture si no existe
            camData.renderTexture = new RenderTexture(renderTextureSize, renderTextureSize, 16);
            camData.cameraObject.targetTexture = camData.renderTexture;
            camData.cameraObject.enabled = true; // 🔥 IMPORTANTE: mantener la cámara activa

            if (string.IsNullOrEmpty(camData.name))
                camData.name = camData.cameraObject.name;
        }

        camerasPanel.SetActive(false);
    }

    void LateUpdate()
    {
        if (!camerasPanel.activeSelf)
            return;

        if (player == null || allCameras.Count == 0)
            return;

        // Mostrar las 4 cámaras más cercanas
        var sorted = allCameras
            .OrderBy(c => Vector3.Distance(player.position, c.cameraObject.transform.position))
            .Take(4)
            .ToList();

        for (int i = 0; i < cameraSlots.Count; i++)
        {
            if (i < sorted.Count)
            {
                var cam = sorted[i].cameraObject;

                if (cam == null || sorted[i].renderTexture == null)
                    continue;

                // 🔁 Renderizar manualmente cada frame
                cam.Render();

                cameraSlots[i].texture = sorted[i].renderTexture;
                nameSlots[i].text = sorted[i].name;
                cameraSlots[i].gameObject.SetActive(true);
                nameSlots[i].gameObject.SetActive(true);
            }
            else
            {
                cameraSlots[i].gameObject.SetActive(false);
                nameSlots[i].gameObject.SetActive(false);
            }
        }
    }

    public void Show4()
    {
        Debug.Log("Camaras abiertas");
        camerasPanel.SetActive(true);
        Debug.Log(camerasPanel.name);
    }

    public void HidePanel()
    {
        camerasPanel.SetActive(false);
    }
}
