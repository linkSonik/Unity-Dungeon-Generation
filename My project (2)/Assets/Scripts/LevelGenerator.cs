using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Transform[] startingPosition;
    public GameObject[] rooms;

    public static HashSet<Vector2> occupiedPositions = new HashSet<Vector2>();

    private int direction;
    private int downCount;
     
    public float moveAmount;
    private float timeRoom;
    public float startTimeRoom = 0.25f;

    public float minX;
    public float maxX;
    public float minY;
    public int maxRooms;
    public static int roomsCreated;

    public bool stop;

    public LayerMask room;


    private void Start()
    {
        int randStartingPos = Random.Range(0, startingPosition.Length);
        transform.position = startingPosition[randStartingPos].position;
        CreateRoom(transform.position, rooms[0]); 
        roomsCreated = 1;

        direction = Random.Range(1, 6);
    }

    private void Move()
    {
        if (roomsCreated >= maxRooms)
        {
            stop = true;
            return;
        }

        if (direction == 1 || direction == 2) // идем вправо
        {
            if (transform.position.x < maxX)
            {
                downCount = 0;
                Collider2D roomDetector = Physics2D.OverlapCircle(transform.position, 1, room);
                if (roomDetector != null && roomDetector.GetComponent<RoomType>().type == 4)
                {
                    roomDetector.GetComponent<RoomType>().RoomDestruction();
                    CreateRoom(transform.position, rooms[Random.Range(0, rooms.Length - 1)]);
                    roomsCreated++;
                }
                Vector2 newPose = new Vector2(transform.position.x + moveAmount, transform.position.y);
                transform.position = newPose;

                int rand = Random.Range(0, rooms.Length - 1);
                CreateRoom(transform.position, rooms[rand]);
                roomsCreated++;

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
                if (roomDetector != null && roomDetector.GetComponent<RoomType>().type == 4)
                {
                    roomDetector.GetComponent<RoomType>().RoomDestruction();
                    CreateRoom(transform.position, rooms[Random.Range(0, rooms.Length - 1)]);
                    roomsCreated++;
                }
                Vector2 newPose = new Vector2(transform.position.x - moveAmount, transform.position.y);
                transform.position = newPose;

                int rand = Random.Range(0, rooms.Length - 1);
                CreateRoom(transform.position, rooms[rand]);
                roomsCreated++;

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
            if (transform.position.y > minY)
            {
                Collider2D roomDetector = Physics2D.OverlapCircle(transform.position, 1, room);
                if (roomDetector != null && roomDetector.GetComponent<RoomType>().type != 1 && roomDetector.GetComponent<RoomType>().type != 3)
                {
                    if (downCount >= 2)
                    {
                        roomDetector.GetComponent<RoomType>().RoomDestruction();
                        CreateRoom(transform.position, rooms[4]);

                        roomsCreated++;
                    }
                    else
                    {
                        roomDetector.GetComponent<RoomType>().RoomDestruction();
                        int randBottomRoom = Random.Range(1, 3);
                        if (randBottomRoom == 2)
                        {
                            randBottomRoom = 1;
                        }
                        CreateRoom(transform.position, rooms[randBottomRoom]); 
                        roomsCreated++;
                    }
                }

                Vector2 newPose = new Vector2(transform.position.x, transform.position.y - moveAmount);
                transform.position = newPose;

                int rand = Random.Range(2, 4);
                CreateRoom(transform.position, rooms[rand]);
                roomsCreated++;

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
        if (timeRoom <= 0 && stop == false)
        {
            Move();
            timeRoom = startTimeRoom;
        }
        else
        {
            timeRoom -= Time.deltaTime;
        }
    }

    private void CreateRoom(Vector2 position, GameObject roomPrefab)
    {
        Instantiate(roomPrefab, position, Quaternion.identity);
        occupiedPositions.Add(position);
    }
}