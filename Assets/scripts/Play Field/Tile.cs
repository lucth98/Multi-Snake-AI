using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    public void setCollor(bool type)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (type)
        {
            spriteRenderer.color = new Color32(62,250,147, 255);
        }
        else
        {
            spriteRenderer.color = new Color32(137, 250, 188, 255);
        }
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