using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HUDProgress : MonoBehaviour
{
    public TextMeshProUGUI progressText;
    public Slider progressBar;
    public Button logoutButton; // Botón único para cerrar sesión

    private int totalMinijuegos = 4;
    private int minijuegosCompletados;

    void Start()
    {
        if (progressBar == null || progressText == null || logoutButton == null)
        {
            Debug.LogError("ERROR: Alguna referencia en HUDProgress no está asignada en el Inspector.");
            return;
        }

        minijuegosCompletados = PlayerPrefs.GetInt("MinijuegosCompletados", 0);
        minijuegosCompletados = Mathf.Clamp(minijuegosCompletados, 0, totalMinijuegos);

        progressBar.value = (float)minijuegosCompletados / totalMinijuegos;

        // Solo actualizar si es necesario
        if (minijuegosCompletados > 0)
        {
            UpdateHUD();
        }

        logoutButton.onClick.RemoveAllListeners();  // Borra eventos previos
        logoutButton.onClick.AddListener(Logout);   // Agrega solo uno
    }


    void UpdateHUD()
    {
        if (progressText == null || progressBar == null)
        {
            Debug.LogError("ERROR: HUDProgress no tiene referencias asignadas.");
            return;
        }

        // Evitar llamadas repetidas
        if (progressText.text == $"Minijuegos: {minijuegosCompletados} / {totalMinijuegos}")
        {
            return; // No hacer nada si el texto ya es el mismo
        }

        minijuegosCompletados = Mathf.Clamp(minijuegosCompletados, 0, totalMinijuegos);

        progressText.text = $"Minijuegos: {minijuegosCompletados} / {totalMinijuegos}";
        progressBar.value = (float)minijuegosCompletados / totalMinijuegos;

        Debug.Log($"Progreso actualizado: {progressBar.value * 100}%");
    }


    // Cerrar Sesión (Reinicia progreso y vuelve al menú principal)
    public void Logout()
    {
        Debug.Log("Cerrando sesión...");

        // Reiniciar progreso
        PlayerPrefs.SetInt("MinijuegosCompletados", 0);
        PlayerPrefs.Save();

        // Volver al menú principal
        SceneManager.LoadScene("MainMenu"); // Asegúrate de que "MainMenu" es el nombre correcto de la escena
    }
}

