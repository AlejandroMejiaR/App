using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public GameObject cat; 
    public GameObject man; 
    public GameObject woman; 


    void Start()
    {
        string selected = Scene_loader.selectedCharacter;

        if (selected == "cat")
            Instantiate(cat, transform.position, Quaternion.identity);
        else if (selected == "man")
            Instantiate(man, transform.position, Quaternion.identity);
        else if (selected == "woman")
            Instantiate(woman, transform.position, Quaternion.identity);
        else
            Debug.LogError("Personaje no reconocido: " + selected);
    }
}