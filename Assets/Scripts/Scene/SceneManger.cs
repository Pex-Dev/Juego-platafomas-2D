using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager instance;
    public GameObject screenFaderPrefab;
    private ScreenFader screenFader;

    void Awake()
    {   
        DontDestroyOnLoad(gameObject);

        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void ChangeScene(string sceneName)
    {
        StartCoroutine(FadeAndChangeScene(sceneName));
    }

    private IEnumerator FadeAndChangeScene(string sceneName)
    {
        // Instanciamos el fader si no existe
        if (screenFader == null)
        {
            GameObject faderObject = Instantiate(screenFaderPrefab);
            DontDestroyOnLoad(faderObject);
            screenFader = faderObject.GetComponent<ScreenFader>();
        }

        // Fade a negro
        yield return screenFader.FadeCoroutine(0f, 1f);

        // Cargar la escena de forma asíncrona
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
            yield return null;

        // Fade de negro a visible
        yield return screenFader.FadeCoroutine(1f, 0f);
        if (screenFader != null)
        {
            Destroy(screenFader.gameObject);
        }
    }
}
