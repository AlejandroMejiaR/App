using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.IO;       // Necesario para Path.Combine

public class VideoLoader : MonoBehaviour
{
    [Tooltip("Componente VideoPlayer que reproduce el vídeo")]
    public VideoPlayer videoPlayer;

    [Tooltip("Nombre exacto de la escena a cargar al terminar el vídeo")]
    public string nextSceneName = "Login";

    [Tooltip("Nombre del archivo de video en la carpeta StreamingAssets (ej. 'nave.mp4')")]
    public string videoFileName; // ¡NUEVO CAMPO!

    void Start()
    {
        // 1. Validar que el VideoPlayer está asignado
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer no asignado en el Inspector para VideoLoader en " + gameObject.name, this);
            return;
        }

        // 2. Validar que el nombre del archivo de video está configurado
        if (string.IsNullOrEmpty(videoFileName))
        {
            Debug.LogError("Nombre de archivo de video no configurado en el Inspector para VideoLoader en " + gameObject.name, this);
            return;
        }

        // 3. Construir la URL completa del video en StreamingAssets
        // Application.streamingAssetsPath devuelve la ruta correcta para cada plataforma (URL para WebGL)
        string videoURL = Path.Combine(Application.streamingAssetsPath, videoFileName);
        
        // Asegúrate de que las barras sean las correctas para URLs (forward slashes)
        // En Windows, Path.Combine puede usar backslashes, que no funcionan en URLs.
        videoURL = videoURL.Replace('\\', '/');

        Debug.Log("VideoLoader: Intentando cargar video desde URL: " + videoURL);

        // 4. Configurar el VideoPlayer para usar una URL
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoURL;

        // 5. Suscribirse a los eventos de preparación y finalización del video
        videoPlayer.prepareCompleted += OnVideoPrepared;
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.errorReceived += OnVideoError; // Para depuración en caso de problemas de carga

        // 6. Iniciar la preparación del video (lo carga en segundo plano)
        videoPlayer.Prepare();
    }

    // Se llama cuando el VideoPlayer ha terminado de preparar el video desde la URL
    void OnVideoPrepared(VideoPlayer vp)
    {
        Debug.Log("VideoLoader: Video preparado con éxito. Reproduciendo...");
        vp.Play(); // Una vez preparado, lo reproducimos
        
        // Desuscribirse del evento para evitar llamadas múltiples si Prepare() se llamara de nuevo
        videoPlayer.prepareCompleted -= OnVideoPrepared;
    }

    // Se llama si ocurre un error al cargar o reproducir el video
    void OnVideoError(VideoPlayer vp, string message)
    {
        Debug.LogError("VideoLoader Error: " + message);
        // En caso de error, puedes decidir qué hacer.
        // Aquí, simplemente avanzamos a la siguiente escena para no bloquear el juego.
        SceneManager.LoadScene(nextSceneName);
        
        // Desuscribirse del evento para limpiar
        videoPlayer.errorReceived -= OnVideoError;
    }

    // Se llama cuando el video llega a su fin (o al loopPointReached si no está en loop)
    void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("VideoLoader: Video finalizado. Cargando la siguiente escena: " + nextSceneName);
        SceneManager.LoadScene(nextSceneName);
        
        // Desuscribirse del evento para limpiar
        videoPlayer.loopPointReached -= OnVideoFinished;
    }
}