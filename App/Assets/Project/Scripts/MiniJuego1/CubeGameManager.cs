using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGameManager : MonoBehaviour
{
    public static CubeGameManager Instance;
    
    [Header("Referencias")]
    public List<CubeController> allCubes = new List<CubeController>();
    public List<SafeZone> safeZones = new List<SafeZone>();
    
    [Header("Configuración")]
    public float verificationDelay = 1f;
    public float victoryAnimationHeight = 2f;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void CheckAllCubes()
    {
        bool allPlaced = true;
        foreach (CubeController cube in allCubes)
        {
            if (!cube.isInSafeZone)
            {
                allPlaced = false;
                break;
            }
        }

        if (allPlaced)
        {
            StartCoroutine(VerifyCubesCoroutine());
        }
    }

    private IEnumerator VerifyCubesCoroutine()
    {
        yield return new WaitForSeconds(verificationDelay);
        
        bool allCorrect = true;
        foreach (SafeZone zone in safeZones)
        {
            if (!zone.IsCorrectCube())
            {
                allCorrect = false;
            }
        }
        
        if (allCorrect)
        {
            StartCoroutine(VictoryAnimation());
        }
        else
        {
            StartCoroutine(HandleIncorrectPlacement());
        }
    }

    private IEnumerator HandleIncorrectPlacement()
    {
        yield return new WaitForSeconds(0.5f);
        
        foreach (SafeZone zone in safeZones)
        {
            if (zone.currentCube != null && !zone.IsCorrectCube())
            {
                zone.currentCube.Unlock();
                zone.currentCube.ResetPosition();
                zone.ResetZone();
            }
        }
    }

    private IEnumerator VictoryAnimation()
    {
        foreach (CubeController cube in allCubes)
        {
            cube.MoveToHeight(victoryAnimationHeight);
        }
        
        yield return new WaitForSeconds(1f);
        Debug.Log("¡Victoria! Todos los cubos están correctamente colocados");
    }
}