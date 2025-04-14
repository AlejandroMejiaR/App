using UnityEngine;
using UnityEngine.UI;

public class MenuOpciones : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Start()
    {
     
        // Vincular los sliders con los métodos correspondientes
        musicSlider.onValueChanged.AddListener(delegate { CambiarVolumenMusica(); });
        sfxSlider.onValueChanged.AddListener(delegate { CambiarVolumenSFX(); });
    }

    // Cambiar el volumen de la música
    public void CambiarVolumenMusica()
    {
        AudioManager.instance.CambiarVolumenMusica(musicSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value); // Guardar el valor
    }

    // Cambiar el volumen de los SFX
    public void CambiarVolumenSFX()
    {
        AudioManager.instance.CambiarVolumenSFX(sfxSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value); // Guardar el valor
    }
}
