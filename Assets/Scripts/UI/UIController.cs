using UnityEngine;

public class UIController : MonoBehaviour
{
    public LifeContainer lifeController;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLife(int life)
    {
        lifeController.UpdateLifeBar(life);
    }
}
