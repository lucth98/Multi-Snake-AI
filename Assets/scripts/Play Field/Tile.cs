using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool isBarrier = false;
    public SnakePart snake { get; set; }
    public Token token { get; set; }

    public bool color { get; set; }
    public void setCollor(bool type)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (type)
        {
            if (color)
            {
                spriteRenderer.color = new Color32(62, 250, 147, 255);
            }
            else
            {
                spriteRenderer.color = new Color32(255, 255, 255, 255);
            }
        }
        else
        {
            if (color)
            {
                spriteRenderer.color = new Color32(137, 250, 188, 255);
            }
            else
            {
                spriteRenderer.color = new Color32(240, 240, 240, 255);
            }
            
        }
    }

    public void makeBarrier()
    {
        isBarrier = true;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.black;
        gameObject.tag = "End";
    }

    public bool isThisBarrier()
    {
        return isBarrier;
    }


    public Vector2 getPostion()
    {
        return transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
