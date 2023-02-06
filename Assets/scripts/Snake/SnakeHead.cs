using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SnakeHead : SnakePart
{
    private SnakeAI AI;

    public override void Heuristic(int value)
    {
        AI.heuristicValue = value;
    }
    public override void hasWon()
    {
        AI.winnGameReward();
    }

    public override void hasLost()
    {
        AI.loseGameReward();
    }

    public override void aiEndEpisode()
    {
        if (AI != null)
        {
            AI.endAIEpisode();
        }
    }

    public override void collisionAction()
    {
        if (AI != null)
        {
            AI.aiDeath();
        }
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
            if (AI != null)
            {
                AI.snakeIncreaseReward();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            AI = GetComponent<SnakeAI>();
        }
        catch (Exception e)
        {

        }
    }

    // Update is called once per frame

    void Update()
    {

    }
}
