using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pyromaniac : Enemy
{
    // How far the player has to be from the enemy for the enemy to explode
    public float distanceFromPlayerToExplode = 2f;
    public float explosionRadius = 5f;
    bool exploded = false;

    // Update is called once per frame
    protected virtual void Update()
    {
        if (gm.player != null && Vector2.Distance(transform.position, gm.player.position) < distanceFromPlayerToExplode && !exploded    )
        {
            
            GetComponent<Animator>().SetTrigger("Explode");
            //explode ani,
        }
    }

    public virtual void Explode()
    {
        dead = true;
        gm.CamShake(1f, 0.5f);
        List<GameObject> enemies = GameMaster.Instance.spawnedEnemies;
        foreach (GameObject enemyInRange in enemies)
        {
            if (enemyInRange != null && Vector2.Distance(transform.position, enemyInRange.transform.position) < explosionRadius &&
            enemyInRange != gameObject)
                enemyInRange.GetComponent<Enemy>().Die();
        }
        //Explode anim
        if(Vector2.Distance(transform.position,  gm.player.transform.position) < explosionRadius && !exploded) { 
            
            exploded = true;
            gm.player.GetComponent<Player>().TakeDamage();
        }
        GetComponent<Animator>().enabled = false;
        
        base.Die();
    }
}
