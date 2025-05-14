using UnityEngine;

public class PanelController : MonoBehaviour
{
    // Arrastra aquí el panel desde el Inspector
    public GameObject panel;

    // La tecla que quieres usar para abrir/cerrar el panel
    public KeyCode toggleKey = KeyCode.P;  // Puedes cambiar la P por la que quieras

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            // Cambia el estado activo/inactivo del panel
            panel.SetActive(!panel.activeSelf);
        }
    }
}
