using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int monedas;
    public int nivelesDesbloqueados;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            monedas = PlayerPrefs.GetInt("Monedas", 0);
            nivelesDesbloqueados = PlayerPrefs.GetInt("NivelDesbloqueado",0);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AgregarMonedas(int cantidad)
    {
        monedas += cantidad;
        PlayerPrefs.SetInt("Monedas", monedas);
        PlayerPrefs.Save();
    }

    public void DesbloquearNivel(int nivel)
    {
        int nivelActual = PlayerPrefs.GetInt("NivelDesbloqueado", 1);

        if (nivel > nivelActual)
        {
            PlayerPrefs.SetInt("NivelDesbloqueado", nivel);
            PlayerPrefs.Save();
        }
    }
}