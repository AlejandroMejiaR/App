using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSFX : MonoBehaviour, IPointerEnterHandler
{
    // Reproducir sonido hover al pasar el mouse
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.instance.ReproducirSFX(AudioManager.instance.sfxHover);
    }

    // Método público para reproducir sonido click, lo llamaremos desde el botón
    public void PlayClickSFX()
    {
        AudioManager.instance.ReproducirSFX(AudioManager.instance.sfxClick);
    }
}
