using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HUDProgress : MonoBehaviour
{
    public TextMeshProUGUI progressText;
    public Slider progressBar;
    //public Button logoutButton;

    private int totalMinijuegos = 4;
    private int minijuegosCompletados;

    void Start()
    {
        //if (progressBar == null || progressText == null || logoutButton == null)
        if (progressBar == null || progressText == null)
        {
            Debug.LogError("ERROR: Alguna referencia en HUDProgress no está asignada en el Inspector.");
            return;
        }

        // Obtener progreso actual desde PlayerStateManager
        minijuegosCompletados = PlayerStateManager.Instance.GetProgreso();
        minijuegosCompletados = Mathf.Clamp(minijuegosCompletados, 0, totalMinijuegos);

        UpdateHUD();

        /*logoutButton.onClick.RemoveAllListeners();
        logoutButton.onClick.AddListener(Logout);*/
    }

    // Método para actualizar visuales del HUD según progreso actual
    public void UpdateHUD()
    {
        minijuegosCompletados = PlayerStateManager.Instance.GetProgreso();
        minijuegosCompletados = Mathf.Clamp(minijuegosCompletados, 0, totalMinijuegos);

        progressText.text = $"Minijuegos: {minijuegosCompletados} / {totalMinijuegos}";
        progressBar.value = (float)minijuegosCompletados / totalMinijuegos;

        Debug.Log($"Progreso actualizado: {progressBar.value * 100}%");
    }


    /*public void Logout()
    {
        Debug.Log("Cerrando juego...");

        PlayerStateManager.Instance.ResetProgreso();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Para salir del play mode en editor
        #else
            Application.Quit(); // Para builds
        #endif
    }*/
}
