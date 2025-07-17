using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTile : MonoBehaviour
{
    public GameObject[] objects;

    private void Start()
    {
        int rand = Random.Range(0, objects.Length);
        GameObject instanse = (GameObject)Instantiate(objects[rand], transform.position, Quaternion.identity);
        instanse.transform.parent = transform;
    }

}
