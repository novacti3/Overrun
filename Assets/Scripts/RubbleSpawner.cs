using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbleSpawner : MonoBehaviour
{
    private GameMaster gm;

    public GameObject rubblePrefab;
    public float rubbleSpawnChance = 15f;
    public float timeBetweenRubbleBatchSpawn = 5f;
    public int rubblePerSpawn = 3;
    public float timeBetweenRubblePiecesSpawn = 0.5f;

    private bool canSpawnRubble = true;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameMaster.Instance;

        StartCoroutine(SpawnRubble());
    }

    private IEnumerator SpawnRubble()
    {
        while (gm.isGameInProgress)
        {
            if (!canSpawnRubble)
            {
                yield return new WaitForSeconds(timeBetweenRubbleBatchSpawn);
                canSpawnRubble = true;
            }

            int num = Random.Range(0, 100);
            if (canSpawnRubble && num <= rubbleSpawnChance)
            {
                if (rubblePerSpawn > 1)
                {
                    for (int i = 0; i < rubblePerSpawn; i++)
                    {
                        Instantiate(rubblePrefab, transform.position, Quaternion.identity);
                        yield return new WaitForSeconds(timeBetweenRubblePiecesSpawn);
                    }
                }
                else
                    Instantiate(rubblePrefab, transform.position, Quaternion.identity);
                Debug.Log("Spawned rubble");

                canSpawnRubble = false;
            }

            yield return null;
        }
    }
}
