using UnityEngine;
using TMPro;
using System.Collections;

public class CoinCounter : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    private int currentCoins = 0;

    void Start()
    {
        AddCoins(GameManager.Instance.MonedasActualizado(),false);
    }

    public void AddCoins(int amount, bool animate = true)
    {
        int targetCoins = currentCoins + amount;
        StopAllCoroutines();
        if (animate && coinText.gameObject.activeInHierarchy)
            StartCoroutine(AnimateCoins(targetCoins, 0.5f)); 
        currentCoins = targetCoins;
        if (!animate)
        {
            coinText.text = currentCoins.ToString();;
        }
    }

    public int GetCoins()
    {
        return currentCoins;
    }

    private IEnumerator AnimateCoins(int target, float duration)
    {
        int start = int.Parse(coinText.text);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            int displayCoins = Mathf.RoundToInt(Mathf.Lerp(start, target, t));
            coinText.text = displayCoins.ToString();
            yield return null;
        }
        coinText.text = target.ToString();
    }
}
