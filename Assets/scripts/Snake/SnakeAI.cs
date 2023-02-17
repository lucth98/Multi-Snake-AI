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

    private int length = 1;


    public KeyCode buttonTurnLeft = KeyCode.A;
    public KeyCode buttonTurnRight = KeyCode.D;

    private float lastDistanceToInceaseToken = 0.0f;


    public int heuristicValue { get; set; }

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

        //  AddReward(0.5f);
    }

    public void winnGameReward()
    {
        // AddReward(0.5f);

    }

    public void loseGameReward()
    {
        //  AddReward(-0.5f);
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

        //if (newDistance > 1)
        //{


        //    reward = 1 / newDistance;
        //}
        //else
        //{
        //    reward = 1;
        //}

        //reward *= 0.3f;
        //Debug.Log("Distanz reward="+reward);

        if (newDistance < lastDistanceToInceaseToken)
        {

            SetReward(reward);
            Debug.Log("distanze reward= " + reward);
        }
        else
        {
            SetReward(-reward);
            Debug.Log("distanze reward= " + (-reward));
        }

        lastDistanceToInceaseToken = newDistance;
    }

    public override void Heuristic(in ActionBuffers actions)
    {

        //    Debug.Log("Heuristic");
        var discreteActions = actions.DiscreteActions;
        discreteActions[0] = heuristicValue;
        heuristicValue = 0;

        /*
         if (Input.GetKey(buttonTurnLeft))
         {
             Debug.Log("Heuristic Left");
             discreteActions[0] = 1;
         }
         if (Input.GetKey(buttonTurnRight))
         {
             Debug.Log("Heuristic right");
             discreteActions[0] = 2;
         }*/

    }

    public override void OnEpisodeBegin()
    {
       
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //Hier wird mit den sensor daten an die AI zu verbeiten geschickt

        //Anz an Elementen
        float snakeLenght = (float)length;
        sensor.AddObservation(snakeLenght);

        // distanz zum token
        sensor.AddObservation(lastDistanceToInceaseToken);
    }

    public void endAIEpisode()
    {
        setEndReward();

        Debug.Log(" ");
        Debug.Log("!!!!!!!!!!!!!!!!!!!!11 ");
        Debug.Log(" Cumulative ende reward= " + GetCumulativeReward() + " length= " + length);
        length = 1;
        EndEpisode();
    }

    private void setEndReward()
    {
        float newReward = 0;
        Debug.Log("Length= " + length);
        //if (length > 1)
        //{
        //    newReward = length * 0.1f;
        //    if(newReward > 1)
        //    {
        //        newReward = 1;
        //    }
        //}
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

        Debug.Log("End Reward= " + newReward);
        SetReward(newReward);
    }


    public void aiDeath()
    {
        //AI punishment for Dying

        Debug.Log("Death Reward= " + -1);
        SetReward(-1.0f);
        // Testen: Vieleicht straffe erhöhen mit länge zb strafe = -länge der Schlange -50
    }

    public void snakeIncreaseReward()
    {
        Debug.Log("Increase Reward= " + 1);
        SetReward(1.0f);
        length++;
        // Testen: Vieleicht rewart erhöhen mit länge zb rewart = länge der Schlange
    }



    public override void OnActionReceived(ActionBuffers actions)
    {
        distanceToTokenRewart();

        int movmentAction = actions.DiscreteActions[0];

        if (movmentAction == 0)
        {
            //  Debug.Log("go Straight ahead");
            return;
        }
        if (movmentAction == 1)
        {
            // Debug.Log("turn left");
            snake.makeAITurn(false);
        }
        if (movmentAction == 2)
        {
            // Debug.Log("turn right");
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
