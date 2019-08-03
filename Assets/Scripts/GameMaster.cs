using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance;

    public Transform player = null;
    [HideInInspector]
    // The floor the player is on right now
    public int currentFloor = 0;
    [HideInInspector]
    // Whether the game is in progress or not
    public bool isGameInProgress = true;

    // How many enemies there are in total per floor
    public int[] enemiesPerFloor = new int[0];
    // How many enemies there can be at once per floor
    public int[] maxEnemiesAtOnceAmountPerFloor = new int[0];
    [HideInInspector]
    // List of all the enemies in the room
    public List<GameObject> spawnedEnemies = new List<GameObject>();
    [HideInInspector]
    // How many enemies have spawned (both alive and already dead) on the floor so far
    public int enemiesSpawnedAmount = 0;

    [SerializeField]
    private GameObject keyPrefab = null;
    [SerializeField]
    private float keySpawnJumpForce = 5f;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    /*private void Start()
    {
        if (!player)
            player = FindObjectOfType<Player>().transform;    
    }*/

    private void LateUpdate()
    {
        if (spawnedEnemies.Count > 0)
            Debug.Log($"Enemies spawned: {enemiesSpawnedAmount}/{enemiesPerFloor[currentFloor]}");

        if (maxEnemiesAtOnceAmountPerFloor.Length > 0 && 
                spawnedEnemies.Count >= maxEnemiesAtOnceAmountPerFloor[currentFloor])
            Debug.Log("Max enemies spawned!");

        // DEBUG: Testing the whole addition and removal and spawning of enemies
        if(spawnedEnemies.Count > 0 && Input.GetKeyDown(KeyCode.K))
        {
            Destroy(spawnedEnemies[0]);
            RemoveEnemyFromList(spawnedEnemies[0]);
        }
    }

    // Adds an enemy to the spawned enemies list
    // Also ensures the amount of spawned enemies doesn't exceed the max allowed number of enemies in the room at once
    public void AddEnemyToList(GameObject enemy)
    {
        if (spawnedEnemies.Count >= maxEnemiesAtOnceAmountPerFloor[currentFloor])
        {
            Destroy(enemy);
            return;
        }

        spawnedEnemies.Add(enemy);
        enemiesSpawnedAmount++;   
    }

    // Removes the enemy from the spawned enemies list
    public void RemoveEnemyFromList(GameObject enemy)
    {
        spawnedEnemies.Remove(enemy);
        if (spawnedEnemies.Count <= 0 && enemiesSpawnedAmount >= enemiesPerFloor[currentFloor])
        {
            GameObject key = Instantiate(keyPrefab, enemy.transform.position, Quaternion.identity);
            key.GetComponent<Rigidbody2D>().AddForce(Vector2.up * keySpawnJumpForce, ForceMode2D.Impulse);
        }
    }
}
