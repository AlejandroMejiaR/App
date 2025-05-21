using UnityEngine;
using TMPro;

public class KeyboardTutorialManager : MonoBehaviour
{
    [Header("UI Elements")]
    [Tooltip("Panel raíz con la Image y el Text (TMP)")]
    public GameObject tutorialPanel;
    [Tooltip("Componente TextMeshProUGUI dentro del panel")]
    public TextMeshProUGUI tutorialText;

    [Header("Tutorial Pages")]
    [TextArea(3, 10)]
    public string[] pages;   // Rellena desde el Inspector

    [Header("Key Bindings")]
    public KeyCode toggleKey = KeyCode.P;
    public KeyCode nextKey = KeyCode.D;

    private int currentPage = 0;

    private void Start()
    {
        // 1) Al arrancar el minijuego, mostramos el tutorial
        currentPage = 0;
        ShowPage();
        tutorialPanel.SetActive(true);
    }

    private void Update()
    {
        // 2) Toggle con P: si está activo, cerrar; si está cerrado, abrir y resetear
        if (Input.GetKeyDown(toggleKey))
        {
            if (tutorialPanel.activeSelf)
            {
                tutorialPanel.SetActive(false);
            }
            else
            {
                currentPage = 0;
                ShowPage();
                tutorialPanel.SetActive(true);
            }
        }

        // 3) Avanzar con D sólo si el panel está abierto
        if (tutorialPanel.activeSelf && Input.GetKeyDown(nextKey))
        {
            if (currentPage < pages.Length - 1)
            {
                currentPage++;
                ShowPage();
            }
            else
            {
                // Última página → cerrar tutorial
                tutorialPanel.SetActive(false);
            }
        }
    }

    private void ShowPage()
    {
        tutorialText.text = pages[currentPage];
    }
}
