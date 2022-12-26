using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHead : SnakePart
{
    private SnakeAI AI;

    public override void collisionAction()
    {
        //snake.changeSpeed(0f);
        AI.aiDeath();
    }

    public override void tokenAction(Token token)
    {
        token.action(snake);


        if (token is IncreaseSizeToken)
        {
            AI.snakeIncreaseReward();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        AI = GetComponent<SnakeAI>();
    }

    // Update is called once per frame

    void Update()
    {

    }
}
