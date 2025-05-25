using UnityEngine;
using System.Collections;

public class VictoryCreditsManager : MonoBehaviour
{
    [Tooltip("Panel de victoria que aparece al entrar en esta escena")]
    public GameObject victoryPanel;

    [Tooltip("Panel de cr�ditos que debe mostrarse despu�s")]
    public GameObject creditsPanel;

    [Tooltip("Segundos que espera antes de ocultar victoryPanel y mostrar cr�ditos")]
    public float delayBeforeCredits = 10f;

    private void Start()
    {
        // Aseg�rate de que los paneles est�n en el estado inicial correcto
        if (victoryPanel != null) victoryPanel.SetActive(true);
        if (creditsPanel != null) creditsPanel.SetActive(false);

        // Lanza la rutina de transici�n
        StartCoroutine(HideVictoryShowCredits());
    }

    private IEnumerator HideVictoryShowCredits()
    {
        yield return new WaitForSeconds(delayBeforeCredits);

        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        if (creditsPanel != null)
            victoryPanel.SetActive(true);
    }
}
