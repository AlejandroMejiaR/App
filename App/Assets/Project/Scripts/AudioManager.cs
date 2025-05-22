using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioMixer audioMixer;

    [Header("Módulo de música por escena")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private List<SceneAudio> sceneAudioList;

    [Header("Audio Source para SFX")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Clips SFX (Opcional)")]
    public AudioClip sfxTyping;
    public AudioClip sfxClick;
    public AudioClip sfxHover;
    public AudioClip sfxDragStart;
    public AudioClip sfxDragEnd;
    public AudioClip sfxCorrect;
    public AudioClip sfxError;


    [System.Serializable]
    public class SceneAudio
    {
        public string sceneName;   // Nombre exacto de la escena
        public AudioClip clip;     // Clip que quieres reproducir
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (musicSource == null)
            musicSource = GetComponent<AudioSource>();

        if (sfxSource == null)
        {
            // Si no asignaste sfxSource en inspector, creamos uno
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
        }

        musicSource.loop = true;
        musicSource.playOnAwake = false;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    private void PlayMusicForScene(string sceneName)
    {
        var entry = sceneAudioList.Find(e => e.sceneName == sceneName);
        if (entry != null && entry.clip != musicSource.clip)
        {
            musicSource.clip = entry.clip;
            musicSource.Play();
        }
    }

    public void CambiarVolumenMusica(float volumen)
    {
        audioMixer.SetFloat("VolumenMusica", volumen);
    }

    public void CambiarVolumenSFX(float volumen)
    {
        audioMixer.SetFloat("VolumenSFX", volumen);
    }

    // Método público para reproducir cualquier SFX
    public void ReproducirSFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }
}
