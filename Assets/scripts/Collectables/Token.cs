using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    public Grid grid { get;  set; }
    public Tile tile { get; set; }

    public void deliteToken()
    {
        Destroy(gameObject);
    }
    public virtual void action(Snake snake)
    {

    }

    public int getX()
    {
        return (int)transform.position.x;
    }

    public int getY()
    {
        return (int)transform.position.y;
    }

    public Vector2 getPostionVector()
    {
        Vector2 result = new Vector2();
        result.x = (int)transform.position.x;
        result.y = (int)transform.position.y;

        return result;
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
