using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject tutorialPanel;
    public TextMeshProUGUI tutorialText;
    public Button continueButton;
    public Button reopenTutorialButton;

    [Header("Pages Content")]
    [TextArea(3, 10)]
    public string[] pages;
    

    [Header("Opcional: cerrar al iniciar en esta escena")]
    public string escenaTutorialCerrado;

    private int currentPage = 0;

    private void Start()
    {
        // 1) Iniciar cerrado si estamos en la escena indicada
        if (!string.IsNullOrEmpty(escenaTutorialCerrado) &&
            SceneManager.GetActiveScene().name == escenaTutorialCerrado)
        {
            tutorialPanel.SetActive(false);
        }
        else
        {
            tutorialPanel.SetActive(true);
        }

        // 2) Mostrar primera página
        ShowPage(0);

        // 3) Conectar eventos
        continueButton.onClick.AddListener(OnContinueClicked);
        reopenTutorialButton.onClick.AddListener(OnReopenClicked);
    }

    private void ShowPage(int pageIndex)
    {
        currentPage = pageIndex;
        tutorialText.text = pages[pageIndex];

        // cambiar texto del botón
        var label = continueButton.GetComponentInChildren<TMP_Text>();
        if (label != null)
        {
            label.text = (pageIndex < pages.Length - 1) ? "Siguiente" : "Finalizar";
        }
    }

    private void OnContinueClicked()
    {
        if (currentPage < pages.Length - 1)
        {
            // paso a la siguiente “pantalla”
            ShowPage(currentPage + 1);
        }
        else
        {
            // última página → cerrar tutorial
            tutorialPanel.SetActive(false);
        }
    }

    private void OnReopenClicked()
    {
        // reiniciar al abrir de nuevo
        ShowPage(0);
        tutorialPanel.SetActive(true);
    }

    private void OnDestroy()
    {
        // limpieza de listeners (opcional pero recomendable)
        continueButton.onClick.RemoveListener(OnContinueClicked);
        reopenTutorialButton.onClick.RemoveListener(OnReopenClicked);
    }
}

