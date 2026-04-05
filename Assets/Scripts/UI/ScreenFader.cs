using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public Image panel;

        
    public IEnumerator FadeCoroutine(float inicio, float fin)
    {
        float tiempo = 0f;
        float duracion = 1f;


        Color c = panel.color;
        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float alpha = Mathf.Lerp(inicio, fin, tiempo / duracion);
            panel.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }
        panel.color = new Color(c.r, c.g, c.b, fin);
    }

}
