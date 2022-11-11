using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snakeMoveFunction : MonoBehaviour
{
    //diese Klasse ist f�r eine fl�ssige bewegung der Schlangenteile zust�ndig
    //info: https://docs.unity3d.com/ScriptReference/Vector3.MoveTowards.html

    public SnakePart snakePart { get; set; }

    public float speed { get; set; }

    private Vector2 targedVector;

    private bool move = false;


    public void moveToSmoothly(Vector2 targetPosition)
    {
        //ToDO

        this.targedVector = targetPosition;
        move = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        speed = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {

        movment(Time.deltaTime);
        //if (move)
        //{
        //    float step = speed * Time.deltaTime;
        //    //Debug.Log("Pos= " + transform.position);
        //    //Debug.Log("Ziel= " + targedVector);
        //    transform.position = Vector2.MoveTowards(transform.position, targedVector, step);

        //    if (Vector2.Distance(transform.position, targedVector) < 0.01f)
        //    {

        //        Debug.Log("target reached");
        //        transform.position = targedVector;

        //        move = false;
        //        snakePart.movedToPositonCallBack((int)transform.position.x, (int)transform.position.y);
        //    }
        //}



    }

    void movment(float time)
    {
        if (move)
        {
            float step = speed * time;
            //Debug.Log("Pos= " + transform.position);
            //Debug.Log("Ziel= " + targedVector);
            transform.position = Vector2.MoveTowards(transform.position, targedVector, step);

            if (Vector2.Distance(transform.position, targedVector) < 0.01f)
            {

                Debug.Log("target reached");
                transform.position = targedVector;

                move = false;
                snakePart.movedToPositonCallBack((int)transform.position.x, (int)transform.position.y);
            }
        }
    }
}