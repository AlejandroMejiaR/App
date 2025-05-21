using UnityEngine;
using System.Collections;

public class VictoryCreditsManager : MonoBehaviour
{
    [Tooltip("Panel de victoria que aparece al entrar en esta escena")]
    public GameObject victoryPanel;

    [Tooltip("Panel de créditos que debe mostrarse después")]
    public GameObject creditsPanel;

    [Tooltip("Segundos que espera antes de ocultar victoryPanel y mostrar créditos")]
    public float delayBeforeCredits = 10f;

    private void Start()
    {
        // Asegúrate de que los paneles estén en el estado inicial correcto
        if (victoryPanel != null) victoryPanel.SetActive(true);
        if (creditsPanel != null) creditsPanel.SetActive(false);

        // Lanza la rutina de transición
        StartCoroutine(HideVictoryShowCredits());
    }

    private IEnumerator HideVictoryShowCredits()
    {
        yield return new WaitForSeconds(delayBeforeCredits);

        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        if (creditsPanel != null)
            creditsPanel.SetActive(true);
    }
}
