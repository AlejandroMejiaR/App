using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnButton : MonoBehaviour
{
    public void ReturnToLobby()
    {
        SceneManager.LoadScene("Lobby"); // Cargar la escena principal
    }
}