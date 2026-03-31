using UnityEngine;
using UnityEngine.UI;

public class LifeContainer : MonoBehaviour
{
    public int maxLife = 5; //Cantidad maxima de corazones
    public Image[] hearts; //Sprites de corazón 

  

    public void UpdateLifeBar(int life)
    {
        int updatedLife = Mathf.Clamp(life, 0, maxLife);
        
        for(int i = 0; i < maxLife; i++)
        {            
            hearts[i].enabled = i < updatedLife;
        }
    }

}
