using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Transform[] startingPosition;
    public GameObject[] rooms;

    private int direction;
    private int downCount;
    public float moveAmount;
    private float timeRoom;
    public float startTimeRoom = 0.25f;

    public float minX;
    public float maxX;
    public float minY;

    public bool stop;

    public LayerMask room;


    private void Start()
    {
        int randStartingPos = Random.Range(0, startingPosition.Length);
        transform.position = startingPosition[randStartingPos].position;
        Instantiate(rooms[0], transform.position, Quaternion.identity);

        direction = Random.Range(1, 6);
    }

    private void Move()
    {
        if (direction == 1 || direction == 2) // идем вправо
        {
            if(transform.position.x < maxX)
            {
                downCount = 0;
                Collider2D roomDetector = Physics2D.OverlapCircle(transform.position, 1, room);
                if (roomDetector.GetComponent<RoomType>().type == 4)
                {
                    roomDetector.GetComponent<RoomType>().RoomDestruction();
                    Instantiate(rooms[Random.Range(0, rooms.Length - 1)], transform.position, Quaternion.identity);

                }
                Vector2 newPose = new Vector2(transform.position.x + moveAmount, transform.position.y);
                transform.position = newPose;

                int rand = Random.Range(0, rooms.Length - 1);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                direction = Random.Range(1, 6);
                if (direction == 3)
                {
                    direction = 2;
                }
                else if (direction == 4)
                {
                    direction = 5;
                }
            }
            else
            {
                direction = 5;
            }
            
        } 
        else if (direction == 3 || direction == 4) // идем влево
        {
            if (transform.position.x > minX)
            {
                downCount = 0;
                Collider2D roomDetector = Physics2D.OverlapCircle(transform.position, 1, room);
                if (roomDetector.GetComponent<RoomType>().type == 4)
                {
                    roomDetector.GetComponent<RoomType>().RoomDestruction();
                    Instantiate(rooms[Random.Range(0, rooms.Length - 1)], transform.position, Quaternion.identity);

                }
                Vector2 newPose = new Vector2(transform.position.x - moveAmount, transform.position.y);
                transform.position = newPose;

                int rand = Random.Range(0, rooms.Length - 1);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                direction = Random.Range(3, 6);


            }
            else
            {
                direction = 5;
            }
            
        }
        else if (direction == 5) // идем вниз
        {

            downCount++;
            if(transform.position.y > minY)
            {
                Collider2D roomDetector = Physics2D.OverlapCircle(transform.position, 1, room);
                if(roomDetector.GetComponent<RoomType>().type != 1 && roomDetector.GetComponent<RoomType>().type != 3)
                {
                    if (downCount >= 2)
                    {
                        roomDetector.GetComponent<RoomType>().RoomDestruction();
                        Instantiate(rooms[4], transform.position, Quaternion.identity);
                    }
                    else
                    {
                        roomDetector.GetComponent<RoomType>().RoomDestruction();
                        int randBottomRoom = Random.Range(1, 3);

                        if (randBottomRoom == 2)
                        {
                            randBottomRoom = 1;
                        }
                        Instantiate(rooms[randBottomRoom], transform.position, Quaternion.identity);
                    }


                    
                }


                    Vector2 newPose = new Vector2(transform.position.x, transform.position.y - moveAmount);
                transform.position = newPose;

                int rand = Random.Range(2, 4);
               
                
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                direction = Random.Range(1, 6);
            }
            else
            {
                stop = true;
            }
            
        }

        
    }


    void Update()
    {
        if(timeRoom <= 0 && stop == false)
        {
            Move();
            timeRoom = startTimeRoom;
        }
        else
        {
            timeRoom -= Time.deltaTime;
        }
    }
}
