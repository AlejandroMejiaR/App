using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;       // Para controlar el fade
using UnityEngine.Video;    // Para VideoPlayer
using System.IO;        // Necesario para Path.Combine
using System.Collections; // Necesario para IEnumerator

public class LoadingMenu : MonoBehaviour
{
    [Header("Componentes")]
    [Tooltip("Componente VideoPlayer que reproduce el vídeo")]
    public VideoPlayer videoPlayer;
    [Tooltip("Imagen negra que usaremos para el fade")]
    public Image fadePanel;

    [Header("Configuración")]
    [Tooltip("Nombre exacto de la escena a cargar al terminar el vídeo")]
    public string nextSceneName = "MainMenu";
    [Tooltip("Duración del fade en segundos")]
    public float fadeDuration = 1f;
    [Tooltip("Nombre del archivo de video en la carpeta StreamingAssets (ej. 'IntroSonido.mp4')")]
    public string videoFileName; // ¡NUEVO CAMPO!

    void Start()
    {
        // Inicializa el panel totalmente transparente
        SetPanelAlpha(0f);

        // 1. Validar que el VideoPlayer está asignado
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer no asignado en el Inspector para LoadingMenu en " + gameObject.name, this);
            return;
        }

        // 2. Validar que el nombre del archivo de video está configurado
        if (string.IsNullOrEmpty(videoFileName))
        {
            Debug.LogError("Nombre de archivo de video no configurado en el Inspector para LoadingMenu en " + gameObject.name, this);
            return;
        }

        // 3. Construir la URL completa del video en StreamingAssets
        string videoURL = Path.Combine(Application.streamingAssetsPath, videoFileName);
        videoURL = videoURL.Replace('\\', '/'); // Asegúrate de que las barras sean correctas

        Debug.Log("LoadingMenu: Intentando cargar video desde URL: " + videoURL);

        // 4. Configurar el VideoPlayer para usar una URL
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoURL;

        // 5. Suscribirse a los eventos de preparación y finalización del video
        videoPlayer.prepareCompleted += OnVideoPrepared;
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.errorReceived += OnVideoError; // Para depuración en caso de problemas de carga

        // 6. Iniciar la preparación del video
        videoPlayer.Prepare();
    }

    // Se llama cuando el VideoPlayer ha terminado de preparar el video desde la URL
    void OnVideoPrepared(VideoPlayer vp)
    {
        Debug.Log("LoadingMenu: Video preparado con éxito. Reproduciendo...");
        vp.Play(); // Una vez preparado, lo reproducimos
        videoPlayer.prepareCompleted -= OnVideoPrepared;
    }

    // Se llama si ocurre un error al cargar o reproducir el video
    void OnVideoError(VideoPlayer vp, string message)
    {
        Debug.LogError("LoadingMenu Error: " + message);
        // Si hay un error, puedes decidir qué hacer.
        // Aquí, iniciamos el fade para avanzar a la siguiente escena incluso sin el video.
        StartCoroutine(FadeOutAndLoad()); 
        videoPlayer.errorReceived -= OnVideoError;
    }

    // Se llama cuando el video llega a su fin
    void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("LoadingMenu: Video finalizado. Iniciando fade para cargar la siguiente escena.");
        // Inicia la transición (fade out) y luego carga la escena
        StartCoroutine(FadeOutAndLoad());
        videoPlayer.loopPointReached -= OnVideoFinished;
    }

    // Coroutine para el efecto de fade out y carga de escena
    System.Collections.IEnumerator FadeOutAndLoad()
    {
        float elapsed = 0f;

        // Gradual de 0 (transparente) a 1 (negro opaco) en fadeDuration segundos
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float a = Mathf.Clamp01(elapsed / fadeDuration);
            SetPanelAlpha(a);
            yield return null; // Espera al siguiente frame
        }

        // Asegura que el panel quede completamente negro al final
        SetPanelAlpha(1f);

        // Carga la siguiente escena
        SceneManager.LoadScene(nextSceneName);
    }

    // Función para ajustar la transparencia del panel de fade
    void SetPanelAlpha(float a)
    {
        if (fadePanel != null)
        {
            Color c = fadePanel.color;
            c.a = a; // Solo modifica el canal alfa (transparencia)
            fadePanel.color = c;
        }
    }
}