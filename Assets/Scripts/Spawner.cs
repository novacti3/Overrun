using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private GameMaster gm;

    [SerializeField]
    // Scriptable Object which holds all the available enemy types and their spawn chances per floor
    private EnemyTypesSO enemyTypesSO = null;
    // Array of the enemy types that are able to spawn on the current floor
    private EnemyType[] availableEnemyTypes = new EnemyType[0];

    [SerializeField]
    private float timeBetweenEnemySpawns = 1.5f;

    // Whether the SpawnEnemies coroutine is running or not
    // Necessary to keep the enemy spawns going throughout the gameplay
    private bool isCoroutineRunning = false;

    private void Start()
    {
        gm = GameMaster.Instance;

        availableEnemyTypes = ExtractAvailableEnemyTypes().ToArray();

        StartCoroutine(SpawnEnemies());
    }

    private void LateUpdate()
    {
        // Makes sure the coroutine is running when needed and doesn't end so it can keep spawning enemies
        if (!isCoroutineRunning && 
                gm.enemiesSpawnedAmount < gm.enemiesPerFloor[gm.currentFloor] &&
                gm.spawnedEnemies.Count < gm.maxEnemiesAtOnceAmountPerFloor[gm.currentFloor])
            StartCoroutine(SpawnEnemies());
    }

    // Returns all the available enemy types for spawning for the current floor
    private List<EnemyType> ExtractAvailableEnemyTypes()
    {
        int currentFloor = gm.currentFloor;

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
            Debug.Log($"Extracted {types.Count} enee fomy types for the current floor");
            return types;
        }
        else
        {
            Debug.LogError("No specified enemy types!");
            return null;
        }

    }

    private IEnumerator SpawnEnemies()
    {
        isCoroutineRunning = true;

        // Spawn the enemies when the numbers don't exceed stuff they shouldn't exceed
        while (gm.isGameInProgress && 
                gm.enemiesSpawnedAmount < gm.enemiesPerFloor[gm.currentFloor] &&
                gm.spawnedEnemies.Count < gm.maxEnemiesAtOnceAmountPerFloor[gm.currentFloor])
        {
            yield return new WaitForSeconds(timeBetweenEnemySpawns);

            GameObject enemyToSpawn = null;
            // Choose the enemy to spawn from all the enemy types based on their spawn chance
            foreach (EnemyType type in availableEnemyTypes)
            {
                int num = Random.Range(0, 100);
                float spawnChance = type.spawnChances[gm.currentFloor];

                if (num <= spawnChance)
                {
                    enemyToSpawn = type.enemyPrefab;
                }
            }

            if (enemyToSpawn != null)
            {
                gm.AddEnemyToList(Instantiate(enemyToSpawn, transform.position, Quaternion.identity));
                Debug.Log($"{enemyToSpawn.name} spawned at spawner {gameObject.name}!");
            }
            else
                Debug.LogWarning("No enemy spawned! Make sure your spawn chances ain't fucked up");
        }

        isCoroutineRunning = false;
    }
}
