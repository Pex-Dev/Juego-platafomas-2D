using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DieAnimation : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private GameObject gorePart;
    [SerializeField] private Sprite bloodSprite;

    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    public  void StartAnimation()
    {
        if(!sr | !gorePart)return;


        List<Sprite> pedazos = GetSpritePieces(sr.sprite);
        Debug.Log(pedazos.Count);

        for (int i = 0; i < pedazos.Count; i++)
        {
            GameObject gorePartInstance = Instantiate(gorePart, transform.position, transform.rotation);
            SpriteRenderer gSr = gorePartInstance.GetComponent<SpriteRenderer>();      
            gSr.sprite = pedazos[i]; 
            ThrowPieces(gorePartInstance);
        }      

        float bloodNumber = Random.Range(5,10);
        for (int i = 0; i < bloodNumber; i++)
        {
            GameObject gorePartInstance = Instantiate(gorePart, transform.position, transform.rotation);
            SpriteRenderer gSr = gorePartInstance.GetComponent<SpriteRenderer>();      
            gSr.sprite = bloodSprite; 
            ThrowPieces(gorePartInstance);
        } 
    }

    private void ThrowPieces(GameObject pedazo)
    {
        Rigidbody2D rb = pedazo.GetComponent<Rigidbody2D>();           

        Vector2 direccion = Random.insideUnitCircle.normalized;

        rb.AddForce(direccion * 8f, ForceMode2D.Impulse);
        rb.AddTorque(Random.Range(-200f, 200f));

        Destroy(pedazo, 5f);     
    }
    

    private List<Sprite> GetSpritePieces(Sprite spriteOriginal)
    {
        List<Sprite> pedazos = new List<Sprite>();

        Texture2D texturaOriginal = spriteOriginal.texture;
        Rect areaEnHoja = spriteOriginal.rect;

        int divisiones = 3;
        
        float anchoPedazo = areaEnHoja.width / divisiones;
        float altoPedazo = areaEnHoja.height / divisiones;
    
        
        for (int y = 0; y < divisiones; y++)
        {
            for (int x = 0; x < divisiones; x++)
            {
                float posX = areaEnHoja.x + (x * anchoPedazo);
                float posY = areaEnHoja.y + (y * altoPedazo);
                Rect nuevaSeccion = new Rect(posX, posY, anchoPedazo, altoPedazo);
            
                Sprite pedazo = Sprite.Create(texturaOriginal, nuevaSeccion, new Vector2(0.5f, 0.5f), spriteOriginal.pixelsPerUnit);
                pedazos.Add(pedazo);
            }
        }

        return pedazos;
    }
}
