using UnityEngine;

public class GameCloser : MonoBehaviour
{
    // Método público para cerrar la aplicación
    public void CloseGame()
    {
        // En el editor de Unity solo muestra un mensaje
        #if UNITY_EDITOR
            Debug.Log("Cerrar juego (esta función funciona solo en builds).");
        #else
            Application.Quit(); // Cierra el juego en la versión compilada
        #endif
    }
}
