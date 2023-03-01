using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Grid : MonoBehaviour
{
    // public Snake snake { get; private set; }

    public Snake[] snakes { get; private set; }

    public int height = 60;

    public int with = 60;

    public int numberOfSnakes = 1;

    public int numberOfIncreaseSizeTokens = 1;

    public bool enabelHumanPlayer = false;

    public bool color = true;

    private Tile tile;

    private Tile[,] field;

    private IncreaseSizeToken increase;


    public List<IncreaseSizeToken> increaseList { get; private set; }

    public int getDiagonalLenght()
    {
        double result = 0;

        result = height*height + with*with;
        result = Math.Sqrt(result);

        return (int)result;
    }

    public IncreaseSizeToken getFirstToken()
    {
        return increaseList[0];
    }
    public void drawGrid()
    {
        for (int x = 0; x < with; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var newTile = Instantiate(tile, new Vector3(x, y, 1), Quaternion.identity);

                newTile.name = "Tile Pos: X=" + x + " Y=" + y;

                newTile.color = color;

                newTile.setCollor((x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0));

                field[y, x] = newTile;
            }
        }
    }

    private void makeBarres()
    {
        for (int i = 0; i < with; i++) //unterer rand
        {
            getTile(i, 0).makeBarrier();
        }
        for (int i = 0; i < height; i++) //linker rand
        {
            getTile(0, i).makeBarrier();
        }
        for (int i = 0; i < with; i++) //oberer rand
        {
            getTile(i, height - 1).makeBarrier();
        }
        for (int i = 0; i < height; i++)//rechter rand
        {
            getTile(with - 1, i).makeBarrier();
        }
    }

    public void setSize(int with, int height)
    {
        this.with = with;
        this.height = height;
    }

    private void moveCamera()
    {
        Camera camera = Camera.main;
        camera.transform.position = new Vector3(((float)with) / 2, ((float)height) / 2, -10);
        if (height >= with)
        {
            camera.orthographicSize = (float)height / 2 + 2;
        }
        else
        {
            camera.orthographicSize = (float)with / 2 + 2;
        }
        camera.backgroundColor = Color.white;
    }

    public Tile getTile(int x, int y)
    {
        return field[y, x];
    }




    private void init()
    {
        drawGrid();
        makeBarres();
        addTokens();

    }


    public Vector2 getFreePostion()
    {
        Vector2 result = new Vector2();

        System.Random random = new System.Random();
        int x;
        int y;
        Tile tile;
        do
        {
            x = random.Next(0, with);
            y = random.Next(0, height);

            tile = getTile(x, y);

        } while (tile.snake != null || tile.token != null || tile.isThisBarrier());

        result.x = x;
        result.y = y;

        return result;
    }

    private void initSnakes()
    {
        int snakeCount = 0;

        if (enabelHumanPlayer)
        {
            snakeCount = numberOfSnakes + 1;
        }
        else
        {
            snakeCount = numberOfSnakes;
        }


        snakes = new Snake[snakeCount];

        for (int i = 0; i < numberOfSnakes; i++)
        {
            

            Vector2 position = getFreePostion();

            snakes[i] = new Snake();
            snakes[i].teamID = i+1;
            snakes[i].isAI = true;
            snakes[i].init((int)position.x, (int)position.y, this);
        

        }

        initPlayerSnake();

        addSnakeControlls();
    }

    private void initPlayerSnake()
    {
        if (enabelHumanPlayer)
        {
            Snake snake = new Snake();
            snake.isAI = false;

            Vector2 position = getFreePostion();
            snake.init((int)position.x, (int)position.y, this);

            snakes[snakes.Length - 1] = snake;
        }
    }

    public bool checkIfAllSnakesAreDead()
    {
        for (int i = 0; i < snakes.Length; i++)
        {
            if (snakes[i].isMoving())
            {
                return false;
            }
        }
        return true;

    }

    public void addIncreaseToken()
    {
        Debug.Log("add increse Token");
        System.Random random = new System.Random();

        int tokenX = 0;
        int tokenY = 0;

        Tile tokenTile;
        do
        {
            tokenX = random.Next(1, with - 2);
            tokenY = random.Next(1, height - 2);

            tokenTile = getTile(tokenX, tokenY);
        } while (tokenTile.token != null);


        IncreaseSizeToken newToken = Instantiate(increase, new Vector3(tokenX, tokenY, -1), Quaternion.identity);
        newToken.grid = this;
        tokenTile.token = newToken;
        newToken.tile = tokenTile;

        increaseList.Add(newToken);
    }

    public void removeIncreaseTokenFormTokenList(IncreaseSizeToken increaseSizeToken)
    {
        increaseList.Remove(increaseSizeToken);
    }

    private void addTokens()
    {
        increase = Resources.Load<IncreaseSizeToken>("IncreasSizeObject");

        increaseList = new List<IncreaseSizeToken>();


        for (int i = 0; i < numberOfIncreaseSizeTokens; i++)
        {
            addIncreaseToken();
        }
    }

    private void addSnakeControlls() //for testing
    {
        if (snakes.Length > 1)
        {
            snakes[0].setTurnButtons(KeyCode.Q, KeyCode.E);
            snakes[1].setTurnButtons(KeyCode.A, KeyCode.D);
        }


    }

    private bool gameHasEndedInADrawn()
    {
        int sizeOfSnake = snakes[0].getSize();

        for(int i = 0; i < snakes.Length; i++)
        {
            if(snakes[i].getSize() != sizeOfSnake)
            {
                return false;
            } 
        }
        return true;
    }

    private int getSizeOfLargestSnake()
    {
        int size = snakes[0].getSize();

        for(int i = 0; i < snakes.Length; i++)
        {
            if(snakes[i].getSize() > size)
            {
                size = snakes[i].getSize();
            }
        }
        return size;
    }

    private int getSizeOfSmallestSnake()
    {
        int size = snakes[0].getSize();

        for (int i = 0; i < snakes.Length; i++)
        {
            if (snakes[i].getSize() < size)
            {
                size = snakes[i].getSize();
            }
        }
        return size;
    }

    private void endGameRoundRewarts()
    {

        int sizeOfLargestSnake = getSizeOfLargestSnake();
        int sizeOfSmalestSnake = getSizeOfSmallestSnake();

        //Debug.Log("winner size = " + sizeOfLargestSnake);
        //Debug.Log("loser size = " + sizeOfSmalestSnake);

        for(int i = 0; i < snakes.Length; i++)
        {
            if(snakes[i].getSize() == sizeOfLargestSnake)
            {
                snakes[i].hasWonRound();
            }

            if(snakes[i].getSize() == sizeOfSmalestSnake)
            {
                snakes[i].hasLostRound();
            }
        }
    }

    public void gameRoundHasEndet()
    {
        if (!gameHasEndedInADrawn())
        {
            endGameRoundRewarts();
        }
    }

    public void reset()
    {
        Debug.Log("try reset");

        if (checkIfAllSnakesAreDead())
        {
            gameRoundHasEndet();

            for (int x = 0; x < with; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Token token = field[y, x].token;

                    if (token != null)
                    {
                        token.deliteToken();
                    }

                }
            }

            addTokens();


            for (int i = 0; i < snakes.Length; i++)
            {
                snakes[i].endAIEpisode();
                snakes[i].reset();
            }


        }
    }


    // Start is called before the first frame update
    void Start()
    {
        if(numberOfSnakes > 10)
        {
            numberOfSnakes = 10;
        }

        tile = Resources.Load<Tile>("TileObject");
        field = new Tile[height, with];
        moveCamera();
        init();
        initSnakes();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < snakes.Length; i++)
        {
            snakes[i].turn();
        }
    }

}
