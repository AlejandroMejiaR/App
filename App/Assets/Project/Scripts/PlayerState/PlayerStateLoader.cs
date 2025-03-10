using UnityEngine;

public class PlayerStateLoader : MonoBehaviour
{
    void Start()
    {
        Invoke("LoadPlayerState", 0f);
    }

    void LoadPlayerState()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            PlayerStateManager.Instance.LoadPlayerState(player);
        }
    }
}
