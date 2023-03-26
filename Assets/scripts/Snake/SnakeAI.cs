using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors.Reflection;

//Multi Snake
public class SnakeAI : Agent
{
    private CameraSensorComponent cameraSensor;
    private Grid grid;
    private SnakeHead head;
    private Snake snake;

    private int length = 1;

   // private List<Vector2> previosPositions = new List<Vector2>();
    private List<int> previosTurns = new List<int>();

    public KeyCode buttonTurnLeft = KeyCode.A;
    public KeyCode buttonTurnRight = KeyCode.D;

    private float lastDistanceToInceaseToken = 0.0f;


    public int heuristicValue { get; set; }

    private void init()
    {
        // cameraSensor = GetComponent<CameraSensorComponent>();
        head = GetComponent<SnakeHead>();
        snake = head.snake;
        grid = snake.getGrid();

        //   cameraSensor.Camera = Camera.main;



    }

    public void enemyDethReward()
    {
        AddReward(0.5f);
    }

    public void winnGameReward()
    {
        // AddReward(0.5f);

    }

    public void loseGameReward()
    {
        //  AddReward(-0.5f);
    }

    private bool cheackIfAgentMakeLoops(int turn)
    {
        bool result = false;
        //Vector2 currentPositon = snake.getHeadPosition();
        //if (previosPositions.Count != 0)
        //{
        //    if (previosPositions[0] == currentPositon)
        //    {
        //        result = true;
        //    }
        //}

        //previosPositions.Add(currentPositon);

        //if(previosPositions.Count > 4)
        //{
        //    previosPositions.RemoveAt(0);
        //}
        if (previosTurns.Count > 1)
        {
            if (turn == 1 || turn == 2)
            {
                if (previosTurns[previosTurns.Count - 1] == turn)
                {
                    if (previosTurns[previosTurns.Count - 2] == turn)
                    {
                        result = true;
                    }

                }
            }
        }

        previosTurns.Add(turn);

        if (previosTurns.Count > 3)
        {
            previosTurns.RemoveAt(0);
        }

        return result;
    }


    private void loopPunishment(int turn)
    {

        if (cheackIfAgentMakeLoops(turn))
        {
            AddReward(-0.8f);
           
        }

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
        float reward = 0.1f;

        if (newDistance < lastDistanceToInceaseToken)
        {
            AddReward(reward);
        }
        else
        {
            AddReward(-reward);
        }

        lastDistanceToInceaseToken = newDistance;
    }

    public override void Heuristic(in ActionBuffers actions)
    {
        var discreteActions = actions.DiscreteActions;
        discreteActions[0] = heuristicValue;
        heuristicValue = 0;
    }

    public override void OnEpisodeBegin()
    {

    }

    private Vector2 vector2valueNormalization(Vector2 value, float min, float max)
    {
        Vector2 result = value;
        result.x = valueNormalization(result.x, min, max);
        result.y = valueNormalization(result.y, min, max);

        return result;
    }

    private float valueNormalization(float value, float min, float max)
    {
        return (value - min) / (max - min);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        ////Hier wird mit den sensor daten an die AI  geschickt

        ////Anz an Elementen
        //float lengthOfSnake = valueNormalization((float)length, 0, 100);
        //sensor.AddObservation(lengthOfSnake);
        ////Debug.Log("Length Snake:");
        ////Debug.Log(lengthOfSnake);

        ////snake position
        //Vector2 snakePos = vector2valueNormalization(snake.getHeatPositionVector(), 0, grid.height);
        //sensor.AddObservation(snakePos);
        ////Debug.Log("Snake Pos:");
        ////Debug.Log(snakePos); ;

        //// distanz zum token
        //float distanceToken = valueNormalization(lastDistanceToInceaseToken, 0, grid.getDiagonalLenght());
        //sensor.AddObservation(distanceToken);
        ////Debug.Log("Distance Token:");
        ////Debug.Log(distanceToken); 

        ////Token psoition
        //Vector2 tokenPos = vector2valueNormalization(grid.getFirstToken().getPostionVector(), 0, grid.height);
        //sensor.AddObservation(tokenPos);
        ////Debug.Log("Token Pos:");
        ////Debug.Log(tokenPos); 
        ///

        if(previosTurns.Count == 3)
        {
            sensor.AddObservation(valueNormalization(previosTurns[0], 0, 3));
            sensor.AddObservation(valueNormalization(previosTurns[1], 0, 3));
            sensor.AddObservation(valueNormalization(previosTurns[2], 0, 3));
        }

    }

    public void endAIEpisode()
    {
        setEndReward();

        length = 1;
        EndEpisode();
    }

    private void setEndReward()
    {
        float newReward = 0;

        if (length > 3)
        {
            newReward = 1;
        }
        else if (length > 1)
        {
            newReward = 0;
        }
        else
        {
            newReward = -1;
        }

        SetReward(newReward);
    }


    public void aiDeath()
    {
        //AI punishment for Dying
        SetReward(-1.0f);

        resetDistanzToToken();
    }

    public void snakeIncreaseReward()
    {

        AddReward(1.0f);
        length++;

        resetDistanzToToken();
        testOutput("Increase ");
    }

    private void resetDistanzToToken()
    {
        lastDistanceToInceaseToken = grid.getDiagonalLenght() + 1;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        testOutput("Action ");
        int movmentAction = actions.DiscreteActions[0];

        loopPunishment(movmentAction);
        distanceToTokenRewart();

        if (movmentAction == 0)
        {
            //  Fährt gerade
            return;
        }
        if (movmentAction == 1)
        {
            // Linksdrehung
            snake.makeAITurn(false);
        }
        if (movmentAction == 2)
        {
            // Rechtsdrehung
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

    private void testOutput(string message)
    {
        Debug.Log(message + "cul. Reward= "+ GetCumulativeReward());
    }
}
