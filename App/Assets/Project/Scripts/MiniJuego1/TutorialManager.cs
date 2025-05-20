using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  // Para manejar escenas

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel;
    public Button continueButton;
    public Button reopenTutorialButton;

    // Nombre de la escena donde el tutorial inicia cerrado
    public string escenaTutorialCerrado;

    private void Start()
    {
        // Si estamos en la escena que quieres que inicie cerrado
        if (SceneManager.GetActiveScene().name == escenaTutorialCerrado)
        {
            tutorialPanel.SetActive(false); // inicia cerrado
        }
        else
        {
            tutorialPanel.SetActive(true);  // inicia abierto en otras escenas
        }

        continueButton.onClick.AddListener(CloseTutorial);
        reopenTutorialButton.onClick.AddListener(OpenTutorial);
    }

    void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
    }

    void OpenTutorial()
    {
        tutorialPanel.SetActive(true);
    }
}
