using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_loader : MonoBehaviour
{
    public static string selectedCharacter; // Almacena el personaje seleccionado

    // Cambia de escena
    public void Change_Scene(string newScene)
    {
        SceneManager.LoadScene(newScene);
    }

    // Asigna el personaje seleccionado
    public void SetSelectedCharacter(string character)
    {
        selectedCharacter = character;
    }
}