using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private int playerLayer;
    [SerializeField]
    private int groundLayer;
    [SerializeField]
    private int wallLayer;

    [HideInInspector]
    public Bow bow;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    bool canDoDamage = true;
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Just standing up for myself here for some unearthly reason layer mask variable wasnt working here
        //I tried for ages

        //If the arrow is touching the player, pick it up
        if(other.gameObject.layer == playerLayer)
        {
            bow.PickUpArrow();
            Destroy(gameObject);
        }

        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        //If it hits an enemy kill it
        if (enemy != null && canDoDamage) {
            enemy.Die();
            rb.AddForce(transform.right * 3);
        } 

        //If its a wall stick in it and dont do damage anymore
        if(other.gameObject.layer == groundLayer || other.gameObject.layer == wallLayer)
        {
            Debug.Log("Yes");
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            canDoDamage = false;
        }
    }
}
