using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelData : MonoBehaviour
{
    public string levelName; //Nombre del nivel
    public AudioClip musicScene; //Musica del nivel
    public string nextLevelName; //Nombre del siguiente nivel
    public int nextLevelNumber; //Número del siguiente nivel (Weas marcianas que se me ocurren)
    void Start()
    {
        levelName = SceneManager.GetActiveScene().name;
    }
}
