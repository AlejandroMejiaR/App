using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveButton3D : MonoBehaviour
{
    [Header("Button Settings")]
    public float pressDistance = 0.05f;
    public float pressSpeed = 5f;
    public float springSpeed = 10f;
    
    [Header("Events")]
    public UnityEvent onButtonPressed;
    
    [Header("Audio")]
    public AudioClip buttonClickSound;
    
    private Vector3 originalPosition;
    private Vector3 pressedPosition;
    private bool isPressed = false;
    private bool isAnimating = false;
    private AudioSource audioSource;
    private MeshRenderer meshRenderer;
    private Color originalColor;
    public Color hoverColor = new Color(0.8f, 0.8f, 1f);
    
    void Start()
    {
        // Guardar la posición original
        originalPosition = transform.position;
        // Calcular la posición cuando está presionado
        pressedPosition = originalPosition - transform.up * pressDistance;
        
        // Configurar componentes
        if (!TryGetComponent(out audioSource))
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            originalColor = meshRenderer.material.color;
        }
    }
    
    void OnMouseEnter()
    {
        if (meshRenderer != null && !isPressed)
        {
            meshRenderer.material.color = hoverColor;
        }
    }
    
    void OnMouseExit()
    {
        if (meshRenderer != null && !isPressed)
        {
            meshRenderer.material.color = originalColor;
        }
    }
    
    void OnMouseDown()
    {
        if (!isAnimating && !isPressed)
        {
            StartCoroutine(AnimateButtonPress());
        }
    }
    
    IEnumerator AnimateButtonPress()
    {
        isAnimating = true;
        isPressed = true;
        
        // Animar presionando el botón
        float t = 0;
        Vector3 startPosition = transform.position;
        
        while (t < 1)
        {
            t += Time.deltaTime * pressSpeed;
            transform.position = Vector3.Lerp(startPosition, pressedPosition, t);
            yield return null;
        }
        
        // Reproducir sonido si hay uno asignado
        if (buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
        
        // Invocar evento
        onButtonPressed?.Invoke();
        
        // Esperar un poco mientras el botón está presionado
        yield return new WaitForSeconds(0.1f);
        
        // Animar liberando el botón
        t = 0;
        startPosition = transform.position;
        
        while (t < 1)
        {
            t += Time.deltaTime * springSpeed;
            transform.position = Vector3.Lerp(startPosition, originalPosition, t);
            yield return null;
        }
        
        if (meshRenderer != null)
        {
            meshRenderer.material.color = originalColor;
        }
        
        isPressed = false;
        isAnimating = false;
    }
}