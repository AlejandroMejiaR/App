using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoLoader : MonoBehaviour
{
    [Tooltip("Componente VideoPlayer que reproduce el vídeo")]
    public VideoPlayer videoPlayer;

    [Tooltip("Nombre exacto de la escena a cargar al terminar el vídeo")]
    public string nextSceneName = "Login";

    void Start()
    {
        // Suscribirse al evento que salta cuando el vídeo termina
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        // Carga la escena de registro
        SceneManager.LoadScene(nextSceneName);
    }
}
