using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanel; // Referencia al panel de configuración

    public void Jugar()
    {
        SceneManager.LoadScene("CH_Selection"); // Cambiado a escena de selección de personaje
    }

    public void Salir()
    {
        SceneManager.LoadScene("Register");
    }
}

