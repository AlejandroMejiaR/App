using UnityEngine;

public class SettingsPanelManager : MonoBehaviour
{
    public static SettingsPanelManager instance; // Instancia Singleton

    [SerializeField] private GameObject settingsPanel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Hace que el objeto no se destruya al cambiar de escena
        }
        else
        {
            Destroy(gameObject);  // Si ya existe, destruir esta nueva instancia
        }
    }

    // Método para abrir el Settings Panel
    public void OpenSettings()
    {
        settingsPanel.SetActive(true); // Activa el panel de configuraciones
    }

    // Método para cerrar el Settings Panel
    public void CloseSettings()
    {
        settingsPanel.SetActive(false); // Desactiva el panel de configuraciones
    }
}
