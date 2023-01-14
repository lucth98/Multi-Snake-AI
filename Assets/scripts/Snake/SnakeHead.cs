using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHead : SnakePart
{
    private SnakeAI AI;

    public override void aiEndEpisode()
    {
        AI.endAIEpisode();  
    }

    public override void collisionAction()
    {
        AI.aiDeath();
        snake.killSnake();
        grid.reset();
    }

    public override void makeAIDessison()
    {
        if (AI == null)
        {
            return;
        }

        AI.RequestDecision();
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
