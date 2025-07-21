using System.Collections.Generic;
using UnityEngine;

public class SpawnRooms : MonoBehaviour
{
    public LayerMask isRoom;
    public LevelGenerator levelGen;

    private bool hasSpawned = false;

    // Смещения соседних клеток
    private Vector2[] directions = new Vector2[]
    {
        new Vector2(1, 0),
        new Vector2(-1, 0),
        new Vector2(0, 1),
        new Vector2(0, -1)
    };

    void Update()
    {
        if (hasSpawned) return;

        // Проверяем, что текущая позиция свободна (нет комнаты)
        Collider2D roomDetector = Physics2D.OverlapCircle(transform.position, 1f, isRoom);
        if (roomDetector != null)
        {
            // Если комната уже есть здесь, удаляем спавнер
            Destroy(gameObject);
            return;
        }

        if (levelGen.stop == true)
        {
            // Ищем соседние позиции от уже занятых комнат
            List<Vector2> possiblePositions = new List<Vector2>();

            foreach (Vector2 pos in LevelGenerator.occupiedPositions)
            {
                foreach (Vector2 dir in directions)
                {
                    Vector2 neighborPos = pos + dir * levelGen.moveAmount; // moveAmount — размер шага между комнатами
                    if (!LevelGenerator.occupiedPositions.Contains(neighborPos))
                    {
                        possiblePositions.Add(neighborPos);
                    }
                }
            }

            if (possiblePositions.Count > 0)
            {
                // Выбираем случайную свободную соседнюю позицию
                Vector2 spawnPos = possiblePositions[Random.Range(0, possiblePositions.Count)];

                // Создаём комнату в выбранной позиции
                int rand = Random.Range(0, levelGen.rooms.Length);
                Instantiate(levelGen.rooms[rand], spawnPos, Quaternion.identity);

                LevelGenerator.occupiedPositions.Add(spawnPos);

                hasSpawned = true;
                Destroy(gameObject);
            }
            else
            {
                // Нет свободных соседних позиций — удаляем спавнер
                Destroy(gameObject);
            }
        }
    }
}