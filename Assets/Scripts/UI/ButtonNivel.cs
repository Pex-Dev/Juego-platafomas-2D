using UnityEngine;
using UnityEngine.UI;

public class ButtonNivel : MonoBehaviour
{
    public int levelNumber;
    private Button button;
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        if (GameManager.Instance.NivelesActualizado() >= levelNumber)
        {
            button.interactable = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
