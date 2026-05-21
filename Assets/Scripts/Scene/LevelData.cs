using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelData : MonoBehaviour
{
    public string levelName; //Nombre del nivel
    public AudioClip musicScene; //Musica del nivel
    public string nextLevelName; //Nombre del siguiente nivel
    void Start()
    {
        levelName = SceneManager.GetActiveScene().name;
    }
}
