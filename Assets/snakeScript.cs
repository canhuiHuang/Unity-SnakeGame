using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snakeScript : MonoBehaviour {

    public GameObject tail;    //Debug purpose
    public GameObject head;
    public GameObject bodyBlock;
    public List<GameObject> B = new List<GameObject>(); //List de bodyBlocks 


    //These set the amount of time per movement.
    public float secPerMove;    //The lower the secPerMove, the faster the snake moves.
    float nextMoveTime;

    //Variables to save direction, position
    Vector3 direction;
    enum dir { up, down, right, left };
    dir enumdir;

    Vector3 imaginaryTail;
    public bool debugModeOn;

    //Functions
    void dirInput()
    {
        if (Input.GetKeyDown("w") && (enumdir != dir.down || B.Count == 0))
        {
            direction = new Vector3(0, 1, 0);
            enumdir = dir.up;
            print("direction es ahora UP.");
        }
        if (Input.GetKeyDown("s") && (enumdir != dir.up || B.Count == 0))
        {
            direction = new Vector3(0, -1, 0);
            enumdir = dir.down;
            print("direction es ahora DOWN.");
        }
        if (Input.GetKeyDown("d") && (enumdir != dir.left || B.Count == 0))
        {
            direction = new Vector3(1, 0, 0);
            enumdir = dir.right;
            print("direction es ahora RIGHT.");
        }
        if (Input.GetKeyDown("a") && (enumdir != dir.right || B.Count == 0))
        {
            direction = new Vector3(-1, 0, 0);
            enumdir = dir.left;
            print("direction es ahora LEFT.");
        }
    }
    void grow()
    {
        bodyBlock = Instantiate(bodyBlock, imaginaryTail, Quaternion.identity);
        B.Add(bodyBlock);
    }

    void Start ()
    {
        Time.timeScale = 1;
        //Set up variables
        direction = new Vector3(1, 0, 0);
        enumdir = dir.right;
        nextMoveTime = 0;
        imaginaryTail = head.transform.position - direction;

        //Instantiate head
        Vector3 pos = new Vector3(4.5f, 6.5f, -10);
        head = Instantiate(head, pos, Quaternion.identity);
        if(debugModeOn)
            tail = Instantiate(tail, pos, Quaternion.identity);

        //Event catchers
        head.GetComponent<Collision>().foodTaken += grow;
    }
	
	void Update ()
    {
        //Direction input
        dirInput();

        //Debug Mode
        //Test Growth (Debug Mode)
        if (debugModeOn && Input.GetKeyDown("space"))
        {
            grow();
            print("Grown to count: " + B.Count);    //Debug
        }


    }

    void FixedUpdate()
    {
        //Translate Fixed Distance
        if (Time.time > nextMoveTime)
        {
            //Move rest of body first.
            if (B.Count > 0)
            {
                imaginaryTail = B[B.Count - 1].transform.position;
                for (int i = B.Count - 1; i >= 0; i--)
                {
                    if (i == 0)
                    {
                        //B[i].transform.position = head.transform.position;
                        B[i].GetComponent<Rigidbody>().position = head.transform.position;
                    }

                    else
                    {
                        //B[i].transform.position = B[i - 1].transform.position;
                        B[i].GetComponent<Rigidbody>().position = B[i - 1].transform.position;
                    }

                }
                if (debugModeOn)    //ImaginaryTail
                    tail.transform.position = imaginaryTail;
            }
            else
            {
                imaginaryTail = head.transform.position;
                if (debugModeOn)    //ImaginaryTail
                    tail.transform.position = imaginaryTail;
            }
            //Now move head.
            //head.transform.position += direction;
            head.GetComponent<Rigidbody>().position += direction;

            //Teleport through walls
            if (head.transform.position.x > 10)
            {
                head.GetComponent<Rigidbody>().position = new Vector3(0.5f, head.transform.position.y, -10);
            }
            else if (head.transform.position.x < 0)
            {
                head.GetComponent<Rigidbody>().position = new Vector3(9.5f, head.transform.position.y, -10);
            }

            else if (head.transform.position.y > 13)
            {
                head.GetComponent<Rigidbody>().position = new Vector3(head.transform.position.x, 0.5f, -10);
            }
            else if (head.transform.position.y < 0)
            {
                head.GetComponent<Rigidbody>().position = new Vector3(head.transform.position.x, 12.5f, -10);
            }


            //Freeze movement for secPerMove seconds.
            nextMoveTime = Time.time + secPerMove;
        }


    }
}

