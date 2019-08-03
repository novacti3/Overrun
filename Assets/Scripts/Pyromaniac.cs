using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pyromaniac : Kamikaze
{

    // Update is called once per frame
    void Update()
    {
        // Direction from the enemy to the player along the X axis
        Vector2 moveDir = new Vector2(player.position.x - transform.position.x, transform.position.y);
        // Moves the enemy towards the player and clamps the X velocity so it doesn't exceed the movement speed
        rb.velocity = new Vector2(Mathf.Clamp(moveDir.x * movementSpeed, -movementSpeed, movementSpeed), rb.velocity.y);
        
        if(Vector2.Distance(transform.position,player.transform.position) < 2) {
            Explode();
            //explode ani,
        }
    }
}
