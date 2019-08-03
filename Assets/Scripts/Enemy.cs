using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Transform player;

    [SerializeField]
    protected float movementSpeed = 3f;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameMaster.Instance.player;
    }
    
    protected virtual void FixedUpdate()
    {
        if (player != null)
            Move();
    }

    protected virtual void Move()
    {
        // Direction from the enemy to the player along the X axis
        Vector2 moveDir = new Vector2(player.position.x - transform.position.x, transform.position.y);
        // Moves the enemy towards the player and clamps the X velocity so it doesn't exceed the movement speed
        rb.velocity = new Vector2(Mathf.Clamp(moveDir.x * movementSpeed, -movementSpeed, movementSpeed), rb.velocity.y);
    }

    public virtual void Die()
    {
        GameMaster.Instance.killsToPowerArrow += 1;
        GameMaster.Instance.RemoveEnemyFromList(gameObject);
        // TEMPORARY! Replace with the whole falling-off-the-screen-in-ouch-position bs
        Destroy(gameObject);
    }
}