using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public LifeContainer lifeController;

    [SerializeField] private GameObject playingScreen;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject levelCompleteScreen;
    [SerializeField] private CoinCounter coinCounter;

    [SerializeField] private bool isPaused = false;
    [SerializeField] private bool death = false;


    void Start()
    {
        playingScreen.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !death && !levelCompleteScreen.activeInHierarchy)
        {
            isPaused = !isPaused;
            PauseGame(isPaused);
        }
    }

    public void AddCoins(int nCoins)
    {
        coinCounter.AddCoins(nCoins);
    }


    public void SetLife(int life)
    {
        lifeController.UpdateLifeBar(life);
    }

    public void PauseGame(bool value)
    {        
        Time.timeScale = value ? 0f : 1f;
        if (value)
        {
            ShowPauseScreen(true);
        }
        else
        {
            ShowPauseScreen(false);
            ShowPlayingScreen(true);
        }
    }

    public void ShowPlayingScreen(bool value)
    {
        playingScreen.SetActive(value);
        if (value)
        {
            deathScreen.SetActive(false);
            pauseScreen.SetActive(false);
            levelCompleteScreen.SetActive(false);
        }
    }

    public void ShowDeathScreen(bool value)
    {
        death = true;
        deathScreen.SetActive(value);
        if (value)
        {
            playingScreen.SetActive(false);
            pauseScreen.SetActive(false);
            levelCompleteScreen.SetActive(false);
        }
    }
    
    public void ShowPauseScreen(bool value)
    {
        pauseScreen.SetActive(value);
        if (value)
        {
            playingScreen.SetActive(false);
            deathScreen.SetActive(false);
            levelCompleteScreen.SetActive(false);
        }
    }
    public void ShowCompleteLevelScreen(bool value)
    {
        levelCompleteScreen.SetActive(value);
        if (value)
        {
            playingScreen.SetActive(false);
            deathScreen.SetActive(false);
            pauseScreen.SetActive(false);            
        }
    }

    public void ResetScene()
    {
        Time.timeScale = 1f;
        ScenesManager.instance.ChangeScene(SceneManager.GetActiveScene().name);
    }

    public void NextScene()
    {
        GameObject leveldataObject = GameObject.Find("LevelData");
        if (leveldataObject!= null)
        {
            LevelData levelData = leveldataObject.GetComponent<LevelData>();
            if (ScenesManager.instance.DoesSceneExist(levelData.nextLevelName))
            {                
                GameManager.Instance.DesbloquearNivel(levelData.nextLevelNumber);
            }
        }
        Time.timeScale = 1f;
        GameManager.Instance.AgregarMonedas(coinCounter.GetCoins());       
        ScenesManager.instance.NextLevel();
    }

    public void SelectorNiveles()
    {
        Time.timeScale = 1f;
        ScenesManager.instance.ChangeScene("Selector Niveles");
    }
}
