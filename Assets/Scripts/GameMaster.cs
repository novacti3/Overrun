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

    // The max amount of enemies that can be in the room at once
    public int maxEnemyAmount = 25;
    [HideInInspector]
    // List of all the enemies in the room
    public List<GameObject> spawnedEnemies = new List<GameObject>();

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    private void Start()
    {
        if (!player)
            player = FindObjectOfType<Player>().transform;    
    }

    private void LateUpdate()
    {
        if (spawnedEnemies.Count >= maxEnemyAmount)
            Debug.Log("Max enemies spawned!");
    }
}
