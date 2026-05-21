using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager instance;
    public GameObject screenFaderPrefab;
    [SerializeField] private GameObject audioPlayer; //Prefab para reproducir sonidos
    [SerializeField] private GameObject musicPrefab; //Prefab para reproducir sonidos
    public AudioClip musicScene; //Musica del nivel
    private GameObject musicPlayer;
    private ScreenFader screenFader;

    public LevelData levelData; //datos del nivel

    void Start()
    {      
        GameObject leveldataObject = GameObject.Find("LevelData");
        if(!leveldataObject)return;
        levelData = leveldataObject.GetComponent<LevelData>();
        
        if(!levelData.musicScene)return;
        PlayMusic(levelData.musicScene);
    }

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

    public void PlaySound(AudioClip sound)
    {
        GameObject s = Instantiate(audioPlayer);
        AudioSource a = s.GetComponent<AudioSource>();
        a.clip = sound;
        a.Play();
        Destroy(s, sound.length);//Destruir al terminar audio
    }

    public void PlayMusic(AudioClip music, bool loop = true, bool stopCurrent = false)
    {
        if (!musicPlayer)
        {   
            Debug.Log("Instanciar nuevo music player");
            musicPlayer = Instantiate(musicPrefab);
        }
        musicPlayer.GetComponent<MusicPlayer>().setLoop(loop);
        musicPlayer.GetComponent<MusicPlayer>().PlayNewMusic(music,stopCurrent);
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

        AudioClip newSceneMusic = GetSceneMusic();

        // Fade de negro a visible
        if (newSceneMusic != null)
        {
            PlayMusic(newSceneMusic);            
        }
        yield return screenFader.FadeCoroutine(1f, 0f);
        if (screenFader != null)
        {
            Destroy(screenFader.gameObject);
        }
    }

    private AudioClip GetSceneMusic()
    {
        GameObject leveldataObject = GameObject.Find("LevelData");
        if(!leveldataObject) return null;
        levelData = leveldataObject.GetComponent<LevelData>();
        
        if(!levelData.musicScene) return null;
        return levelData.musicScene;
    }
}
