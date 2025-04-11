using UnityEngine;
using System.Collections;

public class CubeController : MonoBehaviour
{
    [Header("Configuración")]
    public int cubeID;
    public Vector3 initialPosition;
    
    [Header("Estado")]
    public SafeZone currentSafeZone;
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
        if(isLocked) return;
        
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
            transform.position = Vector3.Lerp(startPos, initialPosition, elapsedTime/duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        transform.position = initialPosition;
        transform.localScale = originalScale;
    }

    public void MoveToHeight(float y)
    {
        StartCoroutine(MoveToHeightCoroutine(y));
    }

    private IEnumerator MoveToHeightCoroutine(float targetY)
    {
        float duration = 0.5f;
        float elapsedTime = 0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(startPos.x, targetY, startPos.z);
        
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime/duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        transform.position = endPos;
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
        if(currentSafeZone != null)
        {
            currentSafeZone.ReleaseCube(this);
            currentSafeZone = null;
        }
        isInSafeZone = false;
    }

    public void AdjustScaleToZone(Vector3 zoneScale)
    {
        Vector3 newScale = new Vector3(
            //zoneScale.x - 5f,
            zoneScale.x - 1f,
            transform.localScale.y,
            zoneScale.z- 1f
            //zoneScale.z - 3f
        );
        transform.localScale = newScale;
    }
}