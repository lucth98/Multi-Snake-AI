using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Grid : MonoBehaviour
{
    public Snake snake { get; private set; }

    public int height = 60;

    public int with = 60;

    public int numberOfIncreaseSizeTokens = 1;

    private Tile tile;

    private Tile[,] field;

    private IncreaseSizeToken increase;


    public void drawGrid()
    {
        for (int x = 0; x < with; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var newTile = Instantiate(tile, new Vector3(x, y, 1), Quaternion.identity);

                newTile.name = "Tile Pos: X=" + x + " Y=" + y;

                newTile.setCollor((x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0));

                field[y, x] = newTile;
            }
        }
    }

    private void makeBarres()
    {
        for (int i = 0; i < with; i++)
        {
            getTile(i, 0).makeBarrier();
        }
        for (int i = 0; i < height; i++)
        {
            getTile(0, i).makeBarrier();
        }
        for (int i = 0; i < with; i++)
        {
            getTile(i, height - 1).makeBarrier();
        }
        for (int i = 0; i < with; i++)
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

    // Start is called before the first frame update
    void Start()
    {
        tile = Resources.Load<Tile>("TileObject");
        field = new Tile[height, with];
        moveCamera();
        init();
        initSnake();
    }

   

    private void init()
    {
        drawGrid();
        makeBarres();
        addTokens();
       // initSnake();
    }

    private void initSnake()
    {
        snake = new Snake();
        snake.init(with / 2, height / 2, this);

        //ToDO RandPos

    }

    // Update is called once per frame
    void Update()
    {
        snake.turn();
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
    }

    private void addTokens()
    {
        increase = Resources.Load<IncreaseSizeToken>("IncreasSizeObject");

        for(int i = 0; i < numberOfIncreaseSizeTokens; i++)
        {
            addIncreaseToken();
        }
    }

    public void reset()
    {
        //DoTo


        for (int x = 0; x < with; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //toDo
                Token token = field[y, x].token;


                if(token != null)
                {
                    token.deliteToken();
                }

            }
        }

        addTokens();
    }



}
