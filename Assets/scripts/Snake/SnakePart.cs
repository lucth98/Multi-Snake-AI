using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Sensors.Reflection;

public class SnakePart : MonoBehaviour
{

    [Observable]
    public int x { get; set; }

    [Observable]
    public int y { get; set; }


    public SnakePart nextElement { get; set; }
    public Direction direction { get; set; }
    public Snake snake { get; set; }
    public enum Direction
    {
        up, down, left, right
    }

    private Tile positonTile;
    private SnakeBody snakeBodypre;

    public Grid grid { get; set; }
    snakeMoveFunction snakeMove;
    private bool addElemnt = false;
    public bool turnRequiret { get; private set; } = false;
    private bool turnType;

    private List<SnakeTurn> turns = new List<SnakeTurn>();

    private void makeTurn()
    {

        //Debug.Log("Turn " + turnType + " !!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        if (turnType)
        {
            switch (direction)
            {
                case Direction.up:
                    direction = Direction.right;
                    break;

                case Direction.down:
                    direction = Direction.left;
                    break;

                case Direction.right:
                    direction = Direction.down;
                    break;

                case Direction.left:
                    direction = Direction.up;
                    break;
            }
        }

        else
        {
            switch (direction)
            {
                case Direction.up:
                    direction = Direction.left;
                    break;

                case Direction.down:
                    direction = Direction.right;
                    break;

                case Direction.right:
                    direction = Direction.up;
                    break;

                case Direction.left:
                    direction = Direction.down;
                    break;
            }
        }
        if (nextElement != null)
        {
            nextElement.informOfTurn(positonTile, turnType);
        }
    }
    public void turn(bool turnRight)
    {
        turnRequiret = true;
        turnType = turnRight;
    }

    public void informOfTurn(Tile positionOfTurn, bool turnRight)
    {
        //Debug.Log("inform of turn");

        SnakeTurn newTurn = new SnakeTurn(); //weiter machen unfertig 
        newTurn.turnType = turnRight;
        newTurn.positionOfTurn = positionOfTurn;

        this.turns.Add(newTurn);

       // Debug.Log(turns.Count);
    }


    private void addBodyToCordinates(int x, int y)
    {
        //TODo shau ob neues Body Part im Spielfelt liegt
        SnakeBody newBodyPart = Instantiate(snakeBodypre, new Vector3(x, y, -2), Quaternion.identity);

        nextElement = newBodyPart;

        newBodyPart.x = x;
        newBodyPart.y = y;

        newBodyPart.grid = grid;
        newBodyPart.direction = direction;
        newBodyPart.snake = snake;


        newBodyPart.init();
        newBodyPart.moveSnakePart();

        newBodyPart.changeSpeed(snakeMove.speed);

        try
        {
            newBodyPart.positonTile = grid.getTile(x, y);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
        snake.addPartToList(newBodyPart);
    }

    private void addPart()
    {
        if (nextElement != null)
        {
            nextElement.addBodyPart();
        }
        else
        {
            switch (direction)
            {
                case Direction.up:
                    addBodyToCordinates(x, y - 1);
                    break;

                case Direction.down:
                    addBodyToCordinates(x, y + 1);
                    break;

                case Direction.right:
                    addBodyToCordinates(x - 1, y);
                    break;

                case Direction.left:
                    addBodyToCordinates(x + 1, y);
                    break;
            }
        }
    }


    public void addBodyPart()
    {
        addElemnt = true;
    }

    public void moveSnakePart()
    {
        if(x ==0 && y == 0)
        {
            stopSnakeMovement();
            return;
        }

        makeAIDessison();

        int targedX = x;
        int targedY = y;

        switch (direction)
        {
            case Direction.up:
                targedY++;
                break;

            case Direction.down:
                targedY--;
                break;

            case Direction.right:
                targedX++;
                break;
            //hjks
            case Direction.left:
                targedX--;
                break;
        }



        snakeMove.moveToSmoothly(new Vector2(targedX, targedY));
    }

    public void stopSnakeMovement()
    {
        snakeMove.stopMovement();
    }

    public bool hasColidet()
    {
        bool result = false;
        if (positonTile.isThisBarrier())
        {
            result = true;
        }
        if (positonTile.snake != null)
        {
            return true;
        }
        if (positonTile.token != null)
        {
            tokenAction(positonTile.token);
        }

        return result;
    }

    public virtual void makeAIDessison()
    {
        //impletaion in sub clases (snake head)
    }


    public virtual void tokenAction(Token token)
    {
        //impletaion in sub clases (snake head)
    }

    public virtual void collisionAction()
    {
        //impletaion in sub clases (snake head)
    }

    public virtual void aiEndEpisode()
    {
        //impletaion in sub clases (snake head)
    }

    public virtual void hasWon()
    {
        //impletaion in sub clases (snake head)
    }

    public virtual void hasLost()
    {
        //impletaion in sub clases (snake head)
    }

    public virtual void Heuristic(int value)
    {
        //impletaion in sub clases (snake head)
    }

    public void movedToPositonCallBack(int newX, int newY)  //wird von snakemove gerufen wenn eine bewegug fertig ist
    {
        this.x = newX;
        this.y = newY;

        try
        {
            if (this.positonTile != null && this.positonTile.snake != null)
            {
                this.positonTile.snake = null;
            }
            this.positonTile = grid.getTile(x, y);

            //check collison
            if (hasColidet())
            {
                collisionAction();
            }

            positonTile.snake = this;
        }
        catch (Exception e)
        {
         //   Debug.LogError(e);
        }

        if (turns.Count != 0)
        {
            Vector2 turnPos = turns[0].positionOfTurn.getPostion();
            //Debug.Log("make informet turn");

            //Debug.Log("Ziel:" + turnPos + " pos" + x + " " + y);
            if (turnPos.x == this.x && turnPos.y == this.y)  //check ob sich im neuen Feld eine Trhung stafinden soll
            {
                turn(turns[0].turnType);

                turns.RemoveAt(0);
            }
        }

        if (addElemnt)
        {
            addElemnt = false;
            addPart();
        }

        if (turnRequiret)
        {
            turnRequiret = false;
            makeTurn();
        }

        moveSnakePart();
    }

    public void changeSpeed(float newSpeed)
    {
        snakeMove.speed = newSpeed;
    }

    public void deliteSnakePart()
    {
        positonTile.snake = null;
        Destroy(gameObject);
    }

    public bool isMoving()
    {
        return snakeMove.isMoving();
    }
    public void init()
    {
        this.x = (int)transform.position.x;
        this.y = (int)transform.position.y;
        snakeBodypre = Resources.Load<SnakeBody>("SnakeBodyObject");
        snakeMove = GetComponent<snakeMoveFunction>();
        snakeMove.snakePart = this;
    }


    public void teleportPart(Vector2 vector)
    {
        positonTile.snake = null;
        transform.position = vector;

        positonTile = grid.getTile((int)vector.x, (int)vector.y);

        x = (int)vector.x;
        y = (int)vector.y;
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
