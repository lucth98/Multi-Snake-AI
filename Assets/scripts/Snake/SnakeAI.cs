using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class SnakeAI : Agent
{
    private CameraSensorComponent cameraSensor;

    private SnakeHead head;
    private Snake snake;



    private void init()
    {
        cameraSensor = GetComponent<CameraSensorComponent>();
        head = GetComponent<SnakeHead>();
        snake = head.snake;

        cameraSensor.Camera = Camera.main;
    }



    public override void OnEpisodeBegin()
    {
 
       // snake.reset();


    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //Hier wird mit den sensor daten an die AI zu verbeiten geschickt

        //Anz an Elementen
        float snakeLenght = (float)snake.getSnakeLenght();
        sensor.AddObservation(snakeLenght);

    }



    public void aiDeath()
    {
        //AI punishment for Dying

        AddReward(-50.0f);
        // Testen: Vieleicht straffe erhöhen mit länge zb strafe = -länge der Schlange -50

        snake.reset();
        EndEpisode();
    }

    public void snakeIncreaseReward()
    {
        AddReward(1.0f);
        // Testen: Vieleicht rewart erhöhen mit länge zb rewart = länge der Schlange
    }



    public override void OnActionReceived(ActionBuffers actions)
    {
        snake.makeAITurn(actions.DiscreteActions[0] < 0);

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
