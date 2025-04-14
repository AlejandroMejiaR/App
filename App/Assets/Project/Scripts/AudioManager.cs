using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // Instancia Singleton
    [SerializeField] private AudioMixer audioMixer;

    private void Awake()
    {
        // Asegurarse de que solo haya una instancia del AudioManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // No destruir al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Si ya existe una instancia, destruir la nueva
        }
    }

    // Método para cambiar el volumen de la música
    public void CambiarVolumenMusica(float volumen)
    {
        audioMixer.SetFloat("VolumenMusica", volumen);
    }

    // Método para cambiar el volumen de los efectos de sonido
    public void CambiarVolumenSFX(float volumen)
    {
        audioMixer.SetFloat("VolumenSFX", volumen);
    }

 
}
