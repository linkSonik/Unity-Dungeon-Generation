using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRooms : MonoBehaviour
{

    public LayerMask isRoom;
    public LevelGenerator levelGen;
    
    void Update()
    {
        Collider2D roomDetector = Physics2D.OverlapCircle(transform.position, 1, isRoom);
        if(roomDetector == null && levelGen.stop == true)
        {
            int rand = Random.Range(0, levelGen.rooms.Length);
            Instantiate(levelGen.rooms[rand], transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
