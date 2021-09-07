using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EnemyType
{
    public GameObject enemyPrefab = null;
    public float[] spawnChances = new float[5];
}

[CreateAssetMenu(fileName = "New Enemy Types", menuName = "Enemy Types")]
// Holds all the different enemy types in the game
public class EnemyTypesSO : ScriptableObject
{
    // Array of all the available enemy types that the game has to offer
    public EnemyType[] enemyTypes = new EnemyType[0];
}
