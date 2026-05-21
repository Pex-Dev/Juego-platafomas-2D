using UnityEngine;

public class MenuPrincipal : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EscenaComoJugar()
    {
        ScenesManager.instance.ChangeScene("Como Jugar");
    }

    public void ChangeScene(string sceneName)
    {
        ScenesManager.instance.ChangeScene(sceneName);        
    }

    public void Salir()
    {
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
