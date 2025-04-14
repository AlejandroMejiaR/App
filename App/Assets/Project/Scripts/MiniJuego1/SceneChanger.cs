using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [Header("Nombre de la escena a cargar")]
    public string sceneToLoad;

    [Header("Panel opcional para ocultar antes de cambiar de escena")]
    public GameObject winMessagePanel;

    public void LoadScene()
    {
        if (winMessagePanel != null)
        {
            winMessagePanel.SetActive(false); // Ocultar panel antes del cambio de escena
        }

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("No se ha asignado el nombre de la escena a cargar.");
        }
    }
}
