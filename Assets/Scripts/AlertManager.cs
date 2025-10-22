using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlertManager : MonoBehaviour
{
    public GameObject Info;
    public TextMeshProUGUI Info_TEXT;
    public GameObject Success;
    public TextMeshProUGUI Success_TEXT;
    public GameObject Danger;
    public TextMeshProUGUI Danger_TEXT;

    public void DisplaySuccessAlert(string text, float duration = 2f)
    {
        Success_TEXT.text = text;
        Success.SetActive(true);
        StartCoroutine(HideAlertAfterDelay(duration, Success));
    }

    public void DisplayDangerAlert(string text, float duration = 2f)
    {
        Danger_TEXT.text = text;
        Danger.SetActive(true);
        StartCoroutine(HideAlertAfterDelay(duration, Danger));
    }

    private IEnumerator HideAlertAfterDelay(float delay, GameObject alert)
    {
        yield return new WaitForSeconds(delay);
        alert.SetActive(false);
    }

    public void DisplayInfo(string text, float duration = 2f)
    {
        Info_TEXT.text = text;
        Info.SetActive(true);
        StartCoroutine(HideAlertAfterDelay(duration, Info));
    }
}
