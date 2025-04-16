using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
   // public GameObject settingsPanel; // Referencia al panel de configuración

    public void Jugar()
    {
        SceneManager.LoadScene("CH_Selection"); // Cambiado a escena de selección de personaje
    }

    // Método para cambiar de usuario (antes "Salir")
    public void CambiarUsuario()
    {
        // Marcar que estamos cambiando el usuario (no saliendo)
        PlayerPrefs.SetInt("ChangingUser", 1);
        PlayerPrefs.Save();
        
        SceneManager.LoadScene("Login");
    }

    /*// Método para mostrar/ocultar ajustes (si lo necesitas)
    public void ToggleSettings()
    {
        if(settingsPanel != null)
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
        }
    }*/
}