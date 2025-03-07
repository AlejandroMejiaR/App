using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanel; // Referencia al panel de configuración

    public void Jugar()
    {
        SceneManager.LoadScene("SampleScene"); // Cambia por el nombre de tu escena de juego
    }

    public void Salir()
    {
        Application.Quit();
        Debug.Log("Saliendo del juego...");
    }
}

