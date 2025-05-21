using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;       // <- Para controlar el fade
using UnityEngine.Video;    // <- Para VideoPlayer

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

    void Start()
    {
        // Inicializa el panel totalmente transparente
        SetPanelAlpha(0f);

        // Empieza a reproducir el vídeo y escucha su fin
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        // Inicia la transición y luego carga la escena
        StartCoroutine(FadeOutAndLoad());
    }

    System.Collections.IEnumerator FadeOutAndLoad()
    {
        float elapsed = 0f;

        // Gradual de 0 → 1 en fadeDuration segundos
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float a = Mathf.Clamp01(elapsed / fadeDuration);
            SetPanelAlpha(a);
            yield return null;
        }

        // Asegura que quede completamente negro
        SetPanelAlpha(1f);

        // Carga la siguiente escena
        SceneManager.LoadScene(nextSceneName);
    }

    void SetPanelAlpha(float a)
    {
        if (fadePanel != null)
        {
            Color c = fadePanel.color;
            c.a = a;
            fadePanel.color = c;
        }
    }
}
