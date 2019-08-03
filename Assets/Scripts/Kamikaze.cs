using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamikaze : Pyromaniac
{
    protected override void Move()
    {
        Vector2 moveDir = player.transform.position - transform.position;
        transform.right = moveDir;

        rb.velocity = transform.right * movementSpeed;
    }
}
