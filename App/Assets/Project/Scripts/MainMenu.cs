using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanel; // Referencia al panel de configuraci�n

    public void Jugar()
    {
        SceneManager.LoadScene("Lobby"); // Cambia por el nombre de tu escena de juego
    }

    public void Salir()
    {
        Application.Quit();
        Debug.Log("Saliendo del juego...");
    }
}

