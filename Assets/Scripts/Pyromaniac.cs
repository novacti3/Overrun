using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pyromaniac : Enemy
{
    // How far the player has to be from the enemy for the enemy to explode
    public float distanceFromPlayerToExplode = 2f;
    public float explosionRadius = 5f;

    [SerializeField]
    private GameObject explosion;

    // Update is called once per frame
    protected virtual void Update()
    {
        if (gm.player != null && Vector2.Distance(transform.position, gm.player.position) < distanceFromPlayerToExplode)
        {
            Explode();
            //explode ani,
        }
    }

    public virtual void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        List<GameObject> enemies = GameMaster.Instance.spawnedEnemies;
        foreach (GameObject enemyInRange in enemies)
        {
            if (enemyInRange != null && Vector2.Distance(transform.position, enemyInRange.transform.position) < explosionRadius)
                enemyInRange.GetComponent<Enemy>().Die();
        }
        //Explode anim
        gm.player.GetComponent<Player>().TakeDamage();
        if(Vector2.Distance(transform.position,  gm.player.transform.position) < explosionRadius) { 
             gm.player.GetComponent<Player>().TakeDamage();
        }
        base.Die();
    }
}
