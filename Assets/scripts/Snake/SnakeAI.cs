using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

//Multi Snake
public class SnakeAI : Agent
{
    private CameraSensorComponent cameraSensor;
    private Grid grid;

    private SnakeHead head;
    private Snake snake;


    private float lastDistanceToInceaseToken = 0.0f;

    private void init()
    {
        cameraSensor = GetComponent<CameraSensorComponent>();
        head = GetComponent<SnakeHead>();
        snake = head.snake;
        grid = snake.getGrid();

        cameraSensor.Camera = Camera.main;
    }

    public void enemyDethReward()
    {
        AddReward(0.5f);
    }

    public void winnGameReward()
    {
        AddReward(0.5f);
    }

    public void loseGameReward()
    {
        AddReward(-0.5f);
    }

    private float calculateDistanz()
    {
        Vector2 position = snake.getHeadPosition();

        List<IncreaseSizeToken> tokens = grid.increaseList;

        if (tokens.Count == 0)
        {
            return 100;
        }

        float newDistance = Vector2.Distance(position, tokens[0].getPosition());

        for (int i = 0; i < tokens.Count; i++)
        {
            float distance = Vector2.Distance(position, tokens[0].getPosition());

            if (distance < newDistance)
            {
                newDistance = distance;
            }
        }

        return newDistance;
    }

    private void distanceToTokenRewart()
    {
        float newDistance = calculateDistanz();
        float reward = 0;

        if (newDistance < lastDistanceToInceaseToken)
        {
            if (newDistance > 1)
            {


                reward = 1 / newDistance;
            }else
            {
                reward = 1;
            }
            AddReward(reward);
        }
        else
        {
            AddReward(-0.1f);
        }

        lastDistanceToInceaseToken = newDistance;
    }

    public override void Heuristic(in ActionBuffers actions)
    {
       

    }



    public override void OnEpisodeBegin()
    {

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //Hier wird mit den sensor daten an die AI zu verbeiten geschickt

        //Anz an Elementen
        float snakeLenght = (float)snake.getSnakeLenght();
        sensor.AddObservation(snakeLenght);



    }

    public void endAIEpisode()
    {
        EndEpisode();
    }


    public void aiDeath()
    {
        //AI punishment for Dying

        AddReward(-1);
        // Testen: Vieleicht straffe erhöhen mit länge zb strafe = -länge der Schlange -50
    }

    public void snakeIncreaseReward()
    {
        AddReward(1.0f);
        // Testen: Vieleicht rewart erhöhen mit länge zb rewart = länge der Schlange
    }



    public override void OnActionReceived(ActionBuffers actions)
    {
        distanceToTokenRewart();

        int movmentAction = actions.DiscreteActions[0];

        if (movmentAction == 0)
        {
            Debug.Log("go Straight ahead");
            return;
        }
        if (movmentAction == 1)
        {
            Debug.Log("turn left");
            snake.makeAITurn(false);
        }
        if (movmentAction == 2)
        {
            Debug.Log("turn right");
            snake.makeAITurn(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
