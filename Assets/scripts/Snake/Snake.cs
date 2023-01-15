using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class Snake : MonoBehaviour
{
    private List<SnakePart> snake = new List<SnakePart>();

    private KeyCode buttonTurnLeft = KeyCode.A;
    private KeyCode buttonTurnRight = KeyCode.D;

    SnakeHead snakeHeadpre;

    private Grid grid;

    public bool isAI { get; set; }
    public int teamID { get; set; }
    public bool isMoving()
    {
        return snake[0].isMoving();
    }

    public int getSnakeLenght()
    {
        return snake.Count;
    }

    public Grid getGrid()
    {
        return grid;
    }

    public Vector2 getHeadPosition()
    {
        Vector2 position = new Vector2();

        position.x = snake[0].x;
        position.y = snake[0].y;

        return position;

    }

    public void setTurnButtons(KeyCode buttonTurnLeft, KeyCode buttonTurnRight)
    {
        this.buttonTurnLeft = buttonTurnLeft;
        this.buttonTurnRight = buttonTurnRight;
    }

    public void move()
    {
        snake[0].moveSnakePart();
    }

    public void turn()
    {
        if (snake[0].turnRequiret)
        {
            return;
        }

        if (!isAI)
        {
            if (Input.GetKeyDown(buttonTurnLeft))
            {
                snake[0].turn(false);
            }
            if (Input.GetKeyDown(buttonTurnRight))
            {
                snake[0].turn(true);

            }
        }
    }

    public void addPartToList(SnakePart part)
    {
        snake.Add(part);

    }

    public void addSnakePart()
    {
        snake[0].addBodyPart();
    }

    public void addBodyToSnake(SnakeBody body)
    {
        snake.Add(body);
    }

    public void changeSpeed(float newSpeed)
    {
        foreach (SnakePart b in snake)
        {
            Debug.Log("change Speed" + "length= " + snake.Count);
            b.changeSpeed(newSpeed);
        }
    }

    public void init(int x, int y, Grid grid)
    {
        if (isAI)
        {

            string nameOfPrefap = "SnakeHeadObject";
            if (teamID != 0)
            {
                nameOfPrefap += teamID.ToString();
            }

            snakeHeadpre = Resources.Load<SnakeHead>(nameOfPrefap);
        } else
        {
            string nameOfPrefap = "PlayerSnakeHead";
           
            snakeHeadpre = Resources.Load<SnakeHead>(nameOfPrefap);
        }
        SnakeHead head = Instantiate(snakeHeadpre, new Vector3(x, y, -2), Quaternion.identity);
        head.direction = SnakePart.Direction.right;
        head.grid = grid;
        head.snake = this;

        head.init();
        snake.Add(head);


        this.grid = grid;

        move();
    }

    private void removeBody()
    {
        if (snake.Count > 1)
        {
            for (int i = 1; i < snake.Count; i++)
            {
                snake[i].deliteSnakePart();
                snake.RemoveAt(i);
                i--;
            }
        }
    }

    public void reset()
    {
        //  removeBody();

        //Reset Snake Head
        Vector2 newHeadPosition = grid.getFreePostion();
        snake[0].teleportPart(newHeadPosition);
        snake[0].moveSnakePart();
        snake[0].nextElement = null;


        //grid.reset();
    }

    public void endAIEpisode()
    {
        snake[0].aiEndEpisode();
    }

    public void killSnake()
    {
        removeBody();
        snake[0].teleportPart(new Vector2(0f, 0f));
        snake[0].moveSnakePart();
        snake[0].stopSnakeMovement();
        snake[0].stopSnakeMovement();
        snake[0].stopSnakeMovement();
        snake[0].stopSnakeMovement();
    }

    public void makeAITurn(bool turnRight)
    {
        if (!snake[0].turnRequiret)
        {
            //dreht die schlage
            snake[0].turn(turnRight);
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
