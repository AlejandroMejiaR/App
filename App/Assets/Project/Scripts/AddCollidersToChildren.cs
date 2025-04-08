using UnityEngine;

public class AddCollidersToChildren : MonoBehaviour
{
    void Start()
    {
        AddCollidersRecursively(transform);
    }

    void AddCollidersRecursively(Transform obj)
    {
        // Si el objeto tiene una malla y no tiene collider, se le añade un MeshCollider
        if (obj.GetComponent<MeshFilter>() != null && obj.GetComponent<Collider>() == null)
        {
            MeshCollider meshCollider = obj.gameObject.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = obj.GetComponent<MeshFilter>().sharedMesh; // Asignar la malla correcta
            meshCollider.convex = false; // No convex para evitar colisiones prematuras
        }

        // Recorre todos los hijos y aplica la función recursivamente
        foreach (Transform child in obj)
        {
            AddCollidersRecursively(child);
        }
    }
}
