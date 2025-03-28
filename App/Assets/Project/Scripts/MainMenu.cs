using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Authentication;
using System.Threading.Tasks;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanel;

    public void Jugar()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void Salir()
    {
        // Llamamos al método de logout sin hacerlo async
        PerformLogout();
    }

    private async void PerformLogout()
    {
        // Mostrar feedback visual (opcional)
        if (settingsPanel != null) settingsPanel.SetActive(false);
        
        try
        {
            // Verificar y cerrar sesión si está autenticado
            if (AuthenticationService.Instance.IsSignedIn)
            {
                await Task.Run(() => AuthenticationService.Instance.SignOut());
                Debug.Log("Sesión cerrada correctamente");
            }

            // Limpiar datos temporales
            PlayerPrefs.DeleteKey("LastUsername");
            PlayerPrefs.Save();

            // Redirigir a Login
            SceneManager.LoadScene("Login");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error durante logout: {ex.Message}");
            SceneManager.LoadScene("Login"); // Redirigir igualmente
        }
    }
}