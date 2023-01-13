using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseSizeToken : Token
{

    public override void action(Snake snake)
    {
        snake.addSnakePart();
        grid.addIncreaseToken();
        tile.token = null;
        grid.removeIncreaseTokenFormTokenList(this);


        Destroy(gameObject);
    }

    public Vector2 getPosition()
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
