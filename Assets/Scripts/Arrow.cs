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

    int bounceCount = 1;


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
        } 

        //If its a wall stick in it and dont do damage anymore
        if(other.gameObject.layer == groundLayer || other.gameObject.layer == wallLayer)
        {   
            //if(bounceCount > 0) {
            //    RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 1f);
                
            //    rb.velocity = Vector3.Reflect(rb.velocity, hit.normal);
                
            //    
                
            //    Debug.Log(hit.normal);
            //    
            }
            //if(bounceCount <= 0) {
                GetComponent<PolygonCollider2D>().enabled=  false;
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
                canDoDamage = false;
            }
        }
    }
}
