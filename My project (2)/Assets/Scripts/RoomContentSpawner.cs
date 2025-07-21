using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoomContentSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public GameObject coinPrefab;
    public GameObject chestPrefab;

    public int minMonsters = 0;
    public int maxMonsters = 3;

    public int minCoins = 0;
    public int maxCoins = 3;

    private List<GameObject> spawnedMonsters = new List<GameObject>();
    private GameObject spawnedChest;

    private bool chestUnlocked = false;

    private SpawnFloor[] spawnFloors;

    void Start()
    {
        // ѕолучаем все точки спавна пола в комнате
        spawnFloors = GetComponentsInChildren<SpawnFloor>();

        SpawnMonsters();
        SpawnCoins();
        SpawnChest();
    }

    void SpawnMonsters()
    {
        int monsterCount = Random.Range(minMonsters, maxMonsters + 1);
        List<SpawnFloor> availableSpawns = new List<SpawnFloor>(spawnFloors);

        for (int i = 0; i < monsterCount && availableSpawns.Count > 0; i++)
        {
            int index = Random.Range(0, availableSpawns.Count);
            Vector3 spawnPos = availableSpawns[index].transform.position;
            GameObject monster = Instantiate(monsterPrefab, spawnPos, Quaternion.identity, transform);
            spawnedMonsters.Add(monster);

            Monster monsterScript = monster.GetComponent<Monster>();
            if (monsterScript != null)
            {
                monsterScript.OnDeath += OnMonsterDeath;
            }

            availableSpawns.RemoveAt(index); 
        }
    }

    void SpawnCoins()
    {
        int coinCount = Random.Range(minCoins, maxCoins + 1);
        List<SpawnFloor> availableSpawns = new List<SpawnFloor>(spawnFloors);

        for (int i = 0; i < coinCount && availableSpawns.Count > 0; i++)
        {
            int index = Random.Range(0, availableSpawns.Count);
            Vector3 spawnPos = availableSpawns[index].transform.position;
            Instantiate(coinPrefab, spawnPos, Quaternion.identity, transform);
            availableSpawns.RemoveAt(index);
        }
    }

    void SpawnChest()
    {
        if (spawnFloors.Length == 0) return;

        int index = Random.Range(0, spawnFloors.Length);
        Vector3 spawnPos = spawnFloors[index].transform.position;
        spawnedChest = Instantiate(chestPrefab, spawnPos, Quaternion.identity, transform);

        Chest chestScript = spawnedChest.GetComponent<Chest>();
        if (chestScript != null)
        {
            chestScript.Lock();
        }
    }

    void OnMonsterDeath(GameObject monster)
    {
        spawnedMonsters.Remove(monster);
        if (spawnedMonsters.Count == 0 && !chestUnlocked)
        {
            UnlockChest();
        }
    }

    void UnlockChest()
    {
        chestUnlocked = true;
        if (spawnedChest != null)
        {
            Chest chestScript = spawnedChest.GetComponent<Chest>();
            if (chestScript != null)
            {
                chestScript.Unlock();
            }
        }
    }
}
