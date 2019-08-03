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

    public bool boomArrow;

    public bool rollArrow;

    GameMaster gameMaster;


    private void Start()
    {
        gameMaster = GameMaster.Instance;
        rb = GetComponent<Rigidbody2D>();

    }
    private void Update() {
        if(rb.velocity.x > 0.1f && rb.velocity.y > 0.1f) {
            Vector2 dir = transform.GetComponent<Rigidbody2D>().velocity;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
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
        } else if(boomArrow){
            List<GameObject> enemies = gameMaster.spawnedEnemies;
            foreach(GameObject deadEnemy in enemies) {
                if(deadEnemy != null && Vector2.Distance(transform.position, deadEnemy.transform.position) < 10)
                    deadEnemy.GetComponent<Enemy>().Die();
            }
        }

        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        //If it hits an enemy kill it
        if (enemy != null && canDoDamage) {
            enemy.Die();
        } 

        //If its a wall stick in it and dont do damage anymore
        if(other.gameObject.layer == groundLayer || other.gameObject.layer == wallLayer)
        {       
            if(rollArrow && bounceCount > 0) {
                rb.velocity = -rb.velocity;
                bounceCount --;
                return;
            }
                GetComponent<PolygonCollider2D>().enabled=  false;
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
                canDoDamage = false;
            
        }
    }
}
