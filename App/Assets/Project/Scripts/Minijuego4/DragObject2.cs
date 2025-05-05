using UnityEngine;
using System.Collections;

public class DragObject2 : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;
    private CubeController2 cubeController;
    private bool isDragging = false;
    private Vector3 originalScale;

    private void Start()
    {
        cubeController = GetComponent<CubeController2>();
        originalScale = transform.localScale;
    }

    private void OnMouseDown()
    {
        if (cubeController.isLocked) return;

        isDragging = true;
        mZCoord = Camera.main.WorldToScreenPoint(transform.position).z;
        mOffset = transform.position - GetMouseWorldPos();
        cubeController.ClearSafeZone();
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 inputPoint = Input.mousePosition;

        if (Input.touchCount > 0)
        {
            inputPoint = Input.GetTouch(0).position;
        }

        inputPoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(inputPoint);
    }

    private void OnMouseDrag()
    {
        if (cubeController.isLocked || !isDragging) return;

        Vector3 newPos = GetMouseWorldPos() + mOffset;
        transform.position = new Vector3(newPos.x, 20f, newPos.z);
    }

    private void OnMouseUp()
    {
        if (cubeController.isLocked || !isDragging) return;

        isDragging = false;
        StartCoroutine(FallAndCheckPosition());
    }

    private IEnumerator FallAndCheckPosition()
    {
        float fallDuration = 0.5f;
        float elapsedTime = 0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(startPos.x, 0f, startPos.z); // Caer solo en Y

        while (elapsedTime < fallDuration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / fallDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;

        SafeZone2[] allZones = FindObjectsOfType<SafeZone2>();
        bool foundZone = false;

        foreach (var zone in allZones)
        {
            zone.CheckForCube(cubeController);
            if (cubeController.isInSafeZone)
            {
                foundZone = true;
                cubeController.LockInPlace();  // Bloquear el cubo en su lugar
                break;
            }
        }

        if (!foundZone)
        {
            yield return new WaitForSeconds(0.3f);
            cubeController.ResetPosition();
            transform.localScale = originalScale;  // Restaurar escala original si no se encuentra zona
        }

        CubeGameManager2.Instance.CheckAllCubes();
    }
}
