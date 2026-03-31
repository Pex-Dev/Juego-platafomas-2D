using UnityEngine;
using UnityEngine.UI;

public class LifeContainer : MonoBehaviour
{
    public int maxLife = 5; //Cantidad maxima de corazones
    public Image[] hearts; //Sprites de corazón 

  

    public void UpdateLifeBar(int life)
    {
        int updatedLife = life;
        if(updatedLife < 0) updatedLife = 0;
        if(updatedLife > maxLife) updatedLife = maxLife;
        
        for(int i = 0; i < maxLife; i++)
        {            
            if(i > updatedLife)
            {
                hearts[i].enabled = false;
            }
        }

        if(updatedLife == 0)
        {
            for(int i = 0; i < maxLife; i++)
            {            
                hearts[i].enabled = false;
            }
        }
    }
}
