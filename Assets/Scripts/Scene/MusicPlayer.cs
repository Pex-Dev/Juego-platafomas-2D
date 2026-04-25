using System.Collections;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public string currentPlaying = "None";
    public string nextSong = "none";

    public AudioSource audioSourceA;
    public AudioSource audioSourceB;

    private float crossfadeTime = 1f;

    private AudioSource currentSource;
    private AudioSource nextSource;


    void Awake()
    {
        currentSource = audioSourceA;
        nextSource = audioSourceB;
    }

    public void PlayNewMusic(AudioClip newClip, bool stopCurrent = false)
    {     
        if(currentSource.clip == null && nextSource.clip == null)
        {
            currentSource.clip = newClip;
            currentSource.Play();
            currentPlaying = newClip.name;
            return;
        }
        StopAllCoroutines();      
        nextSource.clip = newClip;        
        nextSong = newClip.name;

        if (stopCurrent)
        {
            nextSource.volume = 1f;
            nextSource.Play();
            currentSource.volume = 0;
            currentSource.Stop();
            return;
        }        
        nextSource.volume = 0;
        nextSource.Play();
        StartCoroutine(Crossfade());
    }

    public void setLoop(bool value)
    {
        currentSource.loop = value;
        nextSource.loop = value;
    }
    
    private IEnumerator Crossfade()
    {   
        Debug.Log("iniciando fade");
        float t = 0f;
        while (t < crossfadeTime)
        {
            t += Time.deltaTime;
            currentSource.volume = Mathf.Lerp(1f, 0f, t / crossfadeTime);
            nextSource.volume = Mathf.Lerp(0f, 1f, t / crossfadeTime);
            yield return null;
        }

        currentSource.Stop();

        // Intercambiamos referencias para la próxima transición
        AudioSource temp = currentSource;
        currentSource = nextSource;
        nextSource = temp;
        Debug.Log("Fade terminado");
    }
}
