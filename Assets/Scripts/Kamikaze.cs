using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamikaze : Enemy
{
    // Update is called once per frame
    void Update()
    {
        Vector2 moveDir = player.transform.position - transform.position;
        transform.right = moveDir;

        rb.velocity = transform.right * movementSpeed;
        
        if(Vector2.Distance(transform.position,player.transform.position) < 2) {
            Explode();
            //explode ani,
        }

    }

    public void Explode() {
            List<GameObject> enemies = GameMaster.Instance.spawnedEnemies;
            foreach(GameObject deadEnemy in enemies) {
            if(deadEnemy != null && Vector2.Distance(transform.position, deadEnemy.transform.position) < 5)
                deadEnemy.GetComponent<Enemy>().Die();
            }
            //Explode anim
            player.GetComponent<Player>().TakeDamage();
            Explode();

            base.Die();
        

    }
}
