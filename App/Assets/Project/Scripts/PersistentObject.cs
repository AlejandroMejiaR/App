using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    private void Awake()
    {
        // Hace que este GameObject no se destruya al cargar una nueva escena
        DontDestroyOnLoad(gameObject);
    }
}

