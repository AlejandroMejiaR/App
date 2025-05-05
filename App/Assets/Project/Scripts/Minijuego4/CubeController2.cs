using UnityEngine;
using System.Collections;

public class CubeController2 : MonoBehaviour
{
    [Header("Configuración")]
    public int cubeID;
    public Vector3 initialPosition;

    [Header("Estado")]
    public SafeZone2 currentSafeZone;
    public bool isInSafeZone;
    public bool isLocked;

    private Vector3 originalScale;

    private void Start()
    {
        initialPosition = new Vector3(transform.position.x, 20f, transform.position.z);
        originalScale = transform.localScale;
        ResetPosition();
    }

    public void ResetPosition()
    {
        if (isLocked) return;
        
        ClearSafeZone(); // Limpia las referencias de la zona
        StartCoroutine(RiseToInitialPosition());
    }
    
        private IEnumerator RiseToInitialPosition()
    {
        ClearSafeZone();

        float duration = 0.5f;
        float elapsedTime = 0f;
        Vector3 startPos = transform.position;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPos, initialPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = initialPosition;
        transform.localScale = originalScale;
    }

    public void LockInPlace()
    {
        isLocked = true;
    }

    public void Unlock()
    {
        isLocked = false;
    }

    public void ClearSafeZone()
    {
        if (currentSafeZone != null)
        {
            currentSafeZone.ReleaseCube(this);
            currentSafeZone = null;
        }
        isInSafeZone = false;
    }

    public void AdjustScaleToZone(Vector3 zoneScale)
    {
        // Eliminado el reescalado de acuerdo a la zona
        transform.localScale = originalScale;  // Mantener la escala original
    }
}

