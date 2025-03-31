using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    
    // Método para ir al Lobby
    public void Jugar()
    {
        SceneManager.LoadScene("Lobby");
    }

    // Método para cambiar de usuario (antes "Salir")
    public void CambiarUsuario()
    {
        // Marcar que estamos cambiando el usuario (no saliendo)
        PlayerPrefs.SetInt("ChangingUser", 1);
        PlayerPrefs.Save();
        
        SceneManager.LoadScene("Login");
    }

    // Método para mostrar/ocultar ajustes (si lo necesitas)
    public void ToggleSettings()
    {
        if(settingsPanel != null)
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
        }
    }
}