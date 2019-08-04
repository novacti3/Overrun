using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance;

    [SerializeField]
    private GameObject playerPrefab = null;
    [HideInInspector]
    public Transform player = null;
    private GameObject exitDoor = null;
    [SerializeField]
    private GameObject keyPrefab = null;
    private GameObject spawnedKey = null;
    [SerializeField]
    private float keySpawnJumpForce = 5f;

    public int[] floorSceneIndexes = new int[0];
    [HideInInspector]
    // The floor the player is on right now
    public int currentFloor = 0;
    [HideInInspector]
    // Whether the game is in progress or not
    public bool isGameInProgress = false;

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

    [HideInInspector]
    public int kills = 0;

    [SerializeField]
    Camera camera;

    [SerializeField]
    GameObject[] hearts;
    int heartCount = 0;

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
        SceneManager.sceneLoaded += FloorSetup;

        Key.OnKeyPickedUp += UnlockExitDoor;
        ExitDoor.OnExitDoorUsed += AdvanceToNextFloor;
        Player.OnPlayerDied += EndGame;

        FloorSetup(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }
    void Update() {
        camera.transform.position = new Vector3(0,0,-10);
    }
    private void LateUpdate()
    {
        if (spawnedEnemies.Count > 0)
            Debug.Log($"Enemies spawned: {enemiesSpawnedAmount}/{enemiesPerFloor[currentFloor]}");

        if (maxEnemiesAtOnceAmountPerFloor.Length > 0 &&
                spawnedEnemies.Count >= maxEnemiesAtOnceAmountPerFloor[currentFloor])
            Debug.Log("Max enemies spawned!");

        // DEBUG: Testing the whole addition and removal and spawning of enemies
        if (spawnedEnemies.Count > 0 && Input.GetKeyDown(KeyCode.K))
        {
            Destroy(spawnedEnemies[0]);
            RemoveEnemyFromList(spawnedEnemies[0]);
        }
    }

    private void FloorSetup(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Setting up floor");

        exitDoor = FindObjectOfType<ExitDoor>().gameObject;

        player = Instantiate(playerPrefab, exitDoor.transform.position, Quaternion.identity).transform;

        enemiesSpawnedAmount = 0;
        spawnedEnemies.Clear();

        isGameInProgress = true;
    }

    private void UnlockExitDoor()
    {
        exitDoor.GetComponent<ExitDoor>().isUnlocked = true;
        Debug.Log("Exit door unlocked");
    }

    private void AdvanceToNextFloor()
    {
        Debug.Log("Advancing to the next floor");
        currentFloor++;
        if (currentFloor < floorSceneIndexes.Length)
            SceneManager.LoadScene(floorSceneIndexes[currentFloor]);
        else
            SceneManager.LoadScene(6);
    }

    private void EndGame()
    {
        SceneManager.LoadScene(7);
        SceneManager.sceneLoaded -= FloorSetup;
        Destroy(gameObject);
    }

    // Adds an enemy to the spawned enemies list
    // Also ensures the amount of spawned enemies doesn't exceed the max allowed number of enemies in the room at once
    public void AddEnemyToList(GameObject enemy)
    {
        if (enemiesSpawnedAmount >= enemiesPerFloor[currentFloor])
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
            spawnedKey = Instantiate(keyPrefab, enemy.transform.position, Quaternion.identity);
            spawnedKey.GetComponent<Rigidbody2D>().AddForce(Vector2.up * keySpawnJumpForce, ForceMode2D.Impulse);
        }
    }

    public void CamShake(float time, float intensity) {
        Vector3 originalCamPos = camera.transform.position;
        IEnumerator couroutine  = CameraShake(time, intensity, originalCamPos);
        StartCoroutine(couroutine);
    }
    IEnumerator CameraShake(float time, float intensity, Vector3 originalCamPos) {
		
        for(float i = 0; i < time; i += Time.fixedDeltaTime){
            
			camera.transform.position +=Random.insideUnitSphere * intensity;
			

            yield return new WaitForEndOfFrame();
        }
		
	    camera.transform.localPosition = originalCamPos;
		
        
    }

    public void PlayerDamage() {
        hearts[heartCount].GetComponent<Animator>().SetTrigger("hurt");
        heartCount++;
    }
}
