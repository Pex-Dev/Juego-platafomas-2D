using UnityEngine;
using TMPro;
using System.Collections;

public class CoinCounter : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    private int currentCoins = 0;

    public void AddCoins(int amount, bool animate = true)
    {
        int targetCoins = currentCoins + amount;
        StopAllCoroutines(); // Evitar que se acumulen varios cambios
        if (animate)
            StartCoroutine(AnimateCoins(targetCoins, 0.5f)); // 0.5 segundos de animación
        currentCoins = targetCoins;
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
