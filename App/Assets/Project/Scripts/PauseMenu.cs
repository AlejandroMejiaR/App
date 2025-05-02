using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject Pausa_Panel; // Referencia al panel de configuración
    public GameObject Settings_Panel;

    public void Configuraciones()
    {
        if (SettingsPanelManager.instance != null)
        {
            SettingsPanelManager.instance.OpenSettings(); // Abre el Settings Panel
        }
    }

    // Método para mostrar/ocultar ajustes (si lo necesitas)
    public void Pausa()
    {
        if (Pausa_Panel != null)
        {
            Pausa_Panel.SetActive(!Pausa_Panel.activeSelf);
        }
    }

    public void Salir()
    {
        SceneManager.LoadScene("Lobby"); // Cambiado a escena de selección de personaje
    }
}