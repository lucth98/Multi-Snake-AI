using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snakeMoveFunction : MonoBehaviour
{
    //diese Klasse ist für eine flüssige bewegung der Schlangenteile zuständig
    //info: https://docs.unity3d.com/ScriptReference/Vector3.MoveTowards.html



    public float speed = 2.0f;

    public SnakePart snakePart { get; set; }

  //  public float speed { get; set; }

    private Vector2 targedVector;

    private bool move = false;


    public void setSpeed(float speed)
    {
        this.speed = speed; 
    }

    public float getSpeed() { return speed; }

    public void moveToSmoothly(Vector2 targetPosition)
    {
        //ToDO

        this.targedVector = targetPosition;
        move = true;
    }

    public void stopMovement()
    {
        move = false;
   
    }

    public void startMovment()
    {
        move = true;
    }

    // Start is called before the first frame update
    void Start()
    {
     //   speed = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        movment(Time.deltaTime);
    }

    void movment(float time)
    {
        if (move && targedVector != null)
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
