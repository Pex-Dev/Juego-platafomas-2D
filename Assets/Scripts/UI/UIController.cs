using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public LifeContainer lifeController;

    [SerializeField] private GameObject playingScreen;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private CoinCounter coinCounter;

    void Start()
    {
        playingScreen.SetActive(true);
    }

    public void AddCoins(int nCoins)
    {
        coinCounter.AddCoins(nCoins);
    }

    public void SetLife(int life)
    {
        lifeController.UpdateLifeBar(life);
    }

    public void ShowPlayingScreen(bool value)
    {
        playingScreen.SetActive(value);
        if (value)
        {
            deathScreen.SetActive(false);
        }
    }
    public void ShowDeathScreen(bool value)
    {
        deathScreen.SetActive(value);
        if (value)
        {
            playingScreen.SetActive(false);
        }
    }

    public void ResetEscene()
    {
        ScenesManager.instance.ChangeScene(SceneManager.GetActiveScene().name);
    }
}
