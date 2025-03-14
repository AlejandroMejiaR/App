using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnButton : MonoBehaviour
{
    public void ReturnToLobby()
    {
        // Obtener el progreso actual y sumarle 1 al regresar
        int progresoActual = PlayerPrefs.GetInt("MinijuegosCompletados", 0);
        progresoActual++;
        PlayerPrefs.SetInt("MinijuegosCompletados", progresoActual);
        PlayerPrefs.Save(); // Guardar los datos

        // Cargar la escena del Lobby
        SceneManager.LoadScene("Lobby");
    }
}