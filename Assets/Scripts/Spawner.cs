using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    // Scriptable Object which holds all the available enemy types and their spawn chances per floor
    private EnemyTypesSO enemyTypesSO = null;
    // Array of the enemy types that are able to spawn on the current floor
    private EnemyType[] availableEnemyTypes = new EnemyType[0];

    [SerializeField]
    private float timeBetweenEnemySpawns = 1.5f;

    private void Start()
    {
        availableEnemyTypes = ExtractAvailableEnemyTypes().ToArray();

        StartCoroutine(SpawnEnemies());
    }

    // Returns all the available enemy types for spawning for the current floor
    private List<EnemyType> ExtractAvailableEnemyTypes()
    {
        int currentFloor = GameMaster.Instance.currentFloor;
        List<EnemyType> types = new List<EnemyType>();
        if (enemyTypesSO.enemyTypes.Length > 0)
        {
            foreach (EnemyType type in enemyTypesSO.enemyTypes)
            {
                if (type.spawnChances[currentFloor] > 0)
                {
                    types.Add(type);
                }
            }
            Debug.Log($"Extracted {types.Count} enemy types for the current floor");
            return types;
        }
        else
        {
            Debug.LogError("No specified enemy types!");
            return null;
        }

    }

    IEnumerator SpawnEnemies()
    {
        int currentFloor = GameMaster.Instance.currentFloor;

        while (GameMaster.Instance.spawnedEnemies.Count < GameMaster.Instance.maxEnemyAmount)
        {
            yield return new WaitForSeconds(timeBetweenEnemySpawns);

            GameObject enemyToSpawn = null;
            foreach (EnemyType type in availableEnemyTypes)
            {
                int num = Random.Range(0, 100);
                float spawnChance = type.spawnChances[currentFloor];

                if (num <= spawnChance)
                {
                    enemyToSpawn = type.enemyPrefab;
                }
            }

            if (enemyToSpawn != null)
            {
                GameMaster.Instance.spawnedEnemies.Add(Instantiate(enemyToSpawn, transform.position, Quaternion.identity));
                Debug.Log($"{enemyToSpawn.name} spawned at spawner {gameObject.name}!");
            }
            else
                Debug.LogWarning("No enemy spawned! Make sure your spawn chances ain't fucked up");
        }
    }
}
