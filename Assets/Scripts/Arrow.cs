using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private GameMaster gameMaster;
    private Rigidbody2D rb;
    public Bow bow;

    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private int groundLayer = 8;
    [SerializeField]
    private int wallLayer = 9;
    [SerializeField]
    private int shieldLayer = 13;

    int bounceCount = 1;

    [HideInInspector]
    public bool boomArrow = false;
    [HideInInspector]

    bool canDoDamage = true;

    private void Start()
    {
        gameMaster = GameMaster.Instance;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(GetComponent<Rigidbody2D>()) {
            Vector2 dir = rb.velocity;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
       
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        //Just standing up for myself here for some unearthly reason layer mask variable wasnt working here
        //I tried for ages

        //If the arrow is touching the player, pick it up
        if (other.gameObject.layer == 11 || other.gameObject.layer == 16)
        {
            bow.PickUpArrow();
            Destroy(gameObject);
        }

        if (boomArrow)
        {
            List<GameObject> enemies = gameMaster.spawnedEnemies;
            foreach (GameObject deadEnemy in enemies)
            {
                if (deadEnemy != null)
                    deadEnemy.GetComponent<Enemy>().Die();
                    gameMaster.kills = 0;
                
            }

            boomArrow = false;

        }

        if(other.gameObject.layer == wallLayer || other.gameObject.layer == groundLayer) {
            rb.velocity=Vector2.zero;
            Destroy(rb);
            canDoDamage = false;
        }

        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        //If it hits an enemy kill it
        if (enemy != null && canDoDamage)
        {
            enemy.Die();
            gameMaster.CamShake(0.2f, 0.5f);
        }

        //If its a wall stick in it and dont do damage anymore


        if (other.gameObject.layer == shieldLayer && bounceCount > 0)
        {
            rb.velocity = -rb.velocity;
            bounceCount--;
            ShieldBearer bearer = other.transform.root.GetComponent<ShieldBearer>();
            if(bearer)
                bearer.DamageShield();
        }
    }
}
