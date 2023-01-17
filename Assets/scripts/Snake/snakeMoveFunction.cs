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
       // Debug.Log("moveToSmoothly");
        this.targedVector = targetPosition;
        move = true;
    }

    public bool isMoving()
    {
       // Debug.Log(targedVector);
        return move || !targedVector.Equals(Vector2.zero);
    }

    public void stopMovement()
    {
        
        targedVector = Vector2.zero;
        move = false;

      //  Debug.Log("stop movement"+"targetV= "+targedVector+" move= "+ move);
    }

    public void startMovment()
    {
      //  Debug.Log("start");
        move = true;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        movment(Time.deltaTime);
    }

    void movment(float time)
    {
        if (!move)
        {
            return;
        }
        if (targedVector == null || targedVector.Equals(Vector2.zero))
        {
            move = false;
            return;
        }

       
        float step = speed * time;
        //Debug.Log("Pos= " + transform.position);
        //Debug.Log("Ziel= " + targedVector);
        transform.position = Vector2.MoveTowards(transform.position, targedVector, step);

        if (Vector2.Distance(transform.position, targedVector) < 0.01f)
        {

           // Debug.Log("target reached");
            transform.position = targedVector;

            move = false;
            snakePart.movedToPositonCallBack((int)transform.position.x, (int)transform.position.y);
        }

    }
}
